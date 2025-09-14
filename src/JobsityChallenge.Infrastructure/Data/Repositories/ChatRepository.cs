using JobsityChallenge.Core.DTOs;
using JobsityChallenge.Core.Entities;
using JobsityChallenge.Core.Interfaces.Repositories;
using JobsityChallenge.Core.Parameters;
using JobsityChallenge.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace JobsityChallenge.Infrastructure.Data.Repositories;

public class ChatRepository(ApplicationDbContext context) : IChatRepository
{
    public Task<ChatRoom?> GetChatRoomByIdAsync(int id)
    {
        return context.ChatRooms.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<PagedResult<ChatRoom>> GetChatRoomsAsync(GetChatRoomsParameters parameters)
    {
        var query = context.ChatRooms.AsQueryable();
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(cr => cr.Id)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return new PagedResult<ChatRoom>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    public async Task<ChatRoom> CreateChatRoomAsync(ChatRoom chatRoom)
    {
        context.ChatRooms.Add(chatRoom);
        await context.SaveChangesAsync();
        return chatRoom;
    }

    public async Task<IEnumerable<GetMessageDto>> GetLastMessagesAsync(int chatRoomId, int count = 50)
    {
        var messages = await context.ChatMessages
            .Where(p => p.ChatRoomId == chatRoomId)
            .OrderByDescending(cr => cr.Timestamp)
            .Take(count)
            .Select(p => new GetMessageDto(p.Id, p.UserId, p.User.UserName!, p.Content, p.Timestamp))
            .ToListAsync();

        return messages.OrderBy(cr => cr.Timestamp);
    }

    public Task SaveChangesAsync() => context.SaveChangesAsync();
}
