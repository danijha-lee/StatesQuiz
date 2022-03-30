using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StatesQuiz.Data;
using StatesQuiz.Models;
using StatesQuiz.Services.Interfaces;
using StatesQuiz.Models.ViewModel;

namespace StatesQuiz.Services
{
    public class SQQuizService : ISQQuizService
    {
        private readonly ApplicationDbContext _context;
        private static string value;

        public SQQuizService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<States>> GetStatesAsync()
        {
            try
            {
                List<States> states = await _context.States
                     .Include(s => s.Capital)
                     .Include(s => s.StateId)
                     .ToListAsync();
                return states;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> RegisterUserAsync(string UserName, string Password, string FirstName, string Email)
        {
            User user = new User();
            try
            {
                User newUser = new User();
                newUser.UserName = UserName;
                newUser.Password = Password;
                //newUser.FirstName = FirstName;
                //newUser.Email = Email;
            }
            catch (Exception)
            {
                throw;
            }
            return user;
        }

        public async Task<User> SearchUserNameAsync(string UserName)
        {
            try
            {
                User result = await _context.User.Where(u => u.UserName.ToLower() == UserName.ToLower()).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<States>> GetQuizQuestionsAsync()
        {
            try
            {
                List<States> Allstates = await _context.States.Take(50).ToListAsync();
                List<States> states = new List<States>();
                for (int i = 0; i < 10; i++)
                {
                    Random random = new Random();
                    int number = random.Next(0, 50);
                    if (states.Contains(Allstates[number]))
                    {
                        i--;
                    }
                    else
                    {
                        states.Add(Allstates[number]);
                    }
                }

                return states;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CheckUserInput(States answer)
        {
            var result = false;

            if (answer != null)
            {
                string response = answer.Capital.ToLower();
                States State = await _context.States.Where(s => s.StateId == answer.StateId).FirstOrDefaultAsync();
                if (State.Capital.ToLower() == response)
                {
                    result = true;
                }
            }
            return result;
        }

        public async Task<TestResult> GradeTestAsync(int quizGrade)
        {
            TestResult results = new TestResult();
            results.NumberCorrect = (quizGrade / 10);
            results.TotalQuestions = 10;
            results.TestDateTime = DateTime.Now;
            results.UserId = 1;

            await _context.AddAsync(results);
            await _context.SaveChangesAsync();
            return results;
        }

        public async Task<List<TestResult>> GetPreviousTestResultsAsync(int userId)
        {
            List<TestResult> results = await _context.TestResults.Where(t => t.UserId == userId).ToListAsync();
            return results;
        }
    }
}