using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OwlTesting.ViewModels
{
    public class TestQuestionViewModel
    {
		public GivenQuestionDTO Question { get; set; }
		public int CurrentQuestionOrder { get; set; }
		public int QuestionsCount { get; set; }
		public bool[] GivenAnswers { get; set; }
		public DateTime MaxTimeEnd { get; set; }
	}
}
