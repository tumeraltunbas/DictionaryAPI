using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.Application.Abstracts.DAL;
using DictionaryAPI.Application.Utils.Constants;
using DictionaryAPI.Application.Utils.Result;
using DictionaryAPI.Domain.Entities;
using DictionaryAPI.Domain.Entities.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Persistence.Concretes.Business
{
    public class EntryVoteService : IEntryVoteService
    {

        IEntryVoteDal _entryVoteDal;
        IEntryDal _entryDal;
        IHttpContextAccessor _contextAccessor;

        public EntryVoteService(IEntryVoteDal entryVoteDal, IEntryDal entryDal, IHttpContextAccessor contextAccessor)
        {
            _entryVoteDal = entryVoteDal;
            _entryDal = entryDal;
            _contextAccessor = contextAccessor;
        }

        public Result EntryUpVote(string entryId)
        {
            Entry entry = _entryDal.GetSingle(e => e.Id == Guid.Parse(entryId));

            if(entry == null)
            {
                return new ErrorResult(Message.EntryNotFound);
            }

            EntryUpVote upVote = new(
                userId: Guid.Parse(Convert.ToString(_contextAccessor.HttpContext.Items["Id"])),
                entryId: entry.Id
            );

            _entryVoteDal.Add(upVote);

            return new SuccessResult(Message.VoteCreated);
        }
    }
}
