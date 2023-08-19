using Slugify;
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

        public Title(string content)
        {
            Content = content;
            Entries = new List<Entry>() { };
            Slug = GenerateSlug(Content);
        }

        private string GenerateSlug(string content)
        {
            SlugHelper slugHelper = new();
            return slugHelper.GenerateSlug(content);
        }

    }
}
