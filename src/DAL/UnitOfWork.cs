using DAL.Interface;
using DAL.Repositories;
using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private MainContext context;
        #region Private Repositories

        private ISubjectRepository subjectRepo;
        private IQuestionRepository questionRepo;
        private IAnswerRepository answerRepo;
        private IUserRepository userRepo;
        private ITestRepository testRepo;
        private ITestResultRepository testResultRepo;
        #endregion


        public UnitOfWork
			(
				MainContext context,
				ITestRepository testRepo,
				IQuestionRepository questionRepo,
				ITestResultRepository testResultRepo,
				ISubjectRepository subjectRepo,
				IAnswerRepository answerRepo,
				IUserRepository userRepo
			)
        {
            this.context = context;
			this.subjectRepo = subjectRepo;
            this.questionRepo = questionRepo;
            this.answerRepo = answerRepo;
            this.userRepo = userRepo;
            this.testRepo = testRepo;
            this.testResultRepo = testResultRepo;
        }


        #region Getters
        public ISubjectRepository SubjectRepo
        {
            get
            {
                return subjectRepo;
            }
        }
        public ITestRepository TestRepo
        {
            get
            {
                return testRepo;
            }
        }
        public IQuestionRepository QuestionRepo
        {
            get
            {
                return questionRepo;
            }
        }
        public IAnswerRepository AnswerRepo
        {
            get
            {
                return answerRepo;
            }
        }

        public IUserRepository UserRepo
        {
            get
            {
                return userRepo;
            }
        }

        public ITestResultRepository TestResultRepo
        {
            get
            {
                return testResultRepo;
            }
        }
        #endregion
        public int Save()
        {
            try
            {
                return context.SaveChanges();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        #region Dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
