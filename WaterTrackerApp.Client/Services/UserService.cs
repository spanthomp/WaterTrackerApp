using WaterTrackerApp.Client.Dtos;
using System.Net.Http.Json;

namespace WaterTrackerApp.Client.Services
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

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<UserDto>($"api/user/{id}");
        }

        public async Task<UserDto?> CreateUserAsync(UserDto user)
        {
            var response = await _http.PostAsJsonAsync("api/user", user);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<UserDto>();
            return null;
        }

        public async Task<bool> UpdateUserAsync(int id, UserDto user)
        {
            var response = await _http.PutAsJsonAsync($"api/user/{id}", user);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/user/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}