using WikYRepositories.DTOs.Author;

namespace WikYRepositories.DTOs.Comment
{
    public class GetCommentDTO
    {
        public int Id { get; set; }
        public required GetAuthorDTO Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required string Content { get; set; }
    }
}
