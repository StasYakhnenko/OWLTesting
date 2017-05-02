using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.DB
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string ImageLink { get; set; }
        public double Weight { get; set; }
        public int TestId { get; set; }
        public virtual Test Test { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
