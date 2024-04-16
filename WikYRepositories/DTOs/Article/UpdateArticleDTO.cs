using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikYModels.Models;

namespace WikYRepositories.DTOs.Article
{
    public class UpdateArticleDTO
    {
        public string? Content { get; set; }
        public int? ThemeId { get; set; }
        public Priority? Priority { get; set; }

    }
}
