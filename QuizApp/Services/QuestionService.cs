using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Models;

namespace QuizApp.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly AppDbContext _dbContext;
        public QuestionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Question> GetQuestionsForView(int ViewNumber)
        {
            int startId = 0;
            int endId = 0;

            switch (ViewNumber)
            {
                case 1:
                    startId = 1;
                    endId = 3;
                    break;
                case 2:
                    startId = 4;
                    endId = 6;
                    break;
                case 3:
                    startId = 7;
                    endId = 9;
                    break;
                default:
                    // Handle other cases or return an empty list
                    return new List<Question>();
            }

            return _dbContext.Questions
                .Where(q => q.Id >= startId && q.Id <= endId)
                .ToList();

        }
    }
}
