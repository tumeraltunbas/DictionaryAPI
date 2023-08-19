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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //User - Entry one to many
            modelBuilder.Entity<User>()
                .HasMany(u => u.Entries) //User has many entries
                .WithOne(e => e.User) //Entry has one User
                .HasForeignKey(e => e.UserId);

            //Title and Entry one to many
            modelBuilder.Entity<Title>()
                .HasMany(t => t.Entries)
                .WithOne(e => e.Title)
                .HasForeignKey(e => e.TitleId);


            //User.Username Unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            //User.Email Unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            //Title.slug Unique
            modelBuilder.Entity<Title>()
                .HasIndex(t => t.Slug)
                .IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            configurationManager.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            optionsBuilder.UseSqlServer(configurationManager.GetConnectionString("MSSQL"));
        }

        public DbSet<User> Users { get; set; }

    }
}
