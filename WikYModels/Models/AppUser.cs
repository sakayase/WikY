using Microsoft.AspNetCore.Identity;

namespace WikYModels.Models
{
    public class AppUser : IdentityUser
    {
        public Author Author { get; set; }
    }
}
