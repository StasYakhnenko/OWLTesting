using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.DB
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Grade { get; set; }
        public double Requirment { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool ConsiderPartialAnswers { get; set; }
        public byte TimeLimit { get; set; }
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<TeacherTest> TeacherTests { get; set; }
        public virtual ICollection<TestResult> TestHistory { get; set; }
    }
}
