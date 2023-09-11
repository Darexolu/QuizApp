namespace QuizApp.Models
{
    public class SubmittedQuestionViewModel
    {
        public int QuestionId { get; set; } // ID of the submitted question
        public int CorrectOptionIndex { get; set; }  //Index of the selected correct option
    }
}
