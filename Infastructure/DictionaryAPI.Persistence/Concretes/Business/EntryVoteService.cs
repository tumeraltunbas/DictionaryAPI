using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.Application.Abstracts.DAL;
using DictionaryAPI.Application.Utils.Constants;
using DictionaryAPI.Application.Utils.Result;
using DictionaryAPI.Domain.Entities;
using DictionaryAPI.Domain.Entities.Enums;
using DictionaryAPI.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        DictionaryContext _context;

        public EntryVoteService(
            IEntryVoteDal entryVoteDal, 
            IEntryDal entryDal, 
            IHttpContextAccessor contextAccessor, 
            DictionaryContext context
        )
        {
            _entryVoteDal = entryVoteDal;
            _entryDal = entryDal;
            _contextAccessor = contextAccessor;
            _context = context;
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
        public Result EntryDownVote(string entryId)
        {
            Entry entry = _entryDal.GetSingle(e => e.Id == Guid.Parse(entryId));

            if (entry == null)
            {
                return new ErrorResult(Message.EntryNotFound);
            }

            EntryVote upVote = _entryVoteDal.GetSingle(
                ev => ev.UserId == Guid.Parse(Convert.ToString(_contextAccessor.HttpContext.Items["Id"]))
                &&
                ev.EntryId == entry.Id
                &&
                ev.VoteType == VoteType.UpVote
             ); 

            if(upVote != null)
            {
                _entryVoteDal.Delete(upVote);
            }

            EntryDownVote downVote = new(
                userId: Guid.Parse(Convert.ToString(_contextAccessor.HttpContext.Items["Id"])),
                entryId: entry.Id
            );

            _entryVoteDal.Add(downVote);

            return new SuccessResult(Message.VoteCreated);

        }

        public Result UndoVote(string entryId)
        {
            Entry entry = _entryDal.GetSingle(e => e.Id == Guid.Parse(entryId));

            if (entry == null)
            {
                return new ErrorResult(Message.EntryNotFound);
            }

            EntryVote vote = _entryVoteDal.GetSingle(
                ev => ev.UserId == Guid.Parse(Convert.ToString(_contextAccessor.HttpContext.Items["Id"]))
                &&
                ev.EntryId == entry.Id
            );

            if(vote == null)
            {
                return new ErrorResult(Message.VoteNotFound);
            }

            _entryVoteDal.Delete(vote);

            return new SuccessResult(Message.VoteDeleted);

        }

        public Result GetUpVotedEntries()
        {
            List<EntryVote> upVotedEntries = _context.EntryVotes
                .Include(ev => ev.Entry)
                .Where(
                    ev => ev.UserId == Guid.Parse(Convert.ToString(_contextAccessor.HttpContext.Items["Id"]))
                    &&
                    ev.VoteType == VoteType.UpVote
                )
                .ToList();

            return new SuccessDataResult<List<EntryVote>>(upVotedEntries);

        }
    }
}
