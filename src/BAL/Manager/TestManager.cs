using BAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model.DTO;
using DAL.Interface;
using AutoMapper;
using Model.DB;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Collections;
using Microsoft.AspNetCore.Identity;

namespace BAL.Manager
{
    public class TestManager : BaseManager, ITestManager
    {
		private UserManager<ApplicationUser> userIdenityManager;
		private readonly ILogger logger;
		public TestManager(IUnitOfWork uOw, ILogger<TestManager> logger,UserManager<ApplicationUser> userIdenityManager) : base(uOw)
        {
			this.logger = logger;
			this.userIdenityManager = userIdenityManager;
        }

        public void AddQuestionToTest(QuestionDTO question, int id)
        {
            var entity = uOw.TestRepo.GetByID(id);
            if (entity == null)
            {
                return;
            }
			if (question.Answers == null)
			{
				throw new Exception("У питання немає жодної відповіді!");
			}
			var answersToDelete = new List<AnswerDTO>();
			foreach (var answer in question.Answers.Where(x => x.IsDeleted).ToList())
			{
				answersToDelete.Add(answer);
			}
			foreach (var answerToDelete in answersToDelete)
			{
				question.Answers.Remove(answerToDelete);
			}
            entity.Questions.Add(Mapper.Map<Question>(question));
            uOw.TestRepo.Update(entity);
            uOw.Save();
        }

        public int AddTest(TestDTO test)
        {
            var entity = Mapper.Map<Test>(test);
			try
			{
				uOw.TestRepo.Insert(entity);
				uOw.Save();
			}
			catch(Exception ex)
			{
				logger.LogError(ex.Message);
			}
            entity.TeacherTests?.Clear();
            entity.TeacherTests = new List<TeacherTest>();
            List<string> teachersIds = JsonConvert.DeserializeObject<List<string>>(test.TeachersJSON);
            foreach (var teacherId in teachersIds)
            {
                entity.TeacherTests.Add(new TeacherTest()
                {
                    TestId = entity.Id,
                    UserId = teacherId
                });
            }
            return entity.Id;
        }

		public bool CheckUserAccess(int id, string userId)
		{
			var test = uOw.TestRepo.GetByID(id);
			bool isHasAccess = test.TeacherTests.Any(x => x.UserId == userId);
			return isHasAccess;
		}

		public List<TestDTO> GetAll()
        {
            List<TestDTO> tests = new List<TestDTO>();
            foreach (var test in uOw.TestRepo.GetAll())
            {
                var model = Mapper.Map<TestDTO>(test);
                model.Subject = Mapper.Map<SubjectDTO>(test.Subject);;
                tests.Add(model);
            }
            return tests;
        }

        public TestDTO GetById(int id)
        {
            var test = uOw.TestRepo.GetByID(id);
            return Mapper.Map<TestDTO>(test);
        }

		public List<TestDTO> GetTestsForTeacher(string userId)
		{
			var tests = uOw.TestRepo.GetAll().Where(x => x.TeacherTests?.Any(y => y.UserId == userId) ?? false).ToList();
			var targetTests = new List<TestDTO>();
			foreach (var test in tests)
			{
				targetTests.Add(Mapper.Map<TestDTO>(test));
			}
			return targetTests;
		}

		public List<TestDTO> GetTestsToPass()
		{
			var tests = uOw.TestRepo.GetAll().Where(x => !x.EndDate.HasValue || x.EndDate > DateTime.Now).ToList();
			var targetTests = new List<TestDTO>();
			foreach (var test in tests)
			{
				targetTests.Add(Mapper.Map<TestDTO>(test));
			}
			return targetTests;
		}

		public void UpdateTest(TestDTO test)
        {
            if (test==null)
            {
                return;
            }
            var entity = uOw.TestRepo.GetByID(test.Id);
            if (entity==null)
            {
                return;
            }
            entity.Name = test.Name;
            entity.Grade = test.Grade;
            entity.Requirment = test.Requirment;
            entity.ConsiderPartialAnswers = test.ConsiderPartialAnswers;
            entity.SubjectId = test.SubjectId;
            entity.StartDate = test.StartDate;
            entity.EndDate = test.EndDate;
            entity.TimeLimit = entity.TimeLimit;
            entity.TeacherTests?.Clear();
            uOw.TestRepo.Update(entity);
            uOw.Save();
            entity.TeacherTests = new List<TeacherTest>();
            List<string> teachersIds = JsonConvert.DeserializeObject<List<string>>(test.TeachersJSON);
            foreach (var teacherId in teachersIds)
            {
                entity.TeacherTests.Add(new TeacherTest()
                {
                    TestId = entity.Id,
                    UserId = teacherId
                });
            }
            uOw.TestRepo.Update(entity);
            uOw.Save();
        }
    }
}
