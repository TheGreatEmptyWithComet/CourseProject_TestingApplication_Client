using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingClientApp
{
    internal class GeneralStatRecord
    {
        public int PlaceInGroup { get; private set; }
        public string UserName { get; private set; }
        public int Score { get; private set; }
        public int QuestionAmount { get; private set; }
        public double ScorePercentage { get; private set; }

        public GeneralStatRecord(int place, string userName, int score, int questionAmount, double scorePercentage)
        {
            this.PlaceInGroup = place;
            this.UserName = userName;
            this.Score = score;
            this.QuestionAmount = questionAmount;
            this.ScorePercentage = scorePercentage;
        }
    }
}
