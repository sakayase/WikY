using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WikYModels.Interface;
namespace WikYModels.Models
{
    public class Comment : ITimeStampedModel
    {
        public int Id { get; set; }
        [ForeignKey("AuthorId")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public required Author Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [StringLength(100, ErrorMessage = "The comment must not exceed 100 characters.")]
        public required string Content { get; set; }
        [ForeignKey("ArticleId")]
        public required Article Article { get; set; }
    }
}
