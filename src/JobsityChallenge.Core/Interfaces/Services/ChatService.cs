using JobsityChallenge.Core.DTOs;
using JobsityChallenge.Core.Entities;
using JobsityChallenge.Core.Interfaces.Repositories;
using JobsityChallenge.Core.Parameters;
using JobsityChallenge.Core.Results;

namespace JobsityChallenge.Core.Interfaces.Services;

public class ChatService(IChatRepository chatRepository) : IChatService
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
}
