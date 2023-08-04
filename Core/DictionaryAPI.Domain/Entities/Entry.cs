using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Domain.Entities
{
    public class Entry : BaseEntity
    {
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public Guid TitleId { get; set; }
        public User User { get; set; }
        public Title Title { get; set; }
    }
}
