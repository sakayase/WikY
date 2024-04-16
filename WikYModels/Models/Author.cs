using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WikYModels.Models
{
    public class Author
    {
        public int Id { get; set; }
        [StringLength(20, ErrorMessage = "The username must not exceed 20 characters.")]
        public required string UserName { get; set; }
        public IEnumerable<Article>? Articles { get; set; }
        public IEnumerable<Comment>? Comments { get; set; }
        public DateOnly BirthDate { get; set; }
    }
}
