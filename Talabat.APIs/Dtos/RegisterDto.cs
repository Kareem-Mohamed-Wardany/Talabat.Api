using System.ComponentModel.DataAnnotations;

namespace Talabat.Api.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Phone { get; set; } = null!;

        [Required]
        //[RegularExpression]
        public string Password { get; set; } = null!;
    }
}
