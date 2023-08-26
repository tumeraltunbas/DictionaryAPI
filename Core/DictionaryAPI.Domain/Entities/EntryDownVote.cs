using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Domain.Entities
{
    public class EntryDownVote : EntryVote
    {
        public EntryDownVote(Guid userId, Guid entryId) : base(userId, entryId, Enums.VoteType.DownVote)
        {
            
        }

    }
}
