using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace KS.GuessAthlete.Logic.AthletePicker.Hockey
{
    /// <summary>
    /// Picks a random athlete based on some criteria
    /// </summary>
    public class HockeyAthletePicker
    {
        protected IRepositoryCollection Repository { get; set; }

        public HockeyAthletePicker(IRepositoryCollection repository)
        {
            Repository = repository;
        }

        public async Task<Athlete> PickAthlete(int skaterGamesPlayed, int skaterPoints,
            decimal skaterPPG, int goalieGamesPlayed, int goalieWins, int startYear)
        {
            IAthleteRepository athleteRepository = Repository.Athletes();
            IDraftRepository draftRepository = Repository.Drafts();
            IJerseyNumberRepository jerseyNumberRepository = Repository.JerseyNumbers();
            IAthleteAwardRepository athleteAwardRepository = Repository.AthleteAwards();
            ISkaterStatLineRepository skaterStatLineRepository = Repository.SkaterStatLines();
            IGoalieStatLineRepository goalieStatLineRepository = Repository.GoalieStatLines();

            List<int> athleteIds = (await athleteRepository
                .SkatersForCriteria(skaterGamesPlayed, skaterPoints, skaterPPG, startYear))
                .ToList();
            athleteIds.AddRange(await athleteRepository
                .GoaliesForCriteria(goalieGamesPlayed, goalieWins, startYear));

            Random rnd = new Random();
            int chosenId = athleteIds[rnd.Next(0, athleteIds.Count)];

            Athlete athlete = await athleteRepository.Get(chosenId);
            athlete.Drafts = await draftRepository.ForAthlete(chosenId);
            athlete.JerseyNumbers = await jerseyNumberRepository.ForAthlete(chosenId);
            athlete.Awards = await athleteAwardRepository.ForAthlete(chosenId);

            if (athlete.Position.ToUpper() == "G")
            {
                athlete.Stats = await goalieStatLineRepository.ForAthlete(chosenId);
            }
            else
            {
                athlete.Stats = await skaterStatLineRepository.ForAthlete(chosenId);
            }

            return athlete;
        }
    }
}
