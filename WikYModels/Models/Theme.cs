namespace WikYModels.Models
{
    public class Theme
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public IEnumerable<Article>? Articles { get; set; }
    }
}
