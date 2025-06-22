namespace QuizWebApp.Models
{
    public class UserQuizResult
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int Score { get; set; }
        public DateTime CompletedAt { get; set; }
        public ICollection<UserAnswer> Answers { get; set; } = new List<UserAnswer>();
    }
}

