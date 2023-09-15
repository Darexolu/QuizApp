using QuizApp.Models;

namespace QuizApp.Services
{
    public interface IQuestionService
    {
        List<Question> GetQuestionsForView(int ViewNumber);
    }
}
