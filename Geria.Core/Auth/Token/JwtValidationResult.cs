using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geria.Core.Auth.Token
{
    public class JwtValidationResult
    {
        public JwtValidationResult()
        {
            Errors = new List<string>();
        }
        public bool Success
        {
            get { return !Errors.Any(); }
        }
        public void AddErrors(string errors)
        {
            Errors.Add(errors);
        }
        public IList<string> Errors { get; set; }
        public JwtPayLoad JwtPayLoad { get; set; }
    }
}
