using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string ImageLink { get; set; }
        public double Weight { get; set; }
        public int TestId { get; set; }
        public TestDTO Test { get; set; }
        public List<AnswerDTO> Answers { get; set; }
    }
}
