using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Model.IRepos
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllList(Expression<Func<T, bool>> predicate = null);
        Task<IEnumerable<T>> GetAllList<Tkey>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, Tkey>> orderBy = null);
        Task<IEnumerable<T>> GetPageList(PageRequest page, Expression<Func<T, bool>> predicate = null);
        Task<IEnumerable<T>> GetPageList<Tkey>(PageRequest page, Expression<Func<T, bool>> predicate = null, Expression<Func<T, Tkey>> orderBy = null);
        Task<T> Get(Expression<Func<T, bool>> predicate);
        Task<int> Insert(T entity);
        Task<int> Delete(T entity);
        Task<int> Update(T entity);
        Task<long> Count(Expression<Func<T, bool>> predicate);
    }
}
