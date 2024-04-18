namespace WikYRepositories.DTOs.Article
{
    public class GetArticleCommentDTO
    {
        public required int id { get; set; }
        public required string Content { get; set; }
        public required GetArticleAuthorDTO ArticleAuthor { get; set; }
    }
}
