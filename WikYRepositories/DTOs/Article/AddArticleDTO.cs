using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikYModels.Models;

namespace WikYRepositories.DTOs.Article
{
    public class AddArticleDTO
    {
        public int? AuthorID { get; set; }
        public required string Content { get; set; }
        public Priority Priority { get; set; } = Priority.Normal;
        public int ThemeId {  get; set; }
    }
}
