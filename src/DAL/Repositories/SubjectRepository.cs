using DAL.Interface;
using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
	public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
	{
		public SubjectRepository(MainContext context) : base(context)
		{
		}
	}
}
