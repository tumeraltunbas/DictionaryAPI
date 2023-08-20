using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Domain.Entities
{
    public class EntryFavorite: BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid EntryId { get; set; }
        public User User { get; set; }
        public Entry Entry { get; set; }

        public EntryFavorite(Guid userId, Guid entryId)
        {
            UserId = userId;
            EntryId = entryId;
        }

    }
}
