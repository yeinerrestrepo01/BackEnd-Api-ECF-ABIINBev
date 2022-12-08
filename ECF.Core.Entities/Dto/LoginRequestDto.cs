using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECF.Core.Entities.Dto
{
    public class LoginRequestDto
    {
        public string Usuario { get; set; }
        public string Password { get; set; }
    }
}
