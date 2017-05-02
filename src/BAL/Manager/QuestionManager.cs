using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Interface;
using BAL.Interface;
using Model.DTO;
using AutoMapper;
using Model.DB;
using Microsoft.Extensions.Logging;

namespace BAL.Manager
{
	public class QuestionManager : BaseManager, IQuestionManager
	{
		private readonly ILogger logger;
		public QuestionManager(IUnitOfWork uOw, ILogger<QuestionManager> logger) : base(uOw)
		{
			this.logger = logger;
		}

		public QuestionDTO GetByID(int id)
		{
			return Mapper.Map<QuestionDTO>(uOw.QuestionRepo.GetByID(id));
		}

		public void UpdateQuestion(QuestionDTO model)
		{
			if ((model.Answers?.Count ?? 0) == 0)
			{
				throw new Exception("Питання немає жодної відповіді!");
			}
			try
			{
				var entity = uOw.QuestionRepo.GetByID(model.Id);
				entity.ImageLink = model.ImageLink;
				entity.Text = model.Text;
				foreach (var oldAnswer in entity.Answers)
				{
					uOw.AnswerRepo.SetStateModified(oldAnswer);
					uOw.AnswerRepo.Delete(oldAnswer);
				}
				entity.Answers?.Clear();
				entity.Answers = new List<Answer>();
				if (model.Answers != null)
				{
					foreach (var answer in model.Answers)
					{
						answer.QuestionId = entity.Id;
						var targetAnswer = Mapper.Map<Answer>(answer);
						targetAnswer.Question = entity;
						entity.Answers.Add(targetAnswer);
						uOw.AnswerRepo.Insert(targetAnswer);
					}
				}
				uOw.QuestionRepo.Update(entity);
				uOw.QuestionRepo.SetStateModified(entity);
				uOw.Save();
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
			}
		}
	}
}
