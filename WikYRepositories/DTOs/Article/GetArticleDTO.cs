using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikYModels.Models;

namespace WikYRepositories.DTOs.Article
{
    public class GetArticleDTO
    {
        public int Id { get; set; }
        public required GetArticleAuthorDTO Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Priority Priority { get; set; }
        public required string Content { get; set; }
        public GetArticleThemeDTO? Theme { get; set; }
        public IEnumerable<GetArticleCommentDTO> Comments { get; set; } = Enumerable.Empty<GetArticleCommentDTO>();
    }
}
