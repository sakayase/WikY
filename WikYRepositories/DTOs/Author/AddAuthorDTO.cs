using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikYModels.Models;

namespace WikYRepositories.DTOs.Author
{
    public class AddAuthorDTO
    {
        [StringLength(20, ErrorMessage = "The username must not exceed 20 characters.")]
        public required string UserName { get; set; }
        [DataType(DataType.Password)]
        [Compare("Pwd2", ErrorMessage = "The passwords must match.")]
        public required string Pwd { get; set; }
        public required string Pwd2 { get; set; }
        public required DateOnly BirthDate { get; set; }

    }
}
