using System.ComponentModel.DataAnnotations.Schema;

namespace QuizWebApp.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string UserId { get; set; }  // Powiązanie z użytkownikiem

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<UserQuizResult> Results { get; set; } = new List<UserQuizResult>();
        [NotMapped]
        public string CreatorId => UserId;
    }
}
