using DictionaryAPI.Application.Abstracts.DAL;
using DictionaryAPI.Domain.Entities;
using DictionaryAPI.Persistence.Concretes.Repository;
using DictionaryAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Persistence.Concretes.DAL
{
    public class TitleDal : EntityRepositoryBase<Title>, ITitleDal
    {
        DictionaryContext _context;

        public TitleDal(DictionaryContext context)
        {
            _context = context;
        }

        public Title GetBySlug(string slug)
        {
            return _context.Set<Title>().SingleOrDefault(s => s.Slug == slug);
        }
    }
}
