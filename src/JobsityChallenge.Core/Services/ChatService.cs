using JobsityChallenge.Core.DTOs;
using JobsityChallenge.Core.Entities;
using JobsityChallenge.Core.Interfaces.Repositories;
using JobsityChallenge.Core.Interfaces.Services;
using JobsityChallenge.Core.Parameters;
using JobsityChallenge.Core.Results;
using JobsityChallenge.Shared.Hubs;
using JobsityChallenge.Shared.MessageBroker.Events;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace JobsityChallenge.Core.Services;

public class ChatService(
    IChatRepository chatRepository,
    UserManager<IdentityUser> userManager,
    IPublishEndpoint publishEndpoint,
    IHubContext<ChatHub> hubContext) : IChatService
{
    public async Task<PagedResult<GetChatRoomDto>> GetChatRoomsAsync(GetChatRoomsParameters parameters)
    {
        var pagedResult = await chatRepository.GetChatRoomsAsync(parameters);

        return new PagedResult<GetChatRoomDto>
        {
            Items = pagedResult.Items.Select(r => new GetChatRoomDto(r.Id, r.Name)),
            PageSize = parameters.PageSize,
            TotalCount = pagedResult.TotalCount
        };
    }

    public async Task<GetChatRoomDto> CreateChatRoomAsync(CreateChatRoomDto request)
    {
        var chatRoom = new ChatRoom(request.Name);
        var createdRoom = await chatRepository.CreateChatRoomAsync(chatRoom);

        return new GetChatRoomDto(createdRoom.Id, createdRoom.Name);
    }

    public async Task<Result> SendMessageAsync(SendMessageDto messageDto)
    {
        var user = await userManager.FindByIdAsync(messageDto.UserId!);
        if (user == null) 
            return Errors.UserNotFound;

        var chatRoom = await chatRepository.GetChatRoomByIdAsync(messageDto.RoomId);
        if (chatRoom == null)
            return Errors.ChatRoomNotFound;

        if (messageDto.Content.StartsWith("/stock="))
        {
            await NotifyUsersAsync(messageDto, user);
            await publishEndpoint.Publish(new StockQuoteBotEvent(messageDto.UserId, messageDto.RoomId, messageDto.Content));
        }
        else
        {
            chatRoom.AddMessage(messageDto.Content, messageDto.UserId!);
            await chatRepository.SaveChangesAsync();

            await NotifyUsersAsync(messageDto, user);
        }
        
        return Result.Success();
    }

    private async Task NotifyUsersAsync(SendMessageDto messageDto, IdentityUser? user)
    {
        await hubContext.Clients
                        .Group(messageDto.RoomId.ToString())
                        .SendAsync("ReceiveMessage", messageDto.RoomId.ToString(), user.UserName, messageDto.Content);
    }

    public Task<IEnumerable<GetMessageDto>> GetMessagesAsync(int id)
    {
        return chatRepository.GetLastMessagesAsync(id);
    }
}
