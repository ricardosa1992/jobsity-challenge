namespace JobsityChallenge.Client.Services
{
    using JobsityChallenge.Client.Models;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;

    public class ChatService
    {
        private readonly HttpClient _http;
        private readonly TokenService _tokenService;

        public ChatService(HttpClient http, TokenService tokenService)
        {
            _http = http;
            _tokenService = tokenService;
        }

        private async Task AddBearerToken()
        {
            var token = await _tokenService.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<ChatRoom>> GetChatRooms()
        {
            await AddBearerToken();
            var pagedResult = await _http.GetFromJsonAsync<PagedResult<ChatRoom>>("api/chatroom");
            return pagedResult?.Items ?? new List<ChatRoom>();
        }

        public async Task CreateChatRoom(ChatRoom room)
        {
            await AddBearerToken();
            var response = await _http.PostAsJsonAsync("api/chatroom", room);
            response.EnsureSuccessStatusCode();
        }

        // Optional: for messages
        public async Task<List<Message>> GetMessages(string roomId)
        {
            await AddBearerToken();
            var messages = await _http.GetFromJsonAsync<List<Message>>($"api/chatroom/{roomId}/messages");
            return messages ?? new List<Message>();
        }

        public async Task SendMessage(string roomId, Message message)
        {
            await AddBearerToken();
            var response = await _http.PostAsJsonAsync($"api/chatroom/{roomId}/messages", message);
            response.EnsureSuccessStatusCode();
        }
    }
}
