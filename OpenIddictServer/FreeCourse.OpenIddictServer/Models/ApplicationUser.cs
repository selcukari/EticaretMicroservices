using Microsoft.AspNetCore.Identity;

namespace FreeCourse.OpenIddictServer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string City { get; set; }
    }
}
