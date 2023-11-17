using Geria.Data.Domain.Model.UserManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geria.Core.Auth
{
    public interface IIdentityService
    {
        string GetUserIdentity();
        User CurrentUser { get; set; }
    }
}
