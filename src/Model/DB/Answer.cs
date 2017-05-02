using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.DB
{
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public string ImageLink { get; set; }
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}
