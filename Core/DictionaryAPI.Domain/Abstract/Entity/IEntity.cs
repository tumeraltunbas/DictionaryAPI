using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Domain.Abstract.Entity
{
    public interface IEntity
    {
        public bool IsVisible { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
