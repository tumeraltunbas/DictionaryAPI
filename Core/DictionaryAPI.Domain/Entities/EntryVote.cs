using DictionaryAPI.Domain.Abstract.Entity;
using DictionaryAPI.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Domain.Entities
{
    public class EntryVote: IEntity
    {
        public Guid UserId { get; set; }
        public Guid EntryId { get; set; }
        public VoteType VoteType { get; set; }
        public User User { get; set; }
        public Entry Entry { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedAt { get; set; }

        public EntryVote(Guid userId, Guid entryId, VoteType voteType)
        {
            UserId = userId;
            EntryId = entryId;
            VoteType = voteType;
        }

    }
}
