using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geria.Core.Auth.Token
{
    public class JwtPayLoad
    {
        public DateTime ValidTo { get; set; }
        public string Id { get; set; }
    }
}
