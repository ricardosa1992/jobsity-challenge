using JobsityChallenge.Core.DTOs;
using JobsityChallenge.Core.Entities;
using JobsityChallenge.Core.Interfaces.Repositories;
using JobsityChallenge.Core.Interfaces.Services;
using JobsityChallenge.Core.Parameters;
using JobsityChallenge.Core.Results;
using JobsityChallenge.Shared.MessageBroker.Events;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace JobsityChallenge.Core.Services;

public class ChatService(
    IChatRepository chatRepository,
    UserManager<IdentityUser> userManager,
    IPublishEndpoint publishEndpoint) : IChatService
{
    public async Task<PagedResult<ChatRoomDto>> GetChatRoomsAsync(GetChatRoomsParameters parameters)
    {
        var pagedResult = await chatRepository.GetChatRoomsAsync(parameters);

        return new PagedResult<ChatRoomDto>
        {
            Items = pagedResult.Items.Select(r => new ChatRoomDto(r.Id, r.Name)),
            PageSize = parameters.PageSize,
            TotalCount = pagedResult.TotalCount
        };
    }

    public async Task<ChatRoomDto> CreateChatRoomAsync(string name)
    {
        var chatRoom = new ChatRoom(name);
        var createdRoom = await chatRepository.CreateChatRoomAsync(chatRoom);

        return new ChatRoomDto(createdRoom.Id, createdRoom.Name);
    }

    public async Task<Result> SendMessageAsync(SendMessageDto messageDto)
    {
        var user = await userManager.FindByIdAsync(messageDto.UserId!);
        if (user == null) 
            return Errors.UserNotFound;

        var chatRoom = await chatRepository.GetChatRoomByIdAsync(messageDto.RoomId);
        if (chatRoom == null)
            return Errors.ChatRoomNotFound;

        chatRoom.AddMessage(messageDto.Content, messageDto.UserId!);
        await chatRepository.SaveChangesAsync();

        if (messageDto.Content.StartsWith("/stock="))
        {
           await publishEndpoint.Publish(new StockQuoteBotEvent(messageDto.UserId, messageDto.RoomId, messageDto.Content));
        }

        return Result.Success();
    }

    public Task<IEnumerable<GetMessageDto>> GetMessagesAsync(int id)
    {
        return chatRepository.GetMessagesAsync(id);
    }
}
