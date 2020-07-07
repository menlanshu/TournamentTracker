using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public class SQLiteContext : DbContext
    {
        public string ConnString { get; private set; }
        public DbSet<PrizeModel> Prizes { get; set; }
        public DbSet<PersonModel> People { get; set; }
        public DbSet<TeamModel> Teams { get; set; }
        public DbSet<TeamMemberModel> TeamMembers { get; set; }
        public DbSet<TournamentEntryModel> TournamentEntries { get; set; }
        public DbSet<TournamentPrizeModel> TournamentPrizes { get; set; }
        public DbSet<TournamentModel> Tournaments { get; set; }

        public SQLiteContext(string connString)
        {
            ConnString = connString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite(ConnString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PrizeModel>();
            modelBuilder.Entity<PersonModel>();
            modelBuilder.Entity<TeamModel>();

            modelBuilder.Entity<TeamMemberModel>()
                .HasKey(tm => new { tm.TeamId, tm.PersonId });

            modelBuilder.Entity<TeamMemberModel>()
                .Property(tm => tm.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<TeamMemberModel>()
                .HasOne(tm => tm.TeamModel)
                .WithMany(t => t.TeamMemberModels)
                .HasForeignKey(tm => tm.TeamId);

            modelBuilder.Entity<TeamMemberModel>()
                 .HasOne(tm => tm.PersonModel)
                 .WithMany(p => p.TeamMemberModels)
                 .HasForeignKey(tm => tm.PersonId);


            // Tournaments relates model builder
            // Relationship between tournament and team
            // tournament and prize
            modelBuilder.Entity<TournamentModel>();

            modelBuilder.Entity<TournamentEntryModel>()
                .HasKey(te => new { te.TournamentId, te.TeamId });

            modelBuilder.Entity<TournamentEntryModel>()
                .Property(te => te.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<TournamentEntryModel>()
                .HasOne(te => te.Team)
                .WithMany(t => t.TournamentEntryModels)
                .HasForeignKey(te => te.TeamId);

            modelBuilder.Entity<TournamentEntryModel>()
                .HasOne(te => te.Tournament)
                .WithMany(t => t.TournamentEntryModels)
                .HasForeignKey(te => te.TournamentId);


            //Save here as an example for many-to-many ef grammar
            //modelBuilder.Entity<TeamMemberModel>()
            //    .HasOne<TeamModel>(tm => tm.TeamModel)
            //    .WithMany(t => t.TeamMemberModels)
            //    .HasForeignKey(tm => tm.TeamId);

            //modelBuilder.Entity<TeamMemberModel>()
            //     .HasOne<PersonModel>(tm => tm.PersonModel)
            //     .WithMany(p => p.TeamMemberModels)
            //     .HasForeignKey(tm => tm.TeamId);

        }
    }
}
