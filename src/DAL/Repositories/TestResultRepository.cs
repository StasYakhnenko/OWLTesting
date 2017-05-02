using DAL.Interface;
using Microsoft.EntityFrameworkCore;
using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
	public class TestResultRepository : GenericRepository<TestResult>, ITestResultRepository
	{
		public TestResultRepository(MainContext context) : base(context)
		{
		}

		public IEnumerable<TestResult> GetAll()
		{
			return context.TestResults
				.Include(x => x.Test)
				.Include(x => x.Questions)
				.Include(x => x.Questions)
				.Include(x => x.User)
				.ToList();
		}

		public TestResult GetByID(int id)
		{
			return context.TestResults
				.Include(x => x.User)
				.Include(x => x.Test)
				.Include(x => x.Questions)
				.ThenInclude(y => y.Answers)
				.ThenInclude(y => y.Question)
				.ThenInclude(y => y.Answers)
				.FirstOrDefault(x => x.Id == id);
		}

		public IEnumerable<TestResult> GetBySubjectId(int subjectId)
		{
			return context.TestResults
				.Include(x => x.Test)
				.Include(x => x.Questions)
				.Include(x => x.Questions.Select(y => y.Answers))
				.Include(x => x.User)
				.Where(x => x.Test.Subject.Id == subjectId)
				.ToList();
		}

		public IEnumerable<TestResult> GetByUserId(string userId)
		{
			return context.TestResults
				.Include(x => x.User)
				.Include(x => x.Test)
				.Include(x => x.Questions)
				.ThenInclude(y => y.Answers)
				.ThenInclude(y => y.Question)
				.ThenInclude(y => y.Answers)
				.Where(x => x.UserId == userId)
				.ToList();
		}

		public void SetAnswerToQuestion(int testId, int questionId, List<int> answerIds)
		{
			var entity = GetByID(testId);
			var question = entity.Questions.Where(x => x.Id == questionId).FirstOrDefault();
			question.Answers?.Clear();
			question.Answers = new List<GivenAnswer>();
			foreach (var answerId in answerIds)
			{
				question.Answers.Add(new GivenAnswer()
				{
					AnswerId = answerId,
					QuestionId = question.Id
				});
			}
		}

		public void SetQuestionsToTestResult(int testId)
		{
			var entity = GetByID(testId);
			var questions = context.Questions.Where(x => x.TestId == entity.TestId);

			foreach (var item in questions)
			{
				entity.Questions.Add(new GivenQuestion()
				{
					QuestionId = item.Id,
					TestResultId = testId,
					Answers = null
				});
			}
		}
	}
}
