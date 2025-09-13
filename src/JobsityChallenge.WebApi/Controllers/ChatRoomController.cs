using JobsityChallenge.Core.DTOs;
using JobsityChallenge.Core.Interfaces.Services;
using JobsityChallenge.Core.Parameters;
using JobsityChallenge.Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobsityChallenge.WebApi.Controllers;

[Authorize]
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

    [HttpPost("{id}/messages")]
    public async Task<IActionResult> SendMessage([FromRoute] int id, SendMessageDto request)
    {
        request.RoomId = id;
        request.UserId = User.GetUserId();

        var result = await chatService.SendMessageAsync(request);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok();
    }

    [HttpGet("{id}/messages")]
    public async Task<IActionResult> GetMessages([FromRoute] int id)
    {
        var messages = await chatService.GetMessagesAsync(id);

        return Ok(messages);
    }
}
