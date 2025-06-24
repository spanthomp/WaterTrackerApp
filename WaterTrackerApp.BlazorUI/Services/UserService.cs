using Clean.BlazorUI.Models;
using System.Net.Http.Json;

namespace Clean.BlazorUI.Services
{
    public class UserService
    {
        private readonly HttpClient _http;

        public UserService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<UserDto>?> GetAllUsersAsync()
        {
            return await _http.GetFromJsonAsync<List<UserDto>>("api/user");
        }
    }
}