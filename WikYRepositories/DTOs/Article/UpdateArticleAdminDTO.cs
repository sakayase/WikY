using WikYModels.Models;

namespace WikYRepositories.DTOs.Article
{
    public class UpdateArticleAdminDTO
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public int? ThemeId { get; set; }
        public Priority? Priority { get; set; }
    }
}
