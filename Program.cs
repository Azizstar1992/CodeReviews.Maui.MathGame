using System;
using System.Collections;
using System.Threading.Tasks;

class MathGame
{
    private static Random rand = new Random();
    private static int lives = 3;
    private static int score = 0;
    public static void checkAnswer(bool success, int answer, int solution)
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

    public static int selected(string ope)
    {

        int first = rand.Next(1, 10);
        int second = rand.Next(1, 10);
        switch (ope)
        {
            case "/":
                Console.WriteLine($"what is {first * second} / {second}");
                return first;
            case "*":
                Console.WriteLine($"what is {first} * {second}");
                return first * second;
            case "+":
                Console.WriteLine($"what is {first} + {second}");
                return first + second;
            case "-":
                Console.WriteLine($"What is {first} - {second}?");
                return first - second;
            default:
                return 0;
        }
    }
    public static async Task InitializeGame()
    {
        Console.Clear();
        Console.WriteLine("Welcome to the Math Game! 🧮");
        Console.WriteLine("Your goal is to answer as many math questions correctly as possible.\n");

        while (lives > 0)
        {
            try
            {
                Console.WriteLine($"❤️ Lives remaining: {lives}");
                Console.WriteLine($"🏆 Current score: {score}\n");

                string[] operators = { "/", "*", "-", "+" };
                int value = rand.Next(operators.Length);
                int solution = selected(operators[value]);

                string input = Console.ReadLine();
                bool success = int.TryParse(input, out int answer);
                checkAnswer(success, answer, solution);

                await Task.Delay(2000);
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ An error occurred: {ex.Message}");
                Console.WriteLine("Restarting question...\n");
                await Task.Delay(1500);
                Console.Clear();
            }
        }

        Console.Clear();
        Console.WriteLine($"Game over! Your final score was {score} 🎯");
    }

    static async Task Main()
    {
        await InitializeGame();
    }
}
