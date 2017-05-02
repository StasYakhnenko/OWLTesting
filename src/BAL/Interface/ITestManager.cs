using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAL.Interface
{
    public interface ITestManager
    {
        List<TestDTO> GetAll();
        TestDTO GetById(int id);
        int AddTest(TestDTO test);
        void UpdateTest(TestDTO test);
        void AddQuestionToTest(QuestionDTO question, int id);
		bool CheckUserAccess(int id, string userId);
		List<TestDTO> GetTestsForTeacher(string userId);
		List<TestDTO> GetTestsToPass();
	}
}
