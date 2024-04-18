using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikYRepositories.DTOs.Comment
{
    public class AddCommentDTO
    {
        public required int articleId { get; set; }
        public required string content { get; set; }
    }
}
