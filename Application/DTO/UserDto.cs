using System.ComponentModel.DataAnnotations;

namespace Restuarant.Application.DTO
{
    public class UserDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string PasswordConfirm { get; set; } = string.Empty;


    }
}
