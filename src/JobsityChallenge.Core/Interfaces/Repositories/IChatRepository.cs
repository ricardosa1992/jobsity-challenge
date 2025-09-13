using JobsityChallenge.Core.Entities;
using JobsityChallenge.Core.Parameters;
using JobsityChallenge.Core.Results;

namespace JobsityChallenge.Core.Interfaces.Repositories;

public interface IChatRepository
{
    Task<ChatRoom?> GetChatRoomByNameAsync(string name);
    Task<PagedResult<ChatRoom>> GetChatRoomsAsync(GetChatRoomsParameters parameters);
    Task<ChatRoom> CreateChatRoomAsync(ChatRoom chatRoom);
}
