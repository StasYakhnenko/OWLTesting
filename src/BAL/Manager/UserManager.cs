using BAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model.DTO;
using DAL.Interface;
using Microsoft.AspNetCore.Identity;
using Model.DB;
using AutoMapper;

namespace BAL.Manager
{
    public class UserManager : BaseManager, IUserManager
    {
        private readonly UserManager<ApplicationUser> userManager;
        
        public UserManager(IUnitOfWork uOw, UserManager<ApplicationUser> userManager) : base(uOw)
        {
            this.userManager = userManager;
        }

        public List<UserDTO> GetAllTeachers()
        {
            return uOw.UserRepo
                .All
                .ToList()
                .Where(x => userManager.GetRolesAsync(x)?.Result?.Single() == "Teacher")
                ?.Select(x => Mapper.Map<UserDTO>(x))
                ?.ToList();
        }

        public UserDTO GetById(string id)
        {
            return uOw.UserRepo
                .All
                .Where(x => x.Id == id)
                .Select(x => Mapper.Map<UserDTO>(x))
                .FirstOrDefault();
        }
    }
}
