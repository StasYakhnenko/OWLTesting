using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class TestResultDTO
    {
        public int Id { get; set; }
        public DateTime TimeStart { get; set; }
		public DateTime? TimeEnd { get; set;}
		public TestDTO Test { get; set; }
        public UserDTO User { get; set; }
        public string UserId { get; set; }
        public int TestId { get; set; }
        public double ResultGrade { get; set; }
        public StatusTest Status { get; set; }
        public ICollection<GivenQuestionDTO> Questions { get; set; }
    }
}
