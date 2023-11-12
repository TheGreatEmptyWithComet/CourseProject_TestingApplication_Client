using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingClientApp
{
    internal class Statistic
    {
        private List<UserStatRecord> statData;

        public Statistic()
        {
            this.statData = new List<UserStatRecord>();
            LoadStatDataFromFile();
        }

        private void LoadStatDataFromFile()
        {
            if (File.Exists(Properties.Settings.Default.StatDataFileName))
            {
                string statDataAsJson = File.ReadAllText(Properties.Settings.Default.StatDataFileName);
                List<UserStatRecord> statData = JsonConvert.DeserializeObject<List<UserStatRecord>>(statDataAsJson);
                if (statData != null)
                {
                    this.statData = statData;
                }
            }
        }
        private void SaveStatDataToFile()
        {
            string statDataAsJson = JsonConvert.SerializeObject(statData, Formatting.Indented);
            File.WriteAllText(Properties.Settings.Default.StatDataFileName, statDataAsJson);
        }

        public void AddNewRecord(UserStatRecord record)
        {
            statData.Add(record);
            SaveStatDataToFile();
        }

        public Tuple<int, int> GetUserRating(string userLogin)
        {
            var allUsersRatingTable = statData.GroupBy(u => u.UserName, (name, user) => new { name = name, score = user.Sum(u => u.Score), scorePers = user.Sum(u => u.Score) * 100 / user.Sum(u => u.QuestionAmount) }).OrderByDescending(r => r.scorePers).ThenByDescending(r => r.score);

            int userRating = allUsersRatingTable.Select(u => u.name).ToList().IndexOf(userLogin) + 1;
            int userTotalScorePersentage = allUsersRatingTable.Where(u => u.name == userLogin).Select(u => u.scorePers).First();
            return new Tuple<int, int>(userRating, userTotalScorePersentage);
        }
        public List<UserStatRecord> GetUserStatistic(string userLogin)
        {
            List<UserStatRecord> userStatData = statData.Where(r => r.UserName == userLogin).OrderByDescending(r => r.Date).ToList();
            return userStatData;
        }
        public List<UserStatRecord> GetCategoryStatistic(int resultsAmount, string category)
        {
            List<UserStatRecord> userStatData = statData.Where(r => r.Category == category).OrderByDescending(r => r.ScorePercentage).ThenByDescending(r => r.Score).Take(resultsAmount).ToList();
            return userStatData;
        }
        public List<GeneralStatRecord> GetGeneralStatistic()
        {
            var allUsersRatingTable = statData.GroupBy(u => u.UserName, (name, user) => new { name = name, score = user.Sum(u => u.Score), questAmnt = user.Sum(u => u.QuestionAmount), scorePers = user.Sum(u => u.Score) * 100 / user.Sum(u => u.QuestionAmount) }).OrderByDescending(r => r.scorePers).ThenByDescending(r => r.score);

            List<GeneralStatRecord> generalStatistic = new List<GeneralStatRecord>();

            int placeInGroup = 0;
            foreach (var user in allUsersRatingTable)
            {
                generalStatistic.Add(new GeneralStatRecord(++placeInGroup, user.name, user.score, user.questAmnt, user.scorePers));
            }

            return generalStatistic;
        }
    }
}
