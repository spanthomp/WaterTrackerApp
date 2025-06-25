using WaterTrackerApp.Client.Dtos;
using System.Net.Http.Json;

namespace WaterTrackerApp.Client.Services
{
    public class WaterIntakeService
    {
        private readonly HttpClient _http;

        public WaterIntakeService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<WaterIntakeDto>?> GetByUserIdAsync(int userId)
        {
            return await _http.GetFromJsonAsync<List<WaterIntakeDto>>($"api/waterintake/user/{userId}");
        }

        public async Task<WaterIntakeDto?> CreateAsync(WaterIntakeDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/waterintake", dto);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<WaterIntakeDto>();
            return null;
        }

        public async Task<WaterIntakeDto?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<WaterIntakeDto>($"api/waterintake/{id}");
        }

        public async Task<bool> UpdateAsync(int id, WaterIntakeDto dto)
        {
            var response = await _http.PutAsJsonAsync($"api/waterintake/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/waterintake/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<int> GetTotalConsumedAsync(int userId)
        {
            return await _http.GetFromJsonAsync<int>($"api/waterintake/user/{userId}/total");
        }
    }
}
