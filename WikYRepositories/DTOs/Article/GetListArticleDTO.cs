using WikYModels.Models;
using WikYRepositories.DTOs.Author;
using WikYRepositories.DTOs.Theme;

namespace WikYRepositories.DTOs.Article
{
    public class GetListArticleDTO
    {
        public int Id { get; set; }
        public required GetAuthorDTO Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Priority Priority { get; set; }
        public required string Content { get; set; }
        public GetThemeDTO Theme { get; set; }
    }
}
