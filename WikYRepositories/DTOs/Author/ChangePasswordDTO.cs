using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikYRepositories.DTOs.Author
{
    public class ChangePasswordDTO
    {
        public string OldPwd { get; internal set; }
        public string NewPwd { get; internal set;}
    }
}
