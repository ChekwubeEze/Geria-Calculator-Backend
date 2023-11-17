using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geria.Core.Models
{
    public class UserLoginResult
    {
        public int Id { get; set; }
        public string UserName { get; set;}
        public string Email { get; set;}
        public UserLoginResults LoginResult { get; set; }
        public JsonWebToken JsonWebToken { get; set; }
    }
}
