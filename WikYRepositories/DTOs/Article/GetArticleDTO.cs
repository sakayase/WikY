using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikYModels.Models;
using WikYRepositories.DTOs.Author;
using WikYRepositories.DTOs.Theme;

namespace WikYRepositories.DTOs.Article
{
    public class GetArticleDTO
    {
        public int Id { get; set; }
        public required GetAuthorDTO Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Priority Priority { get; set; }
        public required string Content { get; set; }
        public GetThemeDTO? Theme { get; set; }
        public IEnumerable<GetArticleCommentDTO> Comments { get; set; } = Enumerable.Empty<GetArticleCommentDTO>();
    }
}
