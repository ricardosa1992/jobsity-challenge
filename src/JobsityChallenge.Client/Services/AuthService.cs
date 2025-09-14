using JobsityChallenge.Client.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace JobsityChallenge.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly TokenService _tokenService;

        public AuthService(HttpClient http, TokenService tokenService)
        {
            _http = http;
            _tokenService = tokenService;
        }

        public async Task Register(User user)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", user);
            response.EnsureSuccessStatusCode();
        }

        public async Task Login(LoginRequest loginRequest)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", loginRequest);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(content);

            await _tokenService.SaveToken(loginResponse.Token);
        }

        public async Task Logout()
        {
            await _tokenService.RemoveToken();
        }

        public async Task<string?> GetToken() => await _tokenService.GetToken();
    }
}
