using DAL.Interface;
using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
	public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
	{
		public UserRepository(MainContext context) : base(context)
		{
		}
	}
}
