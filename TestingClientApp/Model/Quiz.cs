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
    //public enum UserAnswerType { Correct, Incorrect, Partial }

    internal class Quiz : NotifyPropertyChangedHandler
    {
        #region Events
        /****************************************************************************************/
        //private Statistic statistic = new Statistic();
        //private QuestionDataBase QuestionDataBase = new QuestionDataBase();
        #endregion


        #region Properties
        /****************************************************************************************/
        private NetworkClient networkClient;
        public string UserLogin { get; set; }
        public ListCollectionView QuestionForQuiz { get; private set; }                                         //+++++++++
        private List<Question> storedQuestions;                                                                 //+++++++++

        public int CorrectAnswersAmount { get; private set; } = 0;
        private double userScore;
        public double UserScore
        {
            get => userScore;
            private set
            {
                userScore = value;
                NotifyPropertyChanged(nameof(UserScore));
            }
        }
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
            //SetCategoriesList();
        }
        #endregion


        #region Methods
        /****************************************************************************************/
        // NEW
        public void SetCurretnTest(int testId)
        {
            CurrentTest = TestsList.Where((t) => t.Id == testId).FirstOrDefault();
        }

        public async Task PrepareQuizQuestion()
        {
            storedQuestions = null;
            UserScore = 0;

            // get question from server
            storedQuestions = await networkClient.GetTestQuestions(CurrentTest.Id);

            // Add references to question that were removed from answers while converting to json
            storedQuestions.ForEach((q) =>
            {
                foreach (Answer answer in q.Answers)
                {
                    answer.Question = q;
                }
            });

            List<QuestionVM> questionsVM = storedQuestions.Select((q) => new QuestionVM(q)).ToList();

            ICollectionView tempView = CollectionViewSource.GetDefaultView(questionsVM);
            QuestionForQuiz = (ListCollectionView)tempView;
        }
        public async void FinishQuiz()
        {
            Tuple<double, List<int>> result = await networkClient.GetTestResult(storedQuestions);
            UserScore = result.Item1;
            WriteUserAnswerTypeToQuestions(result.Item2);

            //EstimateAnswers();
            //SaveStatistic();
            //SetUserPlaceInGroup();
        }
        private void WriteUserAnswerTypeToQuestions(List<int> answers)
        {
            for (int i = 0; i < storedQuestions.Count; ++i)
            {
                storedQuestions[i].UserAnswerType = (UserAnswerType)answers[i];
            }
        }




        // Private methods
        //private void SetCategoriesList()
        //{
        //    // get the category image names list
        //    List<string> categoryImageNames = QuestionDataBase.CategoryImagesList.Select(i => Path.GetFileNameWithoutExtension(i)).ToList();

        //    int imageNameListIndex;

        //    foreach (var categoryName in QuestionDataBase.CategoryNamesList)
        //    {
        //        if (categoryImageNames.Contains(categoryName))
        //        {
        //            // get the index of image name in the list
        //            imageNameListIndex = categoryImageNames.IndexOf(categoryName);
        //            // add the new category with image that meet the category name
        //            string fullImagePath = Path.GetFullPath(QuestionDataBase.CategoryImagesList[imageNameListIndex]);
        //            CategoriesList.Add(new QuestionCategory(categoryName, fullImagePath));
        //        }
        //    }
        //}

        // Public methods






        //private void EstimateAnswers()
        //{
        //    CorrectAnswersAmount = 0;
        //    bool isCorrectlyAnswered;

        //    foreach (QuizQuestionRecord record in QuestionForQuiz)
        //    {
        //        isCorrectlyAnswered = true;

        //        foreach (AnswerOption answer in record.AnswerOptions)
        //        {
        //            if (answer.IsUserChecked != answer.IsCorrect)
        //            {
        //                isCorrectlyAnswered = false;
        //                break;
        //            }
        //        }

        //        record.IsCorrectlyAnswered = isCorrectlyAnswered;
        //        if (isCorrectlyAnswered)
        //        {
        //            ++CorrectAnswersAmount;
        //        }
        //    }

        //    UserScore = CorrectAnswersAmount * Properties.Settings.Default.TotalQuizScore / QuestionForQuiz.Count;
        //}
        //private void SaveStatistic()
        //{
        //    statistic.AddNewRecord(new UserStatRecord(DateTime.Now, UserLogin, QuestionCategoryName, CorrectAnswersAmount, Properties.Settings.Default.QuizQuestionAmount, CorrectAnswersAmount * 100 / Properties.Settings.Default.QuizQuestionAmount));
        //}
        //private void SetUserPlaceInGroup()
        //{
        //    UserPlaseInGroup = statistic.GetUserRating(UserLogin).Item1;
        //    CorrectAnswersPercentage = statistic.GetUserRating(UserLogin).Item2;
        //}
        //public void SetUserStatistic()
        //{
        //    UserStatData = statistic.GetUserStatistic(UserLogin);
        //}
        //public void SetCategoryStatistic()
        //{
        //    UserStatData = statistic.GetCategoryStatistic(TopResultsAmount, QuestionCategoryName);
        //}
        //public void SetGeneralStatistic()
        //{
        //    GeneralStatData = statistic.GetGeneralStatistic();
        //}
        #endregion

    }
}

