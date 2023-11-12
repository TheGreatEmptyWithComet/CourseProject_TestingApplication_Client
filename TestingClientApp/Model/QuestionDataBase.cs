using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingClientApp
{
    internal class QuestionDataBase
    {
        private Dictionary<string, string> questionFilesList = new Dictionary<string, string>();
        private Dictionary<string, List<StoredQuestionRecord>> questionBase = new Dictionary<string, List<StoredQuestionRecord>>();
        private List<StoredQuestionRecord> questionListForQuiz = new List<StoredQuestionRecord>();
        private Random random = new Random();

        public List<string> CategoryNamesList { get; private set; } = new List<string>();
        public List<string> CategoryImagesList { get; private set; } = new List<string>();

        public QuestionDataBase()
        {
            InitQuestionFilesList();
            FillQuestionBaseFromFiles();
            InitCategoryNamesList();
            InitCategoryImagesList();
        }

        private void InitQuestionFilesList()
        {
            List<string> questionFileNames = Directory.EnumerateFiles(Properties.Settings.Default.QuestionsTextDirectory).ToList();

            foreach (var file in questionFileNames)
            {
                questionFilesList.Add(Path.GetFileNameWithoutExtension(file), file);
            }
        }
        private void FillQuestionBaseFromFiles()
        {
            string questionsAsJson = string.Empty;

            foreach (var file in questionFilesList)
            {
                if (File.Exists(file.Value))
                {
                    List<StoredQuestionRecord> questionlist = new List<StoredQuestionRecord>();
                    questionsAsJson = File.ReadAllText(file.Value);
                    questionlist = JsonConvert.DeserializeObject<List<StoredQuestionRecord>>(questionsAsJson);

                    if (questionlist != null)
                    {
                        questionBase.Add(file.Key, questionlist);
                    }
                }
            }
        }
        private void InitCategoryNamesList()
        {
            // add categories that have file with questions
            foreach (var questionList in questionBase)
            {
                CategoryNamesList.Add(questionList.Key);
            }
            // add custom category
            CategoryNamesList.Add(Properties.Settings.Default.CustomCategoryName);
        }
        private void InitCategoryImagesList()
        {
            CategoryImagesList = Directory.EnumerateFiles(Properties.Settings.Default.CategoryImagesDirectory).ToList();
        }

        private void FillQuizListWithCertanCategoryQuestions(string category)
        {
            //check the sufficiency of questions of certain category to fill the question list
            if (questionBase[category].Count < Properties.Settings.Default.QuizQuestionAmount)
            {
                throw new Exception($"Error! The chosen category has less questions ({questionBase[category].Count}) that is needed for the quiz ({Properties.Settings.Default.QuizQuestionAmount})");
            }

            int questionIndex = 0;

            //create a temp list of questions (is nedeed to avoid a question reselection)
            List<StoredQuestionRecord> questionsTempList = new List<StoredQuestionRecord>(questionBase[category]);

            //fill the main question list from the temp list with random questions.
            for (int i = 0; i < Properties.Settings.Default.QuizQuestionAmount; ++i)
            {
                questionIndex = random.Next(questionsTempList.Count);

                questionListForQuiz.Add(questionsTempList[questionIndex]);

                //remove question from temp list to avoid it reselection
                questionsTempList.RemoveAt(questionIndex);
            }
        }
        private void FillQuizListWithCustomCategoryQuestions()
        {
            //check the sufficiency of questions in the base to fill the question list
            int baseQuestionAmount = 0;
            foreach (var questioList in questionBase)
            {
                baseQuestionAmount += questioList.Value.Count;
            }

            if (baseQuestionAmount < Properties.Settings.Default.QuizQuestionAmount)
            {
                throw new Exception($"Error! The question base has less questions ({baseQuestionAmount}) that is needed for the quiz ({Properties.Settings.Default.QuizQuestionAmount})");
            }

            int categoryIndex = 0;
            int questionIndex = 0;

            //create a temp deep copy of question base (is nedeed to avoid a question reselection)
            Dictionary<string, List<StoredQuestionRecord>> questionBaseTemp = new Dictionary<string, List<StoredQuestionRecord>>();
            questionBaseTemp = questionBase.ToDictionary(e => e.Key, e => e.Value.ToList());

            for (int i = 0; i < Properties.Settings.Default.QuizQuestionAmount; ++i)
            {
                categoryIndex = random.Next(questionBaseTemp.Count);
                questionIndex = random.Next(questionBaseTemp.ElementAt(categoryIndex).Value.Count);

                //select question from base with random category and on random number and add to list for quiz
                questionListForQuiz.Add(questionBaseTemp.ElementAt(categoryIndex).Value[questionIndex]);

                //remove question to avoid it reselection
                questionBaseTemp.ElementAt(categoryIndex).Value.RemoveAt(questionIndex);

                //if category has no question remove it from the temp base
                if (questionBaseTemp.ElementAt(categoryIndex).Value.Count == 0)
                {
                    questionBaseTemp.Remove(questionBaseTemp.ElementAt(categoryIndex).Key);
                }
            }
        }

        public List<StoredQuestionRecord> GetQuizQuestionList(string category)
        {
            questionListForQuiz.Clear();

            if (category == Properties.Settings.Default.CustomCategoryName)
            {
                FillQuizListWithCustomCategoryQuestions();
            }
            else if (questionBase.ContainsKey(category))
            {
                FillQuizListWithCertanCategoryQuestions(category);
            }
            else
            {
                Console.WriteLine("ERROR: there is no chosen category of questions");
            }

            return questionListForQuiz;
        }
    }
}
