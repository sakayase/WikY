using System.ComponentModel.DataAnnotations;

namespace WikYRepositories.DTOs.Author
{
    public class LoginAuthorDTO
    {
        [StringLength(20)]
        public required string UserName { get; set; }
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
