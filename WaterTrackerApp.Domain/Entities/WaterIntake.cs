using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterTrackerApp.Domain.Entities
{
    public class WaterIntake
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime IntakeDate { get; set; }
        public int ConsumedWater { get; set; } // in ml
        
        public User User { get; set; } = null!;
    }
}
