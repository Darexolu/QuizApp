namespace QuizApp.Models
{
    public class SubmittedQuestion
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int CorrectOptionIndex { get; set; }
        public int ViewNumber { get; set; }
    }
}
