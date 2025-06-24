using WaterTrackerApp.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterTrackerApp.Application.Interfaces
{
    public interface IWaterIntakeService
    {
        Task<IEnumerable<WaterIntakeDto>> GetByUserIdAsync(int userId);
        Task<WaterIntakeDto?> GetByIdAsync(int id);
        Task<WaterIntakeDto> CreateAsync(WaterIntakeDto waterIntakeDto);
        Task<bool> UpdateAsync(int id, WaterIntakeDto waterIntakeDto);
        Task<bool> DeleteAsync(int id);
    }
}
