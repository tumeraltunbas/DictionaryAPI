using DictionaryAPI.Application.Abstracts.DAL;
using DictionaryAPI.Domain.Entities;
using DictionaryAPI.Persistence.Concretes.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Persistence.Concretes.DAL
{
    public class TitleDal : EntityRepositoryBase<Title>, ITitleDal
    {

    }
}
