using QuizApp.Models;

namespace QuizApp.Repositories.IRepository
{
    public interface IQuizRepository : IRepository<Question>
    {
        void Update(Question obj);
        void Save();
    }
}
