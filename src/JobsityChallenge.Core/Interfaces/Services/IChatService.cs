using JobsityChallenge.Core.DTOs;
using JobsityChallenge.Core.Parameters;
using JobsityChallenge.Core.Results;

namespace JobsityChallenge.Core.Interfaces.Services;

public interface IChatService
{
    Task<PagedResult<ChatRoomDto>> GetChatRoomsAsync(GetChatRoomsParameters parameters);
    Task<ChatRoomDto> CreateChatRoomAsync(string name);
}
