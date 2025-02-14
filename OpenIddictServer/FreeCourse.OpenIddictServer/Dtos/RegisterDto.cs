using System.ComponentModel.DataAnnotations;

namespace FreeCourse.OpenIddictServer.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
