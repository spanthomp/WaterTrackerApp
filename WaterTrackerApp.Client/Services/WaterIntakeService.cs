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

        // i. Obtain a user's water intake records
        public async Task<List<WaterIntakeDto>?> GetByUserIdAsync(int userId)
        {
            return await _http.GetFromJsonAsync<List<WaterIntakeDto>>($"api/waterintake/user/{userId}");
        }

        // ii. Add new water intake record
        public async Task<WaterIntakeDto?> CreateAsync(WaterIntakeDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/waterintake", dto);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<WaterIntakeDto>();
            return null;
        }

        // iii. View specific water intake record by ID
        public async Task<WaterIntakeDto?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<WaterIntakeDto>($"api/waterintake/{id}");
        }

        // iv. Modify water intake record
        public async Task<bool> UpdateAsync(int id, WaterIntakeDto dto)
        {
            var response = await _http.PutAsJsonAsync($"api/waterintake/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        // v. Delete water intake record
        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/waterintake/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
