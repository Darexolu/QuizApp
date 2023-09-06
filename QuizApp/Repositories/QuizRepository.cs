using QuizApp.Data;
using QuizApp.Models;
using QuizApp.Repositories.IRepository;

using System.Linq.Expressions;
using QuizApp.Repositories.IRepository;

namespace QuizApp.Repositories
{
    public class QuizRepository : Repository<Question>, IQuizRepository
    {
        public readonly AppDbContext _db;
        public QuizRepository(AppDbContext db): base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Question obj)
        {
            _db.Questions.Update(obj);
        }
    }
}
