# QuizWebApp
**Tytuł projektu:**
Quiz Web App

**Autor projektu:**
Jakub Matuszak Nr albumu: 138318

**Krótki opis projektu:**
Aplikacja internetowa typu quiz online pozwalająca zarejestrowanym użytkownikom na tworzenie, rozwiązywanie i przeglądanie wyników quizów. System wspiera przechowywanie danych o pytaniach, odpowiedziach i wynikach użytkowników.

**Specyfikacja technologiczna:**

* .NET 8 (ASP.NET Core MVC)
* Entity Framework Core 8
* Baza danych: SQLite / SQL Server
* System tożsamości: ASP.NET Core Identity
* Razor Views (MVC)

**Instrukcje uruchomienia projektu:**

1. Przywróć pakiety NuGet.
2. Wykonaj migracje bazy danych:

   ```bash
   dotnet ef database update
   ```
3. Uruchom projekt:

   ```bash
   dotnet run
   ```
4. Aplikacja będzie dostępna pod `https://localhost:{port}`

**Struktura projektu:**

* Models/ - klasy modeli danych (Quiz, Question, Answer, UserQuizResult itd.)
* Data/ - kontekst bazy danych AppDbContext
* Controllers/ - logika aplikacji (AccountController, QuizController, SolveController)
* Views/ - widoki Razor
* wwwroot/ - zasoby statyczne

**Modele:**

1. **ApplicationUser** - rozszerzenie IdentityUser, reprezentuje zarejestrowanego użytkownika.

   * Email, Password - walidowane przez Identity
   * Quizzes - quizy utworzone przez użytkownika
   * Results - zapisane wyniki quizów

2. **Quiz** - reprezentuje quiz.

   * Id (int)
   * Title (string) - tytuł quizu, wymagane
   * Description (string) - opis quizu
   * CreatorId (string) - identyfikator twórcy (FK)
   * Questions (ICollection<Question>)
   * Results (ICollection<UserQuizResult>)

3. **Question** - pytanie w quizie.

   * Id (int)
   * QuizId (int) - FK
   * Content (string) - treść pytania, wymagane
   * Points (int) - liczba punktów za poprawną odpowiedź
   * Answers (ICollection<Answer>)

4. **Answer** - odpowiedź do pytania.

   * Id (int)
   * QuestionId (int) - FK
   * Content (string) - treść odpowiedzi, wymagane
   * IsCorrect (bool) - czy odpowiedź jest poprawna

5. **UserQuizResult** - wynik quizu danego użytkownika.

   * Id (int)
   * QuizId (int)
   * UserId (string)
   * Score (int)
   * CompletedAt (DateTime)
   * Answers (ICollection<UserAnswer>)

6. **UserAnswer** - odpowiedzi użytkownika w danym quizie.

   * Id (int)
   * UserQuizResultId (int)
   * QuestionId (int)
   * SelectedAnswerId (int)

**Kontrolery:**

1. **AccountController**

   * Register (GET, POST): tworzenie konta użytkownika
   * Login (GET, POST): logowanie użytkownika
   * Logout (POST): wylogowanie użytkownika

2. **QuizController**

   * MyQuizzes (GET): lista quizów użytkownika
   * Create (GET, POST): formularz i zapis nowego quizu
   * Edit/Delete (GET, POST): edycja i usuwanie quizów
   * Details (GET): szczegóły quizu

3. **SolveController**

   * Index (GET): rozpoczęcie rozwiązywania quizu
   * Submit (POST): przesłanie wyników
   * Result (GET): wyświetlenie wyniku użytkownika

**Opis systemu użytkowników:**

* Użytkownicy mogą się rejestrować i logować.
* Role: domyślna rola to użytkownik. Konto z emailem w domenie `@admin.com` otrzymuje rolę `Admin`.
* Zalogowani mogą tworzyć quizy, przeglądać swoje quizy i rozwiązywać cudze.
* Goście mają tylko dostęp do strony głównej i formularzy logowania/rejestracji.
* Każdy quiz powiązany jest z twórcą (ApplicationUser).
* Wyniki quizu są zapisywane dla danego użytkownika.

**Najciekawsze funkcjonalności:**

* **System punktowy** - każde pytanie może mieć inną wagę punktową.
* **Automatyczne ocenianie odpowiedzi** i zapis wyniku w bazie danych.
* **Dynamiczne quizy** - użytkownicy mogą samodzielnie tworzyć własne quizy i pytania bez potrzeby edycji kodu.
* **Zapis i przegląd wyników** - możliwość analizy rozwiązanych quizów i ich wyników przez użytkowników.
