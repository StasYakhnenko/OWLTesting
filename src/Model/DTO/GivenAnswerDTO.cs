namespace Model.DTO
{
    public class GivenAnswerDTO
    {
        public int Id { get; set; }
        public AnswerDTO Answer { get; set; }
        public int AnswerId { get; set; }
        public GivenQuestionDTO Question { get; set; }
        public int QuestionId { get; set; }
    }
}