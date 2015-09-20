using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using AutoMapper;
using MotorDepot.BLL.Abstract;
using MotorDepot.BLL.DTO;
using MotorDepot.BLL.Models;
using MotorDepot.DAL.Abstract;
using MotorDepot.DAL.Entities;
using EntryState = MotorDepot.DAL.Entities.EntryState;

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

            if (user.Driver != null && user.Driver.DriverLicense != null)
            {
                user.Driver.DriverLicense.VehicleClasses = new Collection<VehicleClass>();
                userDto.Driver.DriverLicense.VehicleClassIds
                    .Select(x => _db.VehicleClasses.Get(x))
                    .ToList()
                    .ForEach(user.Driver.DriverLicense.VehicleClasses.Add);
            }

            if (user.Role == null || user.RoleId == 0)
            {
                var driverRole = _db.Roles.Find(x => x.Name == "Driver").FirstOrDefault();
                if (driverRole != null)
                    user.RoleId = driverRole.Id;
            }

            

            try
            {
                _db.Users.Create(user);
                _db.Save();
            }
            catch (DbEntityValidationException ex)
            {
                result.Append(ex);
            }

            return result;
        }

        public int? ValidateUser(string login, string password)
        {
            var user = _db.Users.Find(_ => _.EntryState == EntryState.Active && _.IsConfirmed && _.Nickname == login).FirstOrDefault();
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

        public IEnumerable<UserDTO> GetUsers()
        {
            return Mapper.Map<List<UserDTO>>(_db.Users.Find(x => x.EntryState == EntryState.Active));
        }



        public ServiceResult ChangeRoleForUser(Int32 userId, Int32 roleId)
        {
            var result = new ServiceResult();

            var user = _db.Users.Get(userId);
            var role = _db.Roles.Get(roleId);

            if (user == null)
                result.Errors.Add(new PropertyMessagePair { PropertyName = "userId", Message = "Such user doesn't exist in db" });
            if (role == null)
                result.Errors.Add(new PropertyMessagePair { PropertyName = "roleId", Message = "Such role doesn't exist in db" });

            if (user == null || role == null) return result;

            user.Role = null;
            user.RoleId = role.Id;
            

            try
            {
                _db.Users.Update(user);
                _db.Save();
            }
            catch (DbEntityValidationException ex)
            {
                result.Append(ex);
            }

            return result;
        }

        public ServiceResult ConfirmUser(int userId)
        {
            var result = new ServiceResult();

            var user = _db.Users.Get(userId);
            if (user == null)
            {
                result.Errors.Add(new PropertyMessagePair {PropertyName = "userId", Message = "There is no such user in db"});
                return result;
            }

            try
            {
                user.IsConfirmed = true;
                _db.Users.Update(user);
                _db.Save();
            }
            catch (DbEntityValidationException ex)
            {
                result.Append(ex);
            }

            return result;

        }

        public ServiceResult DeleteUser(int userId)
        {
            var result = new ServiceResult();

            var user = _db.Users.Get(userId);
            if (user == null)
            {
                result.Errors.Add(new PropertyMessagePair { PropertyName = "userId", Message = "There is no such user in db" });
                return result;
            }

            try
            {
                user.EntryState = EntryState.Removed;
                _db.Users.Update(user);
                _db.Save();
            }
            catch (DbEntityValidationException ex)
            {
                result.Append(ex);
            }

            return result;
        }

        public bool IsUsernameFree(string username)
        {
            return (!_db.Users.Find(x => x.Nickname == username).Any());
        }

        public IEnumerable<RoleDTO> GetRoles()
        {
            return Mapper.Map<List<RoleDTO>>(_db.Roles.Find(x => x.Name != "Admin"));
        }
    }
}
