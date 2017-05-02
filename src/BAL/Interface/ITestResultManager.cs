using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAL.Interface
{
    public interface ITestResultManager
    {
        int CreateTestResult(int testId, string userId);
        TestResultDTO GetByID(int id);
        void SetAnswerToQuestion(int testId, int questionId, List<int> answersId);
        IEnumerable<TestResultDTO> GetAll();
        GivenQuestionDTO GetGivenQuestionByTestIdAndQuestionOrderId(int id, int questionOrderId);
		void FinishTestResult(int testResultId);
		TestResultShowDTO GetResultShowById(int testResultId);
		TestStatsDTO GetTestStatsById(int id);
		bool[] GetGivenAnswers(int id);
		bool CheckTestResultIsClosed(int id);
		DateTime GetMaxTimeEnd(int id);
		UserTestStatsDTO GetUserStatsForTests(string userId);
		bool CheckIfTestForUserId(int id, string userId);
		bool CheckIfTestHasTestResuls(int id);
	}
}
