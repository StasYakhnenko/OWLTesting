using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class UserTestStatsDTO
    {
		public string UserName { get; set; }
		public double AvarageGrade { get; set; }
		public int PercantageOfAccepted { get; set; }
		public SubjectDTO HardestSubject { get; set; }
		public SubjectDTO EasiestSubject { get; set; }
		public TestResultDTO BestTest { get; set; }
		public TestResultDTO WorthestTest { get; set; }
		public List<TestResultDTO> TestResults { get; set; }
	}
}
