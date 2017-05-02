using DAL.Interface;
using Microsoft.EntityFrameworkCore;
using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(MainContext context) : base(context)
        {
        }

        public Question GetByID(int id)
        {
            return context.Questions.Include(x => x.Answers).FirstOrDefault(x => x.Id == id);
        }
    }
}
