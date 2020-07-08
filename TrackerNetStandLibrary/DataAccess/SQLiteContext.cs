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


            // Team member many to many relationship
            // Teams <=> Person
            modelBuilder.Entity<TeamMemberModel>()
                .HasKey(tm => new { tm.TeamId, tm.PersonId });

            modelBuilder.Entity<TeamMemberModel>()
                .Property(tm => tm.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<TeamMemberModel>()
                .HasOne(tm => tm.TeamModel)
                .WithMany(t => t.TeamMembers)
                .HasForeignKey(tm => tm.TeamId);

            modelBuilder.Entity<TeamMemberModel>()
                 .HasOne(tm => tm.PersonModel)
                 .WithMany(p => p.TeamMemberModels)
                 .HasForeignKey(tm => tm.PersonId);


            // Tournaments relates model builder
            // Relationship between tournament and team
            // tournament and prize
            modelBuilder.Entity<TournamentModel>();

            modelBuilder.Entity<TournamentModel>()
                .HasMany(tm => tm.Rounds)
                .WithOne(r => r.Tournament)
                .HasForeignKey(r => r.TournamentId);


            // Tournament and team, prize many to many relationship
            modelBuilder.Entity<TournamentEntryModel>()
                .HasKey(te => new { te.TournamentId, te.TeamId });

            modelBuilder.Entity<TournamentEntryModel>()
                .Property(te => te.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<TournamentEntryModel>()
                .HasOne(te => te.Team)
                .WithMany(t => t.TournamentEntrys)
                .HasForeignKey(te => te.TeamId);

            modelBuilder.Entity<TournamentEntryModel>()
                .HasOne(te => te.Tournament)
                .WithMany(t => t.TournamentEntryModels)
                .HasForeignKey(te => te.TournamentId);



            // Each match up has many match up entries
            // And each match up has a parent match up
            // Parant match up is foreign key
            modelBuilder.Entity<MatchupModel>()
                .HasMany(m => m.Entries)
                .WithOne(e => e.Matchup)
                .HasForeignKey(e => e.MatchupId);

            modelBuilder.Entity<MatchupModel>()
                .HasOne(m => m.Winner)
                .WithMany(t => t.Matchups)
                .HasForeignKey(m => m.WinnerId).IsRequired(false);


            // Round model
            modelBuilder.Entity<TournamentRoundModel>()
                .HasMany(tr => tr.MatchUps)
                .WithOne(m => m.Round)
                .HasForeignKey(m => m.RoundId);


            // Match up entry model
            modelBuilder.Entity<MatchupEntryModel>()
                .HasOne(me => me.ParentMatchup)
                .WithMany()
                .HasForeignKey(e => e.ParentMatchupId).IsRequired(false);

            modelBuilder.Entity<MatchupEntryModel>()
                .HasOne(me => me.TeamCompeting)
                .WithMany(m => m.MatchupEntrys)
                .HasForeignKey(me => me.TeamCompetingId);

        }
    }
}
