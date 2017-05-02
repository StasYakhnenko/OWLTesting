using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.DB
{
    public class TestResult
    {
        public int Id { get; set; }
        public DateTime TimeStart { get; set; }
		public DateTime? TimeEnd { get; set; }
		public Test Test { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public int TestId { get; set; }
        public double ResultGrade { get; set; }
        public StatusTest Status { get; set; }
        public ICollection<GivenQuestion> Questions { get; set; }
    }
}
