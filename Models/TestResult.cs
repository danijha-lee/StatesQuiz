using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StatesQuiz.Models
{
    public class TestResult
    {
        [Key]
        public int TestResultId { get; set; }

        public int UserId { get; set; }
        public DateTime TestDateTime { get; set; }
        public int TotalQuestions { get; set; }
        public int NumberCorrect { get; set; }

        public virtual User User { get; set; }
    }
}