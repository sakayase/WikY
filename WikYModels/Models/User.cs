using Microsoft.AspNetCore.Identity;

namespace WikYModels.Models
{
    public class User : IdentityUser
    {
        public Author Author { get; set; }
    }
}
