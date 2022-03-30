using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StatesQuiz.Models
{
    public class States
    {
        [Key]
        public int StateId { get; set; }

        public string State { get; set; }
        public string Capital { get; set; }
    }
}