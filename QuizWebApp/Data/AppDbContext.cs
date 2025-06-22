using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizWebApp.Models;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<UserQuizResult> UserQuizResults { get; set; }
    public DbSet<UserAnswer> UserAnswers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Tymczasowe dane testowe
        modelBuilder.Entity<Quiz>().HasData(
            new Quiz
            {
                Id = 1,
                Title = "General Knowledge",
                Description = "Test your general knowledge",
                UserId = "default-user-id", // Use actual user ID
                CreatedAt = DateTime.Parse("2023-01-01")
            },
        new Quiz
        {
            Id = 2,
            Title = "Science Quiz",
            Description = "Questions about science",
            UserId = "default-user-id",
            CreatedAt = DateTime.Parse("2023-01-02")
        }
        );

        {
            // KLUCZOWE: wywołanie bazowej implementacji dla Identity
            base.OnModelCreating(modelBuilder);


            // Twoja konfiguracja dla UserAnswer
            modelBuilder.Entity<UserAnswer>(entity =>
            {
                entity.HasOne(ua => ua.Question)
                      .WithMany()
                      .HasForeignKey(ua => ua.QuestionId)
                      .OnDelete(DeleteBehavior.NoAction);

                // Dodaj konfigurację dla pozostałych relacji
                entity.HasOne(ua => ua.UserQuizResult)
                      .WithMany()
                      .HasForeignKey(ua => ua.UserQuizResultId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(ua => ua.SelectedAnswer)
                      .WithMany()
                      .HasForeignKey(ua => ua.SelectedAnswerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Dodatkowe konfiguracje dla innych modeli
            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasOne(q => q.Quiz)
                      .WithMany(q => q.Questions)
                      .HasForeignKey(q => q.QuizId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.HasOne(a => a.Question)
                      .WithMany(q => q.Answers)
                      .HasForeignKey(a => a.QuestionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}