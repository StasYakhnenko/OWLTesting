using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        Question GetByID(int id);
    }
}
