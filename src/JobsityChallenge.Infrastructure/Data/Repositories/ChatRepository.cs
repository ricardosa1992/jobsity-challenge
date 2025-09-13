using Azure.Core;
using JobsityChallenge.Core.Entities;
using JobsityChallenge.Core.Interfaces.Repositories;
using JobsityChallenge.Core.Parameters;
using JobsityChallenge.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace JobsityChallenge.Infrastructure.Data.Repositories;

public class ChatRepository(ApplicationDbContext context) : IChatRepository
{
    public Task<ChatRoom?> GetChatRoomByNameAsync(string name)
    {
        return context.ChatRooms.FirstOrDefaultAsync(r => r.Name == name);
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
}
