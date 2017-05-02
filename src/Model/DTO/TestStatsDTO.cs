using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class TestStatsDTO
    {
		public TestDTO Test { get; set; }
		public List<TestResultDTO> Results { get; set; }
		public double AvarageGrade { get; set; }
		public int PercantageOfAccepted { get; set; }
		public Dictionary<double, int> BarsOfGrades { get; set; }
		public Dictionary<int, int> BarsOfQuestions { get; set; }
		public QuestionDTO HardestQuestion { get; set; }
		public QuestionDTO EasiestQuestion { get; set; }
	}
}
