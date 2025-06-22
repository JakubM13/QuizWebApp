using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace QuizWebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Quiz> Quizzes { get; set; }
        public ICollection<UserQuizResult> Results { get; set; }
    }
}
