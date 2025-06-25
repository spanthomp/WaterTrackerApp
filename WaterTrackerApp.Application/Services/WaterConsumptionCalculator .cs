using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterTrackerApp.Application.Interfaces;

namespace WaterTrackerApp.Application.Services
{
    public class WaterConsumptionCalculator
    {
        private readonly IWaterIntakeService _waterIntakeService;
        public WaterConsumptionCalculator(IWaterIntakeService waterIntakeService)
        {
            _waterIntakeService = waterIntakeService;
        }

        public async Task<int> GetTotalConsumed(int userId)
        {
            var records = await _waterIntakeService.GetByUserIdAsync(userId);
            return records?.Sum(r => r.ConsumedWater) ?? 0;
        }
    }
}
