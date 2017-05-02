using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model.DB;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class TestRepository : GenericRepository<Test>, ITestRepository
    {
        public TestRepository(MainContext context) : base(context)
        {
        }

        public Test GetByID(int id)
        {
            return context.Tests
                .Include(x => x.Questions)
                .Include(x => x.TeacherTests)
                .Include(x => x.Subject)
				.Include(x => x.TestHistory)
				.ThenInclude(x => x.Questions)
				.ThenInclude(x => x.Answers)
                .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Test> GetAll()
        {
            return context.Tests
				.Include(x => x.Questions)
				.Include(x => x.TeacherTests)
				.Include(x => x.TestHistory)
				.ToList();
        }
    }
}
