using DictionaryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DictionaryAPI.Persistence.Contexts
{
    public class DictionaryContext : DbContext
    {

        ConfigurationManager configurationManager = new();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            configurationManager.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            optionsBuilder.UseSqlServer(configurationManager.GetConnectionString("MSSQL"));
        }

        public DbSet<User> Users { get; set; }

    }
}
