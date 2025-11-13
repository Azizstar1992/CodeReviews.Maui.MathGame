using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;

class HighScore
{
    public string Name { get; set; }
    public int Score { get; set; }
}

class MathGame
{
    private static Random rand = new Random();
    private static int lives = 3;
    private static int score = 0;

    private static int low = 1;
    private static int high = 10;
    private static string filePath = "highscores.json";

    // sleep function
    public static void sleep(int time)
    {
        System.Threading.Thread.Sleep(time);
    }
    public static void CheckAnswer(bool success, int answer, int solution)
    {
        if (!success || answer != solution)
        {
            lives--;
            Console.WriteLine("❌ You got it wrong!");
        }
        else
        {
            score++;
            Console.WriteLine($"✅ Correct! The answer was {solution}");
        }
    }

    // displays highscores
    public static void ShowHighScores()
    {
        string filePath = "highscores.json";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("No high scores yet. Play a game to set the first one!");
            return;
        }

        string json = File.ReadAllText(filePath);
        var scores = JsonSerializer.Deserialize<List<HighScore>>(json) ?? new List<HighScore>();

        Console.WriteLine("\n🏆 Top 10 High Scores:");
        foreach (var s in scores.OrderByDescending(s => s.Score).Take(10))
            Console.WriteLine($"{s.Name}: {s.Score}");
    }

    public static int Selected(string ope)
    {
        int first = rand.Next(low, high);
        int second = rand.Next(low, high);

        switch (ope)
        {
            case "/":
                Console.WriteLine($"What is {first * second} / {second}?");
                return first;
            case "*":
                Console.WriteLine($"What is {first} * {second}?");
                return first * second;
            case "+":
                Console.WriteLine($"What is {first} + {second}?");
                return first + second;
            case "-":
                Console.WriteLine($"What is {first} - {second}?");
                return first - second;
            default:
                return 0;
        }
    }

    public static void AddHighScore(string playerName, int playerScore)
    {
        List<HighScore> scores = File.Exists(filePath)
            ? JsonSerializer.Deserialize<List<HighScore>>(File.ReadAllText(filePath)) ?? new List<HighScore>()
            : new List<HighScore>();

        scores.Add(new HighScore { Name = playerName, Score = playerScore });

        scores = scores.OrderByDescending(s => s.Score).Take(10).ToList();

        File.WriteAllText(filePath, JsonSerializer.Serialize(scores, new JsonSerializerOptions { WriteIndented = true }));

        Console.WriteLine("\n🏆 High Scores:");
        foreach (var s in scores)
            Console.WriteLine($"{s.Name}: {s.Score}");
    }

    public static void playgame()
    {
        bool playAgain = true;
        // updated play again with a while loop
        while (playAgain)
        {
            // Reset lives and score for new game
            lives = 3;
            score = 0;

            InitializeGame();

            Console.Write("\nDo you want to play again? (y/n): ");
            string input = Console.ReadLine()?.ToLower();
            playAgain = input == "y";
            Console.Clear();
        }
    }


    //menu function'
    public static string ShowMenu()
    {
        while (true)
        {
            Console.WriteLine("Select an operation:");
            Console.WriteLine("1. Add (+)");
            Console.WriteLine("2. Subtract (-)");
            Console.WriteLine("3. Multiply (*)");
            Console.WriteLine("4. Divide (/)");
            Console.Write("Enter your choice (1-4): ");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1": return "+";
                case "2": return "-";
                case "3": return "*";
                case "4": return "/";
                default:
                    Console.WriteLine("Invalid choice. Please try again.\n");
                    break;
            }
        }
    }

    // entry point for game 
    public static void InitializeGame()
    {
        Console.Clear();
        Console.WriteLine("Welcome to the Math Game! 🧮");
        Console.WriteLine("Your goal is to answer as many math questions correctly as possible.\n");

        while (lives > 0)
        {
            //increase difficulity
            if(score > 10 && score %10 == 0)
            {
                low+=10;
                high+=10;
            }
            try
            {
                Console.WriteLine($"❤️ Lives remaining: {lives}");
                Console.WriteLine($"🏆 Current score: {score}\n");


                int solution = Selected(ShowMenu());

                string input = Console.ReadLine();
                bool success = int.TryParse(input, out int answer);
                CheckAnswer(success, answer, solution);


                sleep(2000);
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ An error occurred: {ex.Message}");
                Console.WriteLine("Restarting question...\n");
                sleep(1500);
                Console.Clear();
            }
        }

        Console.Clear();
        Console.WriteLine($"Game over! Your final score was {score} 🎯");


        Console.Write("Enter your name for the leaderboard: ");
        string playerName = Console.ReadLine() ?? "Anonymous";
        AddHighScore(playerName, score);
    }

    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Math Game! 🧮");
            Console.WriteLine("1. Show High Scores");
            Console.WriteLine("2. Play the Game");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice (1-3): ");

            string choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    ShowHighScores();
                    Console.Write("\nDo you want to play the game now? (y/n): ");
                    string playAfterScores = Console.ReadLine()?.ToLower();
                    if (playAfterScores == "y")
                    {
                        MathGame.playgame();
                    }
                    else
                    {
                        Console.WriteLine("Exiting. Thanks for visiting! 👋");
                        return;
                    }
                    break;

                case "2":
                    MathGame.playgame();
                    break;

                case "3":
                    Console.WriteLine("Exiting. Thanks for playing! 👋");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Press any key to try again...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}