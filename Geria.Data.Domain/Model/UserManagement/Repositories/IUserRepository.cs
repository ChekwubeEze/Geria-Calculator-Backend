using Geria.Data.Domain.Infrastruture;
using Geria.Data.Domain.Model.UserManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geria.Data.Domain.Model.UserManagement.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
    }
}
