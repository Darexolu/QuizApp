namespace QuizApp.Models
{
    public class CustomViewViewModel
    {
        public List<Question> Questions { get; set; }
        public int ViewNumber { get; set; }
        public List<SubmittedQuestionViewModel> SubmittedQuestions { get; set; }
        
    }
}

