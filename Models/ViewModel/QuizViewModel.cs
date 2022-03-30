using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatesQuiz.Models.ViewModel
{
    public class QuizViewModel
    {
        public List<States> States { get; set; }
        public List<string> Results { get; set; }
    }

    //public class QuizStates : States
    //{
    //    public string QuizCapitol { get; set; }
    //}
}