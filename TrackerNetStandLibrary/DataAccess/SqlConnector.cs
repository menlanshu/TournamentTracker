using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public class SqlConnector : IDataConnection
    {
        private SQLiteContext _SQLiteContext;

        public SqlConnector(string connString)
        {
            _SQLiteContext = new SQLiteContext(connString);
        }

        /// <summary>
        /// Save a new Person to the database
        /// </summary>
        /// <param name="model">A new person information</param>
        /// <returns>The person information, include the unique identifier</returns>
        public PersonModel CreatePerson(PersonModel model)
        {
            _SQLiteContext.People.Add(model);
            _SQLiteContext.SaveChanges();

            return model;
        }

        /// <summary>
        /// Saves a new prize to the database
        /// </summary>
        /// <param name="model">The prize information.</param>
        /// <returns>The prize information, including the unique identifier.</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            _SQLiteContext.Prizes.Add(model);
            _SQLiteContext.SaveChanges();

            return model;
        }

        // TODO - Whether there is a better to handle this kind of situation
        public TeamModel CreateTeam(TeamModel model)
        {
            //context.ChangeTracker.TrackGraph(model, node =>
            //    node.Entry.State = !node.Entry.IsKeySet ? EntityState.Added : EntityState.Unchanged);
            //context.Add(model);

            //foreach (var item in model.TeamMemberModels)
            //{
            //    item.PersonModel = _SQLiteContext.People.Find(item.PersonModel.Id);
            //}

            _SQLiteContext.Add(model);
            _SQLiteContext.SaveChanges();

            return model;
        }

        public TournamentModel CreateTounament(TournamentModel tournament)
        {
            _SQLiteContext.Add(tournament);
            _SQLiteContext.SaveChanges();

            return tournament;
        }

        /// <summary>
        /// Get all persons from DB
        /// </summary>
        /// <returns></returns>
        public List<PersonModel> GetPerson_All()
        {
            return _SQLiteContext.People.ToList();
        }

        public List<TeamModel> GetTeam_All()
        {
            return _SQLiteContext.Teams.Include(p => p.TeamMemberModels).ThenInclude(x => x.PersonModel).ToList();
        }
    }
}
