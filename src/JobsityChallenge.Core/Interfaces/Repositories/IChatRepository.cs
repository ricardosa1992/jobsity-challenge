using JobsityChallenge.Core.DTOs;
using JobsityChallenge.Core.Entities;
using JobsityChallenge.Core.Parameters;
using JobsityChallenge.Core.Results;

namespace JobsityChallenge.Core.Interfaces.Repositories;

public interface IChatRepository
{
    Task<ChatRoom?> GetChatRoomByIdAsync(int id);
    Task<PagedResult<ChatRoom>> GetChatRoomsAsync(GetChatRoomsParameters parameters);
    Task<ChatRoom> CreateChatRoomAsync(ChatRoom chatRoom);
    Task<IEnumerable<GetMessageDto>> GetMessagesAsync(int chatRoomId);
    Task SaveChangesAsync();
}
