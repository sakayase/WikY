namespace WikYRepositories.DTOs.Article
{
    public class UpdateArticleDTO
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public int? ThemeId { get; set; }
    }
}
