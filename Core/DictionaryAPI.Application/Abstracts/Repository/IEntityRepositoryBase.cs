using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.Abstracts.Repository
{
    public interface IEntityRepositoryBase<T>
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
        T GetSingle(Expression<Func<T, bool>> filter);

    }
}
