using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Model;
using Model.IRepos;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected readonly ArtasticContext _dbcontext = null;
        protected readonly DbSet<T> _dbSet;
        public RepositoryBase(ArtasticContext context)
        {
            _dbcontext = context;
            _dbSet = _dbcontext.Set<T>();
        }

        public async Task<bool> Exists(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<long> Count(Expression<Func<T, bool>> predicate = null)
        {
            return await _dbSet.LongCountAsync(predicate);
        }

        public async Task<int> Delete(T entity)
        {
            _dbSet.Remove(entity);
            return await _dbcontext.SaveChangesAsync();
        }
        public async Task<T> Get(Expression<Func<T, bool>> predicate)   
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }
        public async Task<IEnumerable<T>> GetAllList(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _dbSet.ToListAsync();
            }
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllList<Tkey>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, Tkey>> orderBy = null)
        {
            if (orderBy == null)
            {
                if (predicate == null)
                {
                    return await _dbSet.ToListAsync();
                }
                else
                {
                    return await _dbSet.Where(predicate).ToListAsync();
                }
            }
            else
            {
                if (predicate == null)
                {
                    return await _dbSet.OrderByDescending(orderBy).ToListAsync();
                }
                else
                {
                    return await _dbSet.Where(predicate).OrderByDescending(orderBy).ToListAsync();
                }
            }
        }

        public async Task<IEnumerable<T>> GetPageList(PageRequest page, Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _dbSet.Skip((page.PageNumber) * page.PageSize).Take(page.PageSize).ToListAsync();
            }
            else
            {
                return await _dbSet.Where(predicate).Skip((page.PageNumber) * page.PageSize).Take(page.PageSize).ToListAsync();
            }
        }

        public async Task<IEnumerable<T>> GetPageList<Tkey>(PageRequest page, Expression<Func<T, bool>> predicate = null, Expression<Func<T, Tkey>> orderBy = null)
        {
            if (orderBy == null)
            {
                if (predicate == null)
                {
                    return await _dbSet.Skip((page.PageNumber) * page.PageSize).Take(page.PageSize).ToListAsync();
                }
                else
                {
                    return await _dbSet.Where(predicate).Skip((page.PageNumber) * page.PageSize).Take(page.PageSize).ToListAsync();
                }
            }
            else
            {
                if (predicate == null)
                {
                    return await _dbSet.OrderByDescending(orderBy).Skip((page.PageNumber) * page.PageSize).Take(page.PageSize).ToListAsync();
                }
                else
                {
                    return await _dbSet.Where(predicate).OrderByDescending(orderBy).Skip((page.PageNumber) * page.PageSize).Take(page.PageSize).ToListAsync();
                }
            }
        }

        public async Task<int> Insert(T entity)
        {
            await _dbSet.AddAsync(entity);
            return await _dbcontext.SaveChangesAsync();
        }
        public async Task<int> Update(T entity)
        {
            _dbSet.Attach(entity);
            _dbcontext.Entry(entity).State = EntityState.Modified;
            return await _dbcontext.SaveChangesAsync();
        }
    }
}
