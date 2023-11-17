using Geria.Data.Domain.Infrastruture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geria.Data.Domain.Model.UserManagement.Entities
{
    public class User :BaseEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
    }
}
