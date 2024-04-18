namespace WikYRepositories.DTOs.Comment
{
    public class AddCommentDTO
    {
        public required int articleId { get; set; }
        public required string content { get; set; }
    }
}
