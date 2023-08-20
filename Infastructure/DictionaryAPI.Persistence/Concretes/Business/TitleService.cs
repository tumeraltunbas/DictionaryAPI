using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.Application.Abstracts.DAL;
using DictionaryAPI.Application.Utils.Constants;
using DictionaryAPI.Application.Utils.Result;
using DictionaryAPI.Domain.Entities;
using DictionaryAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Persistence.Concretes.Business
{
    public class TitleService : ITitleService
    {
        ITitleDal _titleDal;
        DictionaryContext _context;
        public TitleService(ITitleDal titleDal, DictionaryContext context)
        {
            _titleDal = titleDal;
            _context = context;
        }

        public Result GetTitleBySlug(string slug)
        {

            if(slug == null)
            {
                return new ErrorResult(Message.TitleNotFound);
            }

            Title title = _context.Set<Title>()
                                  .Include(t => t.Entries
                                   .Where(e => e.IsVisible == true))
                                  .FirstOrDefault(t => t.Slug == slug);

            if (title == null)
            {
                return new ErrorResult(Message.TitleNotFound);
            }

            return new SuccessDataResult<Title>(title);
        }
    }
}
