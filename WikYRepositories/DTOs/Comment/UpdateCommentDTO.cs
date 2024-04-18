namespace WikYRepositories.DTOs.Comment
{
    public class UpdateCommentDTO
    {
        public int Id { get; set; }
        public required string Content { get; set; }
    }
}
