using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAL.Interface
{
    public interface IUserManager
    {
        List<UserDTO> GetAllTeachers();
        UserDTO GetById(string id);
    }
}
