using DAL.Interface;
using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
	public class AnswerRepository : GenericRepository<Answer>, IAnswerRepository
	{
		public AnswerRepository(MainContext context) : base(context)
		{
		}
	}
}
