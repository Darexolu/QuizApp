using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using QuizApp.Data;
using Microsoft.EntityFrameworkCore;
using QuizApp.Repositories.IRepository;

namespace QuizApp.Repositories
{
    public class Repository<T>: IRepository<T> where T : class
    {

        public readonly AppDbContext _db;

        internal DbSet<T> dbSet;
        public Repository(AppDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }
       public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }
        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }


    }
}
