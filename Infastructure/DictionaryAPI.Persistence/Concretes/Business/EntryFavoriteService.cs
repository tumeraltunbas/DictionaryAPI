using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.Application.Abstracts.DAL;
using DictionaryAPI.Application.Utils.Constants;
using DictionaryAPI.Application.Utils.Result;
using DictionaryAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Persistence.Concretes.Business
{
    public class EntryFavoriteService : IEntryFavoriteService
    {
        IEntryDal _entryDal;
        IEntryFavoriteDal _entryFavoriteDal;
        IHttpContextAccessor _contextAccessor;

        public EntryFavoriteService(IEntryDal entryDal, IEntryFavoriteDal entryFavoriteDal, IHttpContextAccessor contextAccessor)
        {
            _entryDal = entryDal;
            _entryFavoriteDal = entryFavoriteDal;
            _contextAccessor = contextAccessor;
        }

        public Result FavoriteEntry(string entryId)
        {
            Entry entry = _entryDal.GetSingle(e => e.Id == Guid.Parse(entryId));
            
            if(entry == null || entry.IsVisible != true)
            {
                return new ErrorResult(Message.EntryNotFound);
            }

            EntryFavorite entryFavorite = new(
                entryId: entry.Id,
                userId: Guid.Parse(Convert.ToString(_contextAccessor.HttpContext.Items["Id"]))
                );

            _entryFavoriteDal.Add(entryFavorite);

            return new SuccessResult();
        }

        public Result UndoFavoriteEntry(string entryId)
        {
            Entry entry = _entryDal.GetSingle(e => e.Id == Guid.Parse(entryId));
           
            if (entry == null || entry.IsVisible != true)
            {
                return new ErrorResult(Message.EntryNotFound);
            }

            EntryFavorite favorite = _entryFavoriteDal.GetSingle(
                ef => ef.UserId == Guid.Parse(Convert.ToString(_contextAccessor.HttpContext.Items["Id"]))
                &&
                ef.EntryId == Guid.Parse(entryId)
            );

            if(favorite == null)
            {
                return new ErrorResult(Message.FavoriteNotFound);
            }

            _entryFavoriteDal.Delete(favorite);

            return new SuccessResult(Message.FavoriteDeleted);
        }
    }
}
