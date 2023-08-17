using DictionaryAPI.Application.Abstracts.Repository;
using DictionaryAPI.Domain.Entities;
using DictionaryAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Persistence.Concretes.Repository
{
    public class EntityRepositoryBase<T> : IEntityRepositoryBase<T>
        where T : BaseEntity
        //Generic Type must be instance of BaseEntity and can be created new instance of Generic Type with new()
    {
        public void Add(T entity)
        {
            using (DictionaryContext context = new())
            {
                context.Add(entity);
                context.SaveChanges();
            }
        }

        public void Delete(T entity)
        {
            using (DictionaryContext context = new())
            {
                context.Remove(entity);
                context.SaveChanges();
            }
        }

        public List<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            using (DictionaryContext context = new())
            {
                return context.Set<T>().ToList();
            }
        }

        public T GetById(Guid id)
        {
            using (DictionaryContext context = new())
            {
                return context.Set<T>().FirstOrDefault(e => e.Id == id);
            }
        }

        public void Update(T entity)
        {
            using (DictionaryContext context = new())
            {
                context.Update(entity);
                context.SaveChanges();
            }
        }
    }
}
