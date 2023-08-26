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


            ////EntryFavorites
            modelBuilder.Entity<EntryFavorite>() //Two foreign key as a primary key
                .HasKey(ef => new { ef.UserId, ef.EntryId });

            //User can favorite multiple entry
            modelBuilder.Entity<EntryFavorite>()
                .HasOne(ef => ef.User)
                .WithMany(u => u.FavoritedEntries)
                .HasForeignKey(ef => ef.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //An entry can be liked by multiple users
            modelBuilder.Entity<EntryFavorite>()
                .HasOne(ef => ef.Entry)
                .WithMany(e => e.Favorites)
                .HasForeignKey(ef => ef.EntryId)
                .OnDelete(DeleteBehavior.Restrict);

            //EntryVotes
            modelBuilder.Entity<EntryVote>()
                .HasKey(ev => new { ev.UserId, ev.EntryId });

            modelBuilder.Entity<EntryVote>()
                .HasOne(ev => ev.User)
                .WithMany(u => u.VotedEntries)
                .HasForeignKey(ev => ev.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<EntryVote>()
                .HasOne(ev => ev.Entry)
                .WithMany(e => e.Votes)
                .HasForeignKey(ev => ev.EntryId)
                .OnDelete(DeleteBehavior.Restrict);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            configurationManager.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            optionsBuilder.UseSqlServer(configurationManager.GetConnectionString("MSSQL"));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<EntryFavorite> EntryFavorites { get; set; }
        public DbSet<EntryVote> EntryVotes { get; set; }
    }
}
