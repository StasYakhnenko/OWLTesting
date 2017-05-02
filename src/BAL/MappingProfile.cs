using AutoMapper;
using Model.DB;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAL
{
	public class MappingProfile : Profile
	{
		protected override void Configure()
		{
			base.Configure();

			CreateMap<Subject, SubjectDTO>()
				.ForMember(x => x.Tests,
				y => y.MapFrom(z => z.Tests.Select(a => new TestDTO {
					Name = a.Name,
					EndDate = a.EndDate,
					Id = a.Id
				})));
			CreateMap<SubjectDTO, Subject>();

			CreateMap<Test, TestDTO>()
				.ForMember(p => p.Questions,
				x => x.MapFrom(
					y => y.Questions.Select(z => new QuestionDTO()
					{
						Id = z.Id,
						ImageLink = z.ImageLink,
						Text = z.Text,
						Answers = z.Answers != null ? z.Answers.Select(f => new AnswerDTO() { Id = f.Id, Text = f.Text, IsCorrect = f.IsCorrect, ImageLink = f.ImageLink }).ToList() : null
					}
					)))
				.ForMember(p => p.Teachers, x => x.MapFrom(z => z.TeacherTests.Select(k => new UserDTO() { Id = k.UserId })))
				.ForMember(p => p.Subject, x => x.MapFrom(z => new SubjectDTO() { Name = z.Subject.Name }));

			CreateMap<TestDTO, Test>()
				.ForMember(p => p.Subject, x => x.Ignore())
				.ForMember(p => p.Questions, x => x.Ignore());

			CreateMap<Question, QuestionDTO>()
				.ForMember(p => p.Test, x => x.Ignore())
				.ForMember(p => p.Answers,
					x => x.MapFrom(
						y => y.Answers != null ?
						y.Answers.Select(z => new AnswerDTO() { Id = z.Id, Text = z.Text, IsCorrect = z.IsCorrect, ImageLink = z.ImageLink })
						: null
						));

			CreateMap<QuestionDTO, Question>()
				.ForMember(p => p.Test, x => x.Ignore())
				.ForMember(p => p.Answers,
					x => x.MapFrom(
						y => y.Answers != null ?
						y.Answers.Select(z => new Answer() { Id = z.Id, Text = z.Text, IsCorrect = z.IsCorrect, ImageLink = z.ImageLink })
						: null
						));

			CreateMap<Answer, AnswerDTO>()
				.ForMember(p => p.Question, x => x.Ignore());

			CreateMap<AnswerDTO, Answer>()
				.ForMember(p => p.Question, x => x.Ignore());

			CreateMap<ApplicationUser, UserDTO>()
				.ForMember(x => x.FullName, y => y.MapFrom(z => z.FirstName + " " + z.LastName));

			CreateMap<GivenQuestion, GivenQuestionDTO>()
				.ForMember(p => p.TestResult, x => x.Ignore())
				.ForMember(p => p.Question, x => x.MapFrom(y => new QuestionDTO {
					Id = y.Id,
					ImageLink = y.Question.ImageLink,
					Text = y.Question.Text,
					Answers = y.Answers.Select(e => new AnswerDTO
					{
						Id = e.Id,
						Text = e.Answer.Text,
						ImageLink = e.Answer.ImageLink,
						IsCorrect = e.Answer.IsCorrect,
					}).ToList()
				}))
				.ForMember(p => p.Answers,
					x => x.MapFrom(
						y => y.Answers != null ?
						y.Answers.Select(z => new GivenAnswerDTO() {
							Id = z.Id,
							AnswerId = z.AnswerId,
							QuestionId = z.QuestionId,
							Answer = new AnswerDTO() {
								Id = z.Answer.Id,
								Text = z.Answer.Text,
								IsCorrect = z.Answer.IsCorrect,
								ImageLink = z.Answer.ImageLink
							} })
						: null
						));

			CreateMap<GivenQuestionDTO, GivenQuestion>()
				.ForMember(p => p.TestResult, x => x.Ignore())
				.ForMember(p => p.Answers,
					x => x.MapFrom(
						y => y.Answers != null ?
						y.Answers.Select(z => new GivenAnswer() {
							Id = z.Id,
							AnswerId = z.AnswerId,
							QuestionId = z.QuestionId
						})
						: null
						));

			CreateMap<TestResult, TestResultDTO>()
				.ForMember(p => p.Questions, x => x.MapFrom(y => y.Questions != null ?
				y.Questions.Select(z => new GivenQuestionDTO()
				{
					Id = z.Id,
					QuestionId = z.QuestionId,
					TestResultId = z.TestResultId,
					Question = new QuestionDTO
					{
						Id = z.Question.Id,
						ImageLink = z.Question.ImageLink,
						Text = z.Question.Text,
						Answers = z.Question.Answers.Select(d => new AnswerDTO
						{
							IsCorrect = d.IsCorrect,
							ImageLink = d.ImageLink,
							Text = d.Text
						}).ToList()
					},
					Answers = z.Answers.Select(a => new GivenAnswerDTO
					{
						AnswerId = a.Id,
						Answer = new AnswerDTO
						{
							ImageLink = a.Answer.ImageLink,
							IsCorrect = a.Answer.IsCorrect,
							QuestionId = a.Answer.QuestionId,
							Text = a.Answer.Text
						}
					}).ToList()
				}) : null ));
		}
	}
}
