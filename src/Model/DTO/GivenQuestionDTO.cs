using Common;
using Model.DB;
using System.Collections.Generic;

namespace Model.DTO
{
    public class GivenQuestionDTO
    {
        public int Id { get; set; }
        public QuestionDTO Question { get; set; }
        public int QuestionId { get; set; }
        public TestResultDTO TestResult { get; set; }
        public int TestResultId { get; set; }
        public ICollection<GivenAnswerDTO> Answers { get; set; }

		public StatusQuestion Status { get; set; }

		public static GivenQuestionDTO ConvertFromEntity(GivenQuestion entity)
		{
			var model = new GivenQuestionDTO
			{
				Id = entity.Id,
				TestResultId = entity.TestResultId,
				QuestionId = entity.QuestionId,
				Question = new QuestionDTO
				{
					Id = entity.Question.Id,
					ImageLink = entity.Question.ImageLink,
					Text = entity.Question.Text,
					TestId = entity.Question.TestId,
					Weight = entity.Question.Weight
				},
			};
			model.Question.Answers = new List<AnswerDTO>();
			foreach (var answer in entity.Question.Answers)
			{
				model.Question.Answers.Add(new AnswerDTO
				{
					Id = answer.Id,
					ImageLink = answer.ImageLink,
					QuestionId = answer.QuestionId,
					IsCorrect = answer.IsCorrect,
					Text = answer.Text
				});
			}
			model.Answers = new List<GivenAnswerDTO>();
			if (entity.Answers != null)
			{
				foreach (var answer in entity.Answers)
				{
					model.Answers.Add(new GivenAnswerDTO
					{
						Id = answer.Id,
						AnswerId = answer.AnswerId,
						QuestionId = answer.QuestionId
					});
				}
			}


			return model;
		}
	}
}