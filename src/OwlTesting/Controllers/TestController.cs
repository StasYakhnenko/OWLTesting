using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using BAL.Interface;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Model.DB;
using Microsoft.AspNetCore.Authorization;
using OwlTesting.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OwlTesting.Controllers
{
	public class TestController : Controller
	{
		// GET: /<controller>/
		private readonly ITestManager testManager;
		private readonly IUserManager userManager;
		private readonly ISubjectManager subjectManager;
		private readonly IQuestionManager questionManager;
		private readonly ITestResultManager testResultManager;
		private UserManager<ApplicationUser> userIdenityManager;
		public TestController(ITestManager testManager, IUserManager userManager, ISubjectManager subjectManager, IQuestionManager questionManager, ITestResultManager testResultManager, UserManager<ApplicationUser> userIdenityManager)
		{
			this.testManager = testManager;
			this.userManager = userManager;
			this.subjectManager = subjectManager;
			this.questionManager = questionManager;
			this.testResultManager = testResultManager;
			this.userIdenityManager = userIdenityManager;
		}
		[Authorize(Roles = "Administrator")]
		public IActionResult Index()
		{
			var tests = testManager.GetAll();
			return View(tests);
		}
		[Authorize(Roles = "Administrator,Teacher")]
		public IActionResult AddTest()
		{
			var model = new TestDTO();
			model.TeachersAll = userManager.GetAllTeachers();
			model.Subjects = subjectManager.GetAll();

			return View(model);
		}
		[Authorize(Roles = "Administrator,Teacher")]
		public IActionResult EditQuestions(int id)
		{
			System.Security.Claims.ClaimsPrincipal currentUser = this.User;
			if (User.IsInRole("Administrator") || testManager.CheckUserAccess(id, userIdenityManager.GetUserId(currentUser)))
			{
				var test = testManager.GetById(id);
				return View(test);
			}
			return View("AccessDenied");
		}

		[HttpPost]
		[Authorize(Roles = "Administrator,Teacher")]
		public IActionResult AddTest(TestDTO model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					int newId = testManager.AddTest(model);
					return RedirectToAction("EditQuestions", new { @id = newId });
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}
			model.Teachers = userManager.GetAllTeachers();
			model.Subjects = subjectManager.GetAll();
			return View("AddTest", model);
		}
		[Authorize(Roles = "Administrator,Teacher")]
		public IActionResult EditTest(int id)
		{
			System.Security.Claims.ClaimsPrincipal currentUser = this.User;
			if (User.IsInRole("Administrator") || testManager.CheckUserAccess(id, userIdenityManager.GetUserId(currentUser)))
			{
				var test = testManager.GetById(id);
				test.TeachersAll = userManager.GetAllTeachers();
				test.Subjects = subjectManager.GetAll();
				return View(test);
			}
			return View("AccessDenied");
		}
		[HttpPost]
		[Authorize(Roles = "Administrator,Teacher")]
		public IActionResult EditTest(TestDTO model)
		{
			if (ModelState.IsValid)
			{
				System.Security.Claims.ClaimsPrincipal currentUser = this.User;
				if (User.IsInRole("Administrator") || testManager.CheckUserAccess(model.Id, userIdenityManager.GetUserId(currentUser)))
				{
					try
					{
						testManager.UpdateTest(model);
						return RedirectToAction("EditQuestions", new { @id = model.Id });
					}
					catch(Exception ex)
					{
						ModelState.AddModelError(string.Empty, ex.Message);
					}
				}
				return View("AccessDenied");
			}
			model.Teachers = userManager.GetAllTeachers();
			model.Subjects = subjectManager.GetAll();
			return View("EditTest",model);
		}
		[HttpGet]
		[Authorize(Roles = "Administrator,Teacher")]
		public IActionResult AddQuestion(int testId)
		{
			System.Security.Claims.ClaimsPrincipal currentUser = this.User;
			if (User.IsInRole("Administrator") || testManager.CheckUserAccess(testId, userIdenityManager.GetUserId(currentUser)))
			{
				var model = new QuestionDTO();
				model.TestId = testId;
				return PartialView("_AddQuestion", model);
			}
			return View("AccessDenied");
		}

		[HttpPost]
		[Authorize(Roles = "Administrator,Teacher")]
		public IActionResult AddQuestion(QuestionDTO question)
		{
			if (ModelState.IsValid)
			{
				try
				{
					System.Security.Claims.ClaimsPrincipal currentUser = this.User;
					if (User.IsInRole("Administrator") || testManager.CheckUserAccess(question.TestId, userIdenityManager.GetUserId(currentUser)))
					{
						testManager.AddQuestionToTest(question, question.TestId);
						return RedirectToAction("EditQuestions", new { @id = question.TestId });
					}
					return View("AccessDenied");
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}
			return View("_AddQuestion", question);
		}

		[HttpGet]
		[Authorize(Roles = "Administrator,Teacher")]
		public IActionResult EditQuestion(int questionId)
		{
			var model = questionManager.GetByID(questionId);
			return PartialView("_EditQuestion", model);
		}

		[HttpPost]
		[Authorize(Roles = "Administrator,Teacher")]
		public IActionResult EditQuestion(QuestionDTO question)
		{
			if (ModelState.IsValid)
			{
				try
				{
					questionManager.UpdateQuestion(question);
					return RedirectToAction("EditQuestions", new { @id = question.TestId });
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}
			return View("_EditQuestion", question);
		}
		[HttpPost]
		public IActionResult GetAnswerTemplate(int count)
		{
			return PartialView("_AnswerTemplate", count);
		}
		[HttpGet]
		[Authorize(Roles = "Administrator,Teacher")]
		public IActionResult PreviewQuestion(int questionId)
		{
			var model = questionManager.GetByID(questionId);
			return PartialView("_PreviewQuestion", model);
		}

		[Authorize]
		public IActionResult PassTest(int id)
		{
			System.Security.Claims.ClaimsPrincipal currentUser = this.User;
			string userId = userIdenityManager.GetUserId(currentUser);
			int newTestResultId = testResultManager.CreateTestResult(id, userId);
			return RedirectToAction("PassTestQuestion", new { @id = newTestResultId });
		}
		[Authorize]
		public IActionResult PassTestQuestion(int id, int questionOrderId = 0)
		{
			System.Security.Claims.ClaimsPrincipal currentUser = this.User;
			string userId = userIdenityManager.GetUserId(currentUser);
			bool isNotAccessDenied = testResultManager.CheckIfTestForUserId(id, userId);
			if (!isNotAccessDenied)
			{
				return View("AccessDenied");
			}

			bool toFinishTest = testResultManager.CheckTestResultIsClosed(id);
			if (toFinishTest)
			{
				testResultManager.FinishTestResult(id);
				return RedirectToAction("ShowResult", new { @id = id });
			}

			GivenQuestionDTO model = testResultManager.GetGivenQuestionByTestIdAndQuestionOrderId(id, questionOrderId);
			int questionsCount = testResultManager.GetByID(model.TestResultId).Questions.Count;
			TestQuestionViewModel testQuestion = new TestQuestionViewModel
			{
				Question = model,
				CurrentQuestionOrder = questionOrderId,
				QuestionsCount = questionsCount,
				GivenAnswers = testResultManager.GetGivenAnswers(id),
				MaxTimeEnd = testResultManager.GetMaxTimeEnd(id)
			};

			return View(testQuestion);
		}

		[HttpPost]
		[Authorize]
		public IActionResult GiveAnswerForQuestion(int testResultId, int questionid, int questionOrderId, List<int> answerId)
		{
			System.Security.Claims.ClaimsPrincipal currentUser = this.User;
			string userId = userIdenityManager.GetUserId(currentUser);
			bool isNotAccessDenied = testResultManager.CheckIfTestForUserId(testResultId, userId);
			if (!isNotAccessDenied)
			{
				return View("AccessDenied");
			}

			bool toFinishTest = testResultManager.CheckTestResultIsClosed(testResultId);
			if (toFinishTest)
			{
				testResultManager.FinishTestResult(testResultId);
				return RedirectToAction("ShowResult", new { @id = testResultId });
			}
			testResultManager.SetAnswerToQuestion(testResultId, questionid, answerId);
			int questionsCount = testResultManager.GetByID(testResultId).Questions.Count;
			if (questionsCount == questionOrderId + 1)
			{
				testResultManager.FinishTestResult(testResultId);
				return RedirectToAction("ShowResult", new { @id = testResultId });
			}
			return RedirectToAction("PassTestQuestion", new { @id = testResultId, @questionOrderId = questionOrderId + 1 });
		}
		[Authorize]
		public IActionResult ShowResult(int id)
		{
			var model = testResultManager.GetResultShowById(id);
			return View(model);
		}
		[Authorize(Roles = "Administrator,Teacher")]
		public IActionResult HistoryForTest(int id)
		{
			if (testResultManager.CheckIfTestHasTestResuls(id))
			{
				var model = testResultManager.GetTestStatsById(id);
				return View(model);
			}
			return View("NotAppropriateTest");
		}
		[Authorize]
		public IActionResult TestReview(int id)
		{
			var model = testManager.GetById(id);
			if (model.Questions.Count == 0)
			{
				return View("NotQuestionsTest");
			}
			return View(model);
		}
		[Authorize]
		public IActionResult TestsByUser()
		{
			System.Security.Claims.ClaimsPrincipal currentUser = this.User;
			string userId = userIdenityManager.GetUserId(currentUser);
			var model = testResultManager.GetUserStatsForTests(userId);
			model.UserName = userIdenityManager.GetUserName(currentUser);
			return View(model);
		}
		[Authorize(Roles = "Teacher")]
		public IActionResult TestsForTeacher()
		{
			System.Security.Claims.ClaimsPrincipal currentUser = this.User;
			string userId = userIdenityManager.GetUserId(currentUser);
			var tests = testManager.GetTestsForTeacher(userId);
			return View(tests);
		}
		[AllowAnonymous]
		public IActionResult TestsForPass()
		{
			var tests = testManager.GetTestsToPass();
			return View(tests);
		}
	}
}
