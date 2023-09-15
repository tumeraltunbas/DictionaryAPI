using DictionaryAPI.Application.Utils.Result;
using DictionaryAPI.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.Abstracts.Business
{
    public interface IEntryVoteService
    {
        Result EntryUpVote(string entryId);
        Result EntryDownVote(string entryId);
        Result UndoVote(string entryId);
        Result GetVotedEntries(string voteType);

    }
}
