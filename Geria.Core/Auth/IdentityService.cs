using Geria.Core.Infrastructure.Services.UserManagement;
using Geria.Data.Domain.Model.UserManagement.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geria.Core.Auth
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _context;
        private readonly IUserManagmentServices _userService;

        public User CurrentUser
        {
            get
            {
                if (_cachUser != null)
                    return _cachUser;
                var userName = GetUserIdentity();
                if (!string.IsNullOrEmpty(userName))
                {
                    var user = _userService.GetUserByEmail(userName);
                    _cachUser = user;
                }
                return _cachUser;
            }
            set => _cachUser = value;
        }

        public string GetUserIdentity()
        {
            return _context.HttpContext.User.Identity.Name;
        }

        private User _cachUser;


        public IdentityService(IHttpContextAccessor context, IUserManagmentServices userService)
        {
            _context = context;
            _userService = userService;
        }
    }
}
