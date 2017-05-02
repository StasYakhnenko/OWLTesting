using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class AnswerDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
		public bool IsDeleted { get; set; } = false;
        public string ImageLink { get; set; }
        public int QuestionId { get; set; }
        public QuestionDTO Question { get; set; }
    }
}
