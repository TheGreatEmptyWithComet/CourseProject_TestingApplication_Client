using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TestingServerApp;

namespace TestingClientApp
{
    internal class Quiz : NotifyPropertyChangedHandler
    {
        #region Events
        /****************************************************************************************/
        private Statistic statistic = new Statistic();
        private QuestionDataBase QuestionDataBase  = new QuestionDataBase();
        #endregion


        #region Properties
        /****************************************************************************************/
        private NetworkClient networkClient;
        public string UserLogin { get; set; }
        public ListCollectionView QuestionForQuiz { get; private set; }
        public int CorrectAnswersAmount { get; private set; } = 0;
        public int UserScore { get; private set; } = 0;
        public int UserPlaseInGroup { get; private set; } = 0;
        public int CorrectAnswersPercentage { get; private set; } = 0;
        public List<UserStatRecord> UserStatData { get; private set; }
        public List<GeneralStatRecord> GeneralStatData { get; private set; }
        public List<QuestionCategory> CategoriesList { get; private set; } = new List<QuestionCategory>();              //---------
        public List<TestInfo> TestsList { get; set; } = new List<TestInfo>();                                           //+++++++++
        public TestInfo CurrentTest { get; set; }                                                                      //+++++++++


        public string QuestionCategoryName { get; set; }


        private int topResultsAmount;
        public int TopResultsAmount
        {
            get { return topResultsAmount; }
            set
            {
                if (topResultsAmount != value)
                {
                    topResultsAmount = value;
                    NotifyPropertyChanged(nameof(TopResultsAmount));
                }
            }
        }
        #endregion


        #region Constructor
        /****************************************************************************************/
        public Quiz(NetworkClient networkClient)
        {
            this.networkClient = networkClient;
            this.TopResultsAmount = Properties.Settings.Default.TopResultAmount;
            SetCategoriesList();
        }
        #endregion


        #region Methods
        /****************************************************************************************/
        // NEW
        public void SetCurretnTest(int testId)
        {
            CurrentTest = TestsList.Where((t) => t.Id == testId).FirstOrDefault();
        }




        // Private methods
        private void SetCategoriesList()
        {
            // get the category image names list
            List<string> categoryImageNames = QuestionDataBase.CategoryImagesList.Select(i=>Path.GetFileNameWithoutExtension(i)).ToList();

            int imageNameListIndex;
            
            foreach (var categoryName in QuestionDataBase.CategoryNamesList)
            {
                if (categoryImageNames.Contains(categoryName))
                {
                    // get the index of image name in the list
                    imageNameListIndex = categoryImageNames.IndexOf(categoryName);
                    // add the new category with image that meet the category name
                    string fullImagePath = Path.GetFullPath(QuestionDataBase.CategoryImagesList[imageNameListIndex]);
                    CategoriesList.Add(new QuestionCategory(categoryName, fullImagePath));
                }
            }
        }

        // Public methods
        public async Task PrepareQuizQuestion()
        {
            List<Question> storedQuestions = await networkClient.GetTestQuestions(CurrentTest.Id);
            
            //List<StoredQuestionRecord> tempStoredQuestionList = QuestionDataBase.GetQuizQuestionList(QuestionCategoryName);--------------------------------------------------------
            //List<QuizQuestionRecord> tempQuizQuestionList = new List<QuizQuestionRecord>(tempStoredQuestionList.Select(i => new QuizQuestionRecord(i)));----------------------------

            ICollectionView tempView = CollectionViewSource.GetDefaultView(storedQuestions);
            QuestionForQuiz = (ListCollectionView)tempView;
        }
        public void FinishQuiz()
        {
            EstimateAnswers();
            SaveStatistic();
            SetUserPlaceInGroup();
        }
        private void EstimateAnswers()
        {
            CorrectAnswersAmount = 0;
            bool isCorrectlyAnswered;

            foreach(QuizQuestionRecord record in QuestionForQuiz)
            {
                isCorrectlyAnswered = true;

                foreach (AnswerOption answer in record.AnswerOptions)
                {
                    if (answer.IsUserChecked!= answer.IsCorrect)
                    {
                        isCorrectlyAnswered = false;
                        break;
                    }
                }

                record.IsCorrectlyAnswered = isCorrectlyAnswered;
                if (isCorrectlyAnswered)
                {
                    ++CorrectAnswersAmount;
                }
            }

            UserScore = CorrectAnswersAmount * Properties.Settings.Default.TotalQuizScore / QuestionForQuiz.Count;
        }
        private void SaveStatistic()
        {
            statistic.AddNewRecord(new UserStatRecord(DateTime.Now, UserLogin, QuestionCategoryName, CorrectAnswersAmount, Properties.Settings.Default.QuizQuestionAmount, CorrectAnswersAmount * 100 / Properties.Settings.Default.QuizQuestionAmount));
        }
        private void SetUserPlaceInGroup()
        {
            UserPlaseInGroup = statistic.GetUserRating(UserLogin).Item1;
            CorrectAnswersPercentage = statistic.GetUserRating(UserLogin).Item2;
        }
        public void SetUserStatistic()
        {
            UserStatData = statistic.GetUserStatistic(UserLogin);
        }
        public void SetCategoryStatistic()
        {
            UserStatData = statistic.GetCategoryStatistic(TopResultsAmount, QuestionCategoryName);
        }
        public void SetGeneralStatistic()
        {
            GeneralStatData = statistic.GetGeneralStatistic();
        }
        #endregion

    }
}

