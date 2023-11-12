using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingClientApp
{
    internal class QuestionCategory
    {
        public string Name { get; private set; }
        public string LogoImagePath { get; private set; }
        
        public QuestionCategory(string name, string logoImagePath)
        {
            Name = name;
            LogoImagePath = logoImagePath;
        }
    }
}
