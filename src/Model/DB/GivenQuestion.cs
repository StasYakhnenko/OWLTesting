using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.DB
{
    public class GivenQuestion
    {
        public int Id { get; set; }
        public Question Question { get; set; }
        public int QuestionId { get; set; }
        public TestResult TestResult { get; set; }
        public int TestResultId { get; set; }
        public ICollection<GivenAnswer> Answers { get; set; }
    }
}
