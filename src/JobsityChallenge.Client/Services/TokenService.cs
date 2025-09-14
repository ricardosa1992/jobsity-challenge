using Microsoft.JSInterop;

namespace JobsityChallenge.Client.Services
{
    public class TokenService
    {
        private readonly IJSRuntime _js;
        private const string TokenKey = "authToken";

        public TokenService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SaveToken(string token) =>
            await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token);

        public async Task<string?> GetToken() =>
            await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);

        public async Task RemoveToken() =>
            await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
    }
}
