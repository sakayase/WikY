using WikYModels.Models;

namespace WikYRepositories.DTOs.Article
{
    public class AddArticleDTO
    {
        public required string Content { get; set; }
        public Priority Priority { get; set; } = Priority.Normal;
        public int ThemeId { get; set; }
    }
}
