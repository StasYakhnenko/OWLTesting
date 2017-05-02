using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Interface;
using BAL.Interface;
using Model.DTO;
using Model.DB;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BAL.Manager
{
	public class TestResultManager : BaseManager, ITestResultManager
	{
		private UserManager<ApplicationUser> userIdenityManager;
		private readonly ILogger logger;
		public TestResultManager(IUnitOfWork uOw, UserManager<ApplicationUser> userIdenityManager, ILogger<TestResultManager> logger) : base(uOw)
		{
			this.userIdenityManager = userIdenityManager;
			this.logger = logger;
		}

		public int CreateTestResult(int testId, string userId)
		{
			var entity = new TestResult()
			{
				UserId = userId,
				TestId = testId,
				ResultGrade = 0,
				Status = Common.StatusTest.NotFinished,
				TimeStart = DateTime.Now
			};

			try
			{
				uOw.TestResultRepo.Insert(entity);
				uOw.Save();
				uOw.TestResultRepo.SetQuestionsToTestResult(entity.Id);
				uOw.Save();
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
			}

			return entity.Id;
		}
		private double GetGradeForQuestion(GivenQuestion question, bool toCalculatePartial, int resultGrade, int countOfQuestions)
		{
			var countOfAnswers = question.Question.Answers.Count;
			var countOfRightAnswers = 0;
			var countOfWrongAnswers = 0;
			var countOfAnswersOfUser = question.Answers.Count;
			var countOfRightAnswersTarget = question.Question.Answers.Where(x => x.IsCorrect).Count();

			double questionWeight = question.Question.Weight != 0 ?
				question.Question.Weight :
				(double)resultGrade / (double)countOfQuestions;

			foreach (var answer in question.Answers)
			{
				var answerId = answer.AnswerId;
				bool isCorrect = question.Question.Answers.FirstOrDefault(x => x.Id == answerId).IsCorrect;
				if (isCorrect)
				{
					countOfRightAnswers++;
				}
				else
				{
					countOfWrongAnswers++;
				}
			}
			if (!toCalculatePartial)
			{
				if (countOfRightAnswers == countOfRightAnswersTarget && countOfWrongAnswers == 0)
				{
					return questionWeight;
				}
				else
				{
					return 0;
				}
			}
			else
			{
				return ((double)countOfRightAnswers / (double)countOfAnswersOfUser) * questionWeight;
			}
		}

		private double GetWeightOfQuestion(Test test, GivenQuestion question)
		{
			return question.Question.Weight != 0 ? question.Question.Weight : (double)test.Grade / test.Questions.Count;
		}
		public void FinishTestResult(int testResultId)
		{
			var entity = uOw.TestResultRepo.GetByID(testResultId);
			double resultGrade = 0;
			bool toCalculatePartial = entity.Test.ConsiderPartialAnswers;
			foreach (var question in entity.Questions)
			{
				resultGrade += GetGradeForQuestion(question, toCalculatePartial, entity.Test.Grade, entity.Questions.Count);
			}

			entity.Status = Common.StatusTest.Finished;
			entity.TimeEnd = DateTime.Now;
			entity.ResultGrade = resultGrade;
			try
			{
				uOw.TestResultRepo.Update(entity);
				uOw.Save();
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
			}
		}

		public IEnumerable<TestResultDTO> GetAll()
		{
			List<TestResultDTO> resultList = new List<TestResultDTO>();
			try
			{
				foreach (var testResult in uOw.TestResultRepo.GetAll())
				{
					resultList.Add(Mapper.Map<TestResultDTO>(testResult));
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
			}
			return resultList;
		}

		public TestResultDTO GetByID(int id)
		{
			return Mapper.Map<TestResultDTO>(uOw.TestResultRepo.GetByID(id));
		}

		public GivenQuestionDTO GetGivenQuestionByTestIdAndQuestionOrderId(int id, int questionOrderId)
		{
			var entity = uOw.TestResultRepo.GetByID(id);
			var firstQuestionId = entity.Questions.FirstOrDefault().Id;
			var targetQuestion = entity.Questions.FirstOrDefault(x => x.Id == (questionOrderId + firstQuestionId));
			return GivenQuestionDTO.ConvertFromEntity(targetQuestion);
		}

		public TestResultShowDTO GetResultShowById(int testResultId)
		{
			var testResult = uOw.TestResultRepo.GetByID(testResultId);
			var testResultModel = Mapper.Map<TestResultDTO>(testResult);

			double allGrades = 0;
			double maxGrade = 0;
			foreach (var question in testResult.Questions)
			{
				var gradeForQuestion = GetGradeForQuestion(question, testResult.Test.ConsiderPartialAnswers, testResult.Test.Grade, testResult.Questions.Count);
				var maxGradeForQuestion = GetWeightOfQuestion(testResult.Test, question);

				allGrades += gradeForQuestion;
				maxGrade += maxGradeForQuestion;

				if (gradeForQuestion == 0)
				{
					testResultModel.Questions.FirstOrDefault(x => x.Id == question.Id).Status = Common.StatusQuestion.Wrong;
				}
				else if (gradeForQuestion < maxGradeForQuestion)
				{
					testResultModel.Questions.FirstOrDefault(x => x.Id == question.Id).Status = Common.StatusQuestion.PartialRight;
				}
				else
				{
					testResultModel.Questions.FirstOrDefault(x => x.Id == question.Id).Status = Common.StatusQuestion.Right;
				}
			}
			var testResultShow = new TestResultShowDTO
			{
				TestResult = testResultModel,
				PercantageOfRight = (int)((allGrades / maxGrade) * 100)
			};

			return testResultShow;
		}

		public void SetAnswerToQuestion(int testId, int questionId, List<int> answersId)
		{
			var entity = uOw.TestResultRepo.GetByID(testId);
			var question = entity.Questions.FirstOrDefault(x => x.Id == questionId);

			question.Answers?.Clear();
			question.Answers = new List<GivenAnswer>();

			foreach (var answerId in answersId)
			{
				question.Answers.Add(new GivenAnswer
				{
					QuestionId = question.Id,
					AnswerId = answerId
				});
			}
			uOw.TestResultRepo.Update(entity);
			uOw.Save();
		}

		public TestStatsDTO GetTestStatsById(int id)
		{
			var test = uOw.TestRepo.GetByID(id);
			double? avarageGrade = test.TestHistory?.Average(x => x.ResultGrade);
			int? countOfAccepted = test.TestHistory?.Where(x => (x.ResultGrade >= (test.Requirment / 100) * test.Grade)).Count();
			int percantageOfAccepted = countOfAccepted.HasValue ? (int)((double)countOfAccepted / (double)test.TestHistory.Count() * 100) : 0;
			Dictionary<int, int> questionCountOfRightAnswersStats = new Dictionary<int, int>();
			Dictionary<double, int> testResultsGradesStats = new Dictionary<double, int>();
			if (test.Questions != null)
			{
				for (int i = 0; i < test.Questions.Count + 1; i++)
				{
					questionCountOfRightAnswersStats.Add(i, 0);
				}
			}

			if (test.TestHistory != null)
			{
				foreach (var testResult in test.TestHistory)
				{
					int i = 0;
					foreach (var question in testResult.Questions)
					{
						i++;
						var gradeForQuestion = GetGradeForQuestion(question, testResult.Test.ConsiderPartialAnswers, testResult.Test.Grade, testResult.Questions.Count);
						var maxGradeForQuestion = GetWeightOfQuestion(testResult.Test, question);

						if (gradeForQuestion == maxGradeForQuestion)
						{
							questionCountOfRightAnswersStats[i] += 1;
						}
					}
					if (!testResultsGradesStats.ContainsKey(testResult.ResultGrade))
					{
						testResultsGradesStats.Add(testResult.ResultGrade, 0);
					}

					testResultsGradesStats[testResult.ResultGrade] += 1;
				}
			}

			var maxNumber = questionCountOfRightAnswersStats.Skip(1).Max(x => x.Value);
			var numberOfEasiest = questionCountOfRightAnswersStats.FirstOrDefault(x => x.Value == maxNumber).Key;

			var minNumber = questionCountOfRightAnswersStats.Skip(1).Min(x => x.Value);
			var numberOfHardest = questionCountOfRightAnswersStats.FirstOrDefault(x => x.Value == minNumber).Key;

			var easiestQuestion = test.Questions.Skip(numberOfEasiest - 1).Take(1).FirstOrDefault();
			var hardestQuestion = test.Questions.Skip(numberOfHardest - 1).Take(1).FirstOrDefault();

			var model = new TestStatsDTO
			{
				BarsOfGrades = testResultsGradesStats,
				BarsOfQuestions = questionCountOfRightAnswersStats,
				AvarageGrade = avarageGrade.HasValue ? avarageGrade.Value : 0,
				PercantageOfAccepted = percantageOfAccepted,
				Test = Mapper.Map<TestDTO>(test),
				EasiestQuestion = Mapper.Map<QuestionDTO>(easiestQuestion),
				HardestQuestion = Mapper.Map<QuestionDTO>(hardestQuestion),
				Results = new List<TestResultDTO>()
			};
			foreach (var testResult in test.TestHistory.OrderByDescending(x => x.TimeStart))
			{
				model.Results.Add(Mapper.Map<TestResultDTO>(testResult));
			}
			return model;
		}

		public bool[] GetGivenAnswers(int id)
		{
			bool[] results = new bool[300];
			var entity = uOw.TestResultRepo.GetByID(id);
			int i = 0;
			foreach (var question in entity.Questions)
			{
				if (question.Answers != null && question.Answers.Count > 0)
				{
					results[i] = true;
				}
				i++;
			}
			return results;
		}

		public bool CheckTestResultIsClosed(int id)
		{
			var entity = uOw.TestResultRepo.GetByID(id);
			if (entity.TimeStart.AddMinutes(entity.Test.TimeLimit) < DateTime.Now)
			{
				return true;
			}
			return false;
		}

		public DateTime GetMaxTimeEnd(int id)
		{
			var entity = uOw.TestResultRepo.GetByID(id);
			return entity.TimeStart.AddMinutes(entity.Test.TimeLimit);
		}

		public UserTestStatsDTO GetUserStatsForTests(string userId)
		{
			var testResults = uOw.TestResultRepo.GetByUserId(userId);
			bool isTestResultsPresent = testResults != null && testResults.Count() > 0;
			double? avarageGrade = isTestResultsPresent ? testResults?.Average(x => x.ResultGrade) : 0;
			int? countOfAccepted = testResults?.Where(x => (x.ResultGrade >= (x.Test.Requirment / 100) * x.Test.Grade)).Count();
			int? percantageOfAccepted = (int)((double)(countOfAccepted ?? 0) / ((double?)testResults?.Count() ?? 1) * 100);
			var testResultsModel = new List<TestResultDTO>();

			double? bestGrade = isTestResultsPresent ? testResults?.Max(x => x.ResultGrade) : 0;
			var bestTestResult = isTestResultsPresent?  testResults?.FirstOrDefault(x => x.ResultGrade == bestGrade) : null;

			double? worthestGrade = isTestResultsPresent ? testResults?.Min(x => x.ResultGrade) : null;
			var worthestTestResult = isTestResultsPresent ? testResults?.FirstOrDefault(x => x.ResultGrade == worthestGrade) : null;
			if (isTestResultsPresent)
			{
				foreach (var testResult in testResults.OrderByDescending(x => x.TimeStart))
				{
					testResultsModel.Add(Mapper.Map<TestResultDTO>(testResult));
				}
			}
			var model = new UserTestStatsDTO
			{
				AvarageGrade = avarageGrade.HasValue ? avarageGrade.Value : 0,
				PercantageOfAccepted = percantageOfAccepted.HasValue ? percantageOfAccepted.Value : 0,
				TestResults = testResultsModel ?? new List<TestResultDTO>(),
				UserName = testResultsModel.FirstOrDefault()?.User.FullName,
				BestTest = bestTestResult != null ? Mapper.Map<TestResultDTO>(bestTestResult) : null,
				WorthestTest = worthestTestResult != null ? Mapper.Map<TestResultDTO>(worthestTestResult) : null,
			};

			return model;
		}

		public bool CheckIfTestForUserId(int id, string userId)
		{
			var testResult = uOw.TestResultRepo.GetByID(id);
			return (testResult.UserId == userId);
		}

		public bool CheckIfTestHasTestResuls(int id)
		{
			return uOw.TestRepo.GetByID(id).TestHistory.Any();
		}
	}
}
