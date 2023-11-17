using Geria.Core.Models;
using Geria.Data.Domain.Model.UserManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geria.Core.Infrastructure.Services.UserManagement
{
    public interface IUserManagmentServices
    {
        Task CreateUser(User user);
        Task UpdateUser(User user);
        User GetUserByEmail(string email);
        Task<UserRegistrationResponse> SignUp(UserRegistrationRequest registrationRequest);
        Task<UserLoginResult> Login(UserLogin userLoginRequest);
    }
}
