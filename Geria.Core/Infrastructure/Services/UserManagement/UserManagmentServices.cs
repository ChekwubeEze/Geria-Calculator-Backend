using Geria.Core.Auth.Token;
using Geria.Core.Infrastructure.Services.Calculatormanagement;
using Geria.Core.Models;
using Geria.Data.Domain.Model.UserManagement.Entities;
using Geria.Data.Domain.Model.UserManagement.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geria.Core.Infrastructure.Services.UserManagement
{
    public class UserManagmentServices : IUserManagmentServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserhelper _userhelper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtHandler _jwtHandler;

        public UserManagmentServices(IUserRepository userRepository, IUserhelper userhelper, IPasswordHasher<User> passwordHasher,
            IJwtHandler jwtHandler)
        {
            _userRepository = userRepository;
            _userhelper = userhelper;
            _passwordHasher = passwordHasher;
            _jwtHandler = jwtHandler;
        }
        public async Task CreateUser(User user)
        {
            _userRepository.Add(user);
            await _userRepository.UnitOfWork.SaveChangesAsync();
        }
        public async Task UpdateUser(User user)
        {
            _userRepository.Update(user);
            await _userRepository.UnitOfWork.SaveChangesAsync();
        }

        public User GetUserByEmail(string email)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));
            var user = _userRepository.Get(u => u.Email == email);
            return user;
        }

        public async Task<UserRegistrationResponse> SignUp(UserRegistrationRequest registrationRequest)
        {
            var user = GetUserByEmail(registrationRequest.Email);
            if (user != null)
            {
                throw new Exception("User already exist");
            }
            _userhelper.CreatePasswordHash(registrationRequest.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var newuser = new User()
            {
                PasswordHash = passwordHash,
                Email = registrationRequest.Email,
                PasswordSalt = passwordSalt,
                UserName = registrationRequest.UserName,
            };
             await CreateUser(newuser);

            var response = new UserRegistrationResponse()
            {
                Id = newuser.Id,
                Email = newuser.Email,
                UserName = newuser.UserName

            };
            if (response != null)
            {
                return response;
            }

            return null;
        }

        public async Task<UserLoginResult> Login(UserLogin userLoginRequest)
        {
            var loginResult = new UserLoginResult();

            var user = GetUserByEmail(userLoginRequest.Email);
            if (user == null)
            {
                loginResult.LoginResult = UserLoginResults.USER_DOES_NOT_EXIST;
                return loginResult;
            }
            if (!_userhelper.VerifyPassword(userLoginRequest.Password, user.PasswordHash, user.PasswordSalt))
            {
                loginResult.LoginResult = UserLoginResults.WRONG_PASSWORD;
                return loginResult;
            }
            loginResult.LoginResult = UserLoginResults.SUCCESSFUL;
            var jwt = _jwtHandler.Create(user.Email);
            var refreshToken = _passwordHasher.HashPassword(user, Guid.NewGuid().ToString())
                .Replace("+", string.Empty)
                .Replace("=", string.Empty)
                .Replace("/", string.Empty);
            jwt.RefreshToken = refreshToken;
            user.RefreshToken = refreshToken;
            await UpdateUser(user);
            loginResult.JsonWebToken = jwt;
            loginResult.Email = user.Email;
            loginResult.UserName = user.UserName;
            return loginResult;
        }
    }
}
