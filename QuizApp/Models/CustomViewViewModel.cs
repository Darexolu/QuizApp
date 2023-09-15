namespace QuizApp.Models
{
    public class CustomViewViewModel
    {
        public int ViewNumber { get; set; }
        public List<Question> Questions { get; set; }
        public List<SubmittedQuestionViewModel> SubmittedQuestions { get; set; }
        public List<List<bool>> SelectedOptions { get; set; }

    }
}

