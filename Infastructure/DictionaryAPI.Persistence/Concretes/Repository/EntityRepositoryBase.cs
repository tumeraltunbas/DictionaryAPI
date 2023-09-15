using DictionaryAPI.Application.Abstracts.Repository;
using DictionaryAPI.Application.Utils.Constants;
using DictionaryAPI.Application.Utils.Result;
using DictionaryAPI.Domain.Abstract.Entity;
using DictionaryAPI.Domain.Entities;
using DictionaryAPI.Persistence.Contexts;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Persistence.Concretes.Repository
{
    public class EntityRepositoryBase<T> : IEntityRepositoryBase<T>
        where T : class, IEntity

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

        public T GetSingle(Expression<Func<T, bool>> filter)
        {
            using (DictionaryContext context = new())
            {
                return context.Set<T>().SingleOrDefault(filter);
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
