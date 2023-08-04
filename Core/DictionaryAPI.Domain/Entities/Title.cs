using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Domain.Entities
{
    public class Title : BaseEntity
    {
        public string Content { get; set; }
        public string Slug { get; set; }

        public ICollection<Entry> Entries { get; set; }

    }
}
