using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterTrackerApp.Infrastructure.Data;

namespace WaterTrackerApp.Tests.Utilities
{
    public static class TestDbContext
    {
        public static AppDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unique db foe every test
                .Options;
            return new AppDbContext(options);
        }
    }
}
