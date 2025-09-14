using JobsityChallenge.Core.DTOs;
using JobsityChallenge.Core.Parameters;
using JobsityChallenge.Core.Results;

namespace JobsityChallenge.Core.Interfaces.Services;

public interface IChatService
{
    Task<PagedResult<GetChatRoomDto>> GetChatRoomsAsync(GetChatRoomsParameters parameters);
    Task<GetChatRoomDto> CreateChatRoomAsync(CreateChatRoomDto chatRoomDto);
    Task<Result> SendMessageAsync(SendMessageDto messageDto);
    Task<IEnumerable<GetMessageDto>> GetMessagesAsync(int id);
}
