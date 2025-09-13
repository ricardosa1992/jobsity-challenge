using JobsityChallenge.Core.Interfaces.Services;
using JobsityChallenge.Core.Parameters;
using Microsoft.AspNetCore.Mvc;

namespace JobsityChallenge.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatRoomController(IChatService chatService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetChatRooms([FromQuery] GetChatRoomsParameters parameters)
    {
        var rooms = await chatService.GetChatRoomsAsync(parameters);

        return Ok(rooms);
    }

    [HttpPost]
    public async Task<IActionResult> CreateChatRoom([FromBody] string name)
    {
        var room = await chatService.CreateChatRoomAsync(name);

        return CreatedAtAction(nameof(GetChatRooms), new { id = room.Id }, room);
    }
}
