using System.ComponentModel.DataAnnotations;

    namespace WaterTrackerApp.Client.Dtos
{
    public class WaterIntakeDto
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime IntakeDate { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Consumed water must be a positive amount.")]
        public int ConsumedWater { get; set; }
    }
}
