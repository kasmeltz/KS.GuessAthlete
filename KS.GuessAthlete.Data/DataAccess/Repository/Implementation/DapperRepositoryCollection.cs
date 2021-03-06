﻿using System;
using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Implementation
{
    public class DapperRepositoryCollection : IRepositoryCollection
    {
        protected ICacheProvider CacheProvider { get; set; }

        public DapperRepositoryCollection(ICacheProvider cacheProvider)
        {
            CacheProvider = cacheProvider;
        }

        public IAthleteAwardRepository AthleteAwards()
        {
            return new DapperAthleteAwardRepository(CacheProvider);
        }

        public IAthleteRepository Athletes()
        {
            return new DapperAthleteRepository(CacheProvider);
        }

        public IAwardRepository Awards()
        {
            return new DapperAwardRepository(CacheProvider);
        }

        public IConferenceRepository Conferences()
        {
            return new DapperConferenceRepository(CacheProvider);
        }

        public IDivisionRepository Divisions()
        {
            return new DapperDivisionRepository(CacheProvider);
        }

        public IDraftRepository Drafts()
        {
            return new DapperDraftRepository(CacheProvider);
        }

        public IGoalieStatLineRepository GoalieStatLines()
        {
            return new DapperGoalieStatLineRepository(CacheProvider);
        }

        public IJerseyNumberRepository JerseyNumbers()
        {
            return new DapperJerseyNumberRepository(CacheProvider);
        }

        public ILeagueRepository Leagues()
        {
            return new DapperLeagueRepository(CacheProvider);
        }

        public ISeasonRepository Seasons()
        {
            return new DapperSeasonRepository(CacheProvider);
        }

        public ISkaterStatLineRepository SkaterStatLines()
        {
            return new DapperSkaterStatLineRepository(CacheProvider);
        }

        public ITeamIdentityDivisionRepository TeamIdentityDivisions()
        {
            return new DapperTeamIdentityDivisionRepository(CacheProvider);
        }

        public ITeamIdentityRepository TeamIdentities()
        {
            return new DapperTeamIdentityRepository(CacheProvider);
        }

        public ITeamRepository Teams()
        {
            return new DapperTeamRepository(CacheProvider);
        }        
    }
}
