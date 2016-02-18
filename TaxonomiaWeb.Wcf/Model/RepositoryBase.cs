using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace TaxonomiaWeb.Wcf.Model
{
    public class RepositoryBase<TContext> : IDisposable where TContext : System.Data.Entity.DbContext, new()
    {
        private TContext _DataContext;

        protected virtual TContext DataContext
        {
            get
            {
                if (_DataContext == null)
                {
                    _DataContext = new TContext();
                }
                return _DataContext;
            }
        }

        public virtual IQueryable<T> GetAll<T>() where T : class
        {
            using (DataContext)
            {
                return DataContext.Set<T>();
            }
        }

        public virtual T FindSingleBy<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            if (predicate != null)
            {
                using (DataContext)
                {
                    return DataContext.Set<T>().Where(predicate).SingleOrDefault();
                }
            }
            else
            {
                throw new ArgumentNullException("Predicate value must be passed to FindSingleBy<T>.");
            }
        }

        public virtual IQueryable<T> FindAllBy<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            if (predicate != null)
            {
                using (DataContext)
                {
                    return DataContext.Set<T>().Where(predicate);
                }
            }
            else
            {
                throw new ArgumentNullException("Predicate value must be passed to FindAllBy<T>.");
            }
        }

        public virtual IQueryable<T> FindBy<T, TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderBy) where T : class
        {
            if (predicate != null)
            {
                if (orderBy != null)
                {
                    using (DataContext)
                    {
                        return FindAllBy<T>(predicate).OrderBy(orderBy).AsQueryable<T>(); ;
                    }
                }
                else
                {
                    throw new ArgumentNullException("OrderBy value must be passed to FindBy<T,TKey>.");
                }
            }
            else
            {
                throw new ArgumentNullException("Predicate value must be passed to FindBy<T,TKey>.");
            }
        }

        public virtual int Save<T>(T Entity) where T : class
        {
            return DataContext.SaveChanges();
        }
        public virtual int Update<T>(T Entity) where T : class
        {
            return DataContext.SaveChanges();
        }
        public virtual int Delete<T>(T entity) where T : class
        {
            DataContext.Set<T>().Remove(entity);
            return DataContext.SaveChanges();
        }
        public void Dispose()
        {
            if (DataContext != null) DataContext.Dispose();
        }
    }
}