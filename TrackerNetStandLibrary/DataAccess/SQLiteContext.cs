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
        //public DbSet<TeamModel> Teams { get; set; }
        //public DbSet<TeamMemberModel> TeamMembers { get; set; }

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
            modelBuilder.Entity<PrizeModel>().ToTable("Prizes");
            modelBuilder.Entity<PersonModel>().ToTable("People");
            //modelBuilder.Entity<TeamModel>().ToTable("Teams");
            //modelBuilder.Entity<TeamMemberModel>().ToTable("TeamMembers");
        }
    }
}
