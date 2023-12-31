﻿using DictionaryAPI.Application.Utils.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.Abstracts.Business
{
    public interface IEntryFavoriteService
    {
        Result FavoriteEntry(string entryId);
        Result UndoFavoriteEntry(string entryId);
        Result GetFavoritedEntries();

    }
}
