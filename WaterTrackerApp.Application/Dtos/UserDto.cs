using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterTrackerApp.Application.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;
    }
}
