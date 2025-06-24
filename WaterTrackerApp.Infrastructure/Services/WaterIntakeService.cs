using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterTrackerApp.Application.Dtos;
using WaterTrackerApp.Application.Interfaces;
using WaterTrackerApp.Domain.Entities;
using WaterTrackerApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace WaterTrackerApp.Infrastructure.Services
{
    public class WaterIntakeService : IWaterIntakeService
    {
        private readonly AppDbContext _context;

        public WaterIntakeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WaterIntakeDto>> GetByUserIdAsync(int userId)
        {
            return await _context.WaterIntakes
                .Where(w => w.UserId == userId)
                .Select(w => new WaterIntakeDto
                {
                    Id = w.Id,
                    UserId = w.UserId,
                    IntakeDate = w.IntakeDate,
                    ConsumedWater = w.ConsumedWater
                })
                .ToListAsync();
        }

        public async Task<WaterIntakeDto?> GetByIdAsync(int id)
        {
            var w = await _context.WaterIntakes.FindAsync(id);
            if (w == null) return null;
            return new WaterIntakeDto
            {
                Id = w.Id,
                UserId = w.UserId,
                IntakeDate = w.IntakeDate,
                ConsumedWater = w.ConsumedWater
            };
        }

        public async Task<WaterIntakeDto> CreateAsync(WaterIntakeDto dto)
        {
            var entity = new WaterIntake
            {
                UserId = dto.UserId,
                IntakeDate = dto.IntakeDate,
                ConsumedWater = dto.ConsumedWater
            };
            _context.WaterIntakes.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            return dto;
        }

        public async Task<bool> UpdateAsync(int id, WaterIntakeDto dto)
        {
            var entity = await _context.WaterIntakes.FindAsync(id);
            if (entity == null) return false;

            entity.IntakeDate = dto.IntakeDate;
            entity.ConsumedWater = dto.ConsumedWater;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.WaterIntakes.FindAsync(id);
            if (entity == null) return false;

            _context.WaterIntakes.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
