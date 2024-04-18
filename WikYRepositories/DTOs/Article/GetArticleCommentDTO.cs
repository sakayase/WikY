using WikYRepositories.DTOs.Author;

namespace WikYRepositories.DTOs.Article
{
    public class GetArticleCommentDTO
    {
        public required int id { get; set; }
        public required string Content { get; set; }
        public required GetAuthorDTO ArticleAuthor { get; set; }
    }
}
