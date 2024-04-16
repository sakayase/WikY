using System.ComponentModel.DataAnnotations.Schema;
using WikYModels.Interface;

namespace WikYModels.Models
{
    public class Article : ITimeStampedModel
    {
        public int Id { get; set; }
        [ForeignKey("AuthorId")]
        public required Author Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Priority Priority { get; set; }
        public required string Content { get; set; }
        [ForeignKey("ThemeId")]
        public Theme? Theme { get; set; }
        public IEnumerable<Comment>? Comments { get; set; }
    }

    public enum Priority { Normal, High }
}
