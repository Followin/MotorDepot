using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using AutoMapper;
using MotorDepot.BLL.Abstract;
using MotorDepot.BLL.DTO;
using MotorDepot.BLL.Models;
using MotorDepot.DAL.Abstract;
using MotorDepot.DAL.Entities;

namespace MotorDepot.BLL.Services
{
    public class AuthService : IAuthService
    {
        private IMotorDepotUnitOfWork _db;

        public AuthService(IMotorDepotUnitOfWork db)
        {
            _db = db;
        }
        public ServiceResult RegisterUser(UserDTO userDto)
        {
            var result = new ServiceResult();

            var userExists = _db.Users.Find(x => x.Nickname == userDto.Nickname).Any();
            if (userExists)
            {
                result.Errors.Add(new PropertyMessagePair { PropertyName = "Nickname", Message = "User with such nickname already exists" });
                return result;
            }

            userDto.Password = Crypto.HashPassword(userDto.Password);

            var user = Mapper.Map<User>(userDto);

            if (user.Role == null)
            {
                var driverRole = _db.Roles.Find(x => x.Name == "Driver").FirstOrDefault();
                if (driverRole != null)
                    user.Role = driverRole;
            }

            try
            {

                _db.Users.Create(user);
                _db.Save();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationError in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationError.ValidationErrors)
                    {
                        result.Errors.Add(new PropertyMessagePair {PropertyName = validationError.PropertyName, Message = validationError.ErrorMessage });
                    }
                }
            }

            return result;
        }

        public int? ValidateUser(string login, string password)
        {
            var user = _db.Users.Find(_ => _.Nickname == login).FirstOrDefault();
            if (user != null)
            {
                var isValid = Crypto.VerifyHashedPassword(user.Password, password);
                if (isValid) return user.Id;
            }
            return null;
        }


        public UserDTO GetUserInfo(int id)
        {
            var user = _db.Users.Get(id);
            if (user != null)
            {
                return Mapper.Map<UserDTO>(user);
            }
            return null;
        }


        public ServiceResult ChangeRoleForUser(UserDTO userDto, RoleDTO roleDto)
        {
            var result = new ServiceResult();

            var user = _db.Users.Get(userDto.Id);
            var role = _db.Roles.Get(roleDto.Id);

            if (user == null)
                result.Errors.Add(new PropertyMessagePair { PropertyName = "userDto", Message = "Such user doesn't exist in db" });
            if (role == null)
                result.Errors.Add(new PropertyMessagePair { PropertyName = "roleDto", Message = "Such role doesn't exist in db" });

            if (user == null || role == null) return result;

            user.Role = role;

            try
            {
                _db.Users.Update(user);
                _db.Save();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationError in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationError.ValidationErrors)
                    {
                        result.Errors.Add(new PropertyMessagePair { PropertyName = validationError.PropertyName, Message = validationError.ErrorMessage });
                    }
                }
            }

            return result;
        }
    }
}
