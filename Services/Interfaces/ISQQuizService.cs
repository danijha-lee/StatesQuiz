using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatesQuiz.Models;

namespace StatesQuiz.Services.Interfaces
{
    public interface ISQQuizService
    {
        public Task<User> RegisterUserAsync(string UserName, string Password, string FirstName, string Email);

        public Task<List<States>> GetStatesAsync();

        public Task<User> SearchUserNameAsync(string UserName);

        public Task<List<States>> GetQuizQuestionsAsync();

        public Task<bool> CheckUserInput(States input);

        public Task<TestResult> GradeTestAsync(int quizGrade);

        public Task<List<TestResult>> GetPreviousTestResultsAsync(int userId);
    }
}