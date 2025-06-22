namespace QuizWebApp.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public string Content { get; set; }
        public int Points { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
