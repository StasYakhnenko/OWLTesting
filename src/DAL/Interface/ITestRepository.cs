using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface ITestRepository : IGenericRepository<Test>
    {
        Test GetByID(int id);
        IEnumerable<Test> GetAll();
    }
}
