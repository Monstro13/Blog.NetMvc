using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> SearchFor(Expression<Func<T, Boolean>> predicate);
        
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
