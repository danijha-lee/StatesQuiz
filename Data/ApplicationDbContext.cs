using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StatesQuiz.Models;

namespace StatesQuiz.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)

        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<States> States { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
    }
}