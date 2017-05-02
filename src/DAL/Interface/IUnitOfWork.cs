using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IUnitOfWork
    {
		ISubjectRepository SubjectRepo { get; }
        ITestRepository TestRepo { get; }
		IAnswerRepository AnswerRepo { get; }
        IQuestionRepository QuestionRepo { get; }
		IUserRepository UserRepo { get; }
        ITestResultRepository TestResultRepo { get; }
        void Dispose();

        int Save();
    }
}
