using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingClientApp
{
    internal class UserStatRecord
    {
        public DateTime Date { get; private set; }
        public string UserName { get; private set; }
        public string Category { get; private set; }
        public int Score { get; private set; }
        public int QuestionAmount { get; private set; }
        public double ScorePercentage { get; private set; }

        public UserStatRecord(DateTime date, string userName, string category, int score, int questionAmount, double scorePercentage)
        {
            this.Date = date;
            this.UserName = userName;
            this.Category = category;
            this.Score = score;
            this.QuestionAmount = questionAmount;
            this.ScorePercentage = scorePercentage;
        }
        
    }
}
