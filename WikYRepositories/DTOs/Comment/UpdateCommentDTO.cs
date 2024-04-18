using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikYRepositories.DTOs.Comment
{
    public class UpdateCommentDTO
    {
        public int Id { get; set; }
        public required string Content { get; set; }
    }
}
