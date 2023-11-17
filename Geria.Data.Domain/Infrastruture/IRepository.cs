using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Geria.Data.Domain.Infrastruture
{
    public interface IRepository<T> where T : class
    {
        IUnitOfWork UnitOfWork { get; }
        void Add(T entity);
        void Update(T entity);
        IQueryable<T> GetMany(Expression<Func<T, bool>> where);
        public T Get(Expression<Func<T, bool>> where);
        IQueryable<T> GetAll(Expression<Func<T, bool>> where = null);
    }
}
