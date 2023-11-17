using Geria.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geria.Core.Auth.Token
{
    public interface IJwtHandler
    {
        JsonWebToken Create(string username);
        JwtValidationResult Validate(string Token);
    }
}
