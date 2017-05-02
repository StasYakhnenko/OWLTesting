using BAL.Interface;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model.DB;
using AutoMapper;
using Model.DTO;
using Microsoft.Extensions.Logging;

namespace BAL.Manager
{
    public class SubjectManager : BaseManager, ISubjectManager
    {
		private readonly ILogger logger;
		public SubjectManager(IUnitOfWork uOw, ILogger<SubjectManager> logger) : base(uOw)
        {
			this.logger = logger;
        }

        public List<SubjectDTO> GetAll()
        {
            List<SubjectDTO> subjects = new List<SubjectDTO>();

            foreach (var subject in uOw.SubjectRepo.All.ToList())
            {
                try
                {
                    subjects.Add(Mapper.Map<SubjectDTO>(subject));
                }
                catch(Exception ex)
                {
					logger.LogError(ex.Message);
				}
                
            }

			foreach (var subject in subjects)
			{
				List<TestDTO> testsToHide = new List<TestDTO>();
				if (subject.Tests != null)
				{
					foreach (var test in subject.Tests)
					{
						if (test.EndDate.HasValue && test.EndDate < DateTime.Now)
						{
							testsToHide.Add(test);
						}
					}
					foreach (var testToHide in testsToHide)
					{
						subject.Tests.Remove(testToHide);
					}
				}
			}

            return subjects;
        }

        public int AddSubject(SubjectDTO subject)
        {
            if (subject == null) return -1;
            var dbsubject = Mapper.Map<Subject>(subject);
            uOw.SubjectRepo.Insert(dbsubject);
            uOw.Save();
            return dbsubject.Id;
        }

        public bool DeleteSubject(int id)
        {
            try
            {
                var subject = uOw.SubjectRepo.GetByID(id);
                if (subject == null) return false;

                uOw.SubjectRepo.Delete(subject);
                uOw.Save();
                return true;
            }
            catch(Exception ex)
            {
				logger.LogError(ex.Message);
			}

            return false;
        }

        public bool UpdateSubject(SubjectDTO subject)
        {
            try
            {
                var dbsubject = uOw.SubjectRepo.GetByID(subject.Id);
                if (dbsubject == null) return false;
                dbsubject.Name = subject.Name;
                dbsubject.Describtion = subject.Describtion;
                uOw.Save();
                return true;
            }
            catch(Exception ex)
            {
				logger.LogError(ex.Message);
			}

            return false;
        }

        public SubjectDTO GetById(int id)
        {
            try
            {
                var subject = Mapper.Map<SubjectDTO>(uOw.SubjectRepo.GetByID(id));
                return subject;
            }
            catch(Exception ex)
            {
				logger.LogError(ex.Message);
				return null;
            }
        }
    }
}
