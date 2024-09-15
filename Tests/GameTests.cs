using System;
using System.IO;
using Trivia;
using Xunit;

namespace Tests;

public class GameTests
{
    [Fact]
    public void GameWithTwoPlayers()
    {
        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var game = new Game();

        game.Add("Victor");
        game.Add("Tao");

        var random = new Random();
        bool hasWon;

        do
        {
            game.Roll(random.Next(5) + 1);

            if (random.Next(9) == 7)
            {
                hasWon = game.WrongAnswer();
            }
            else
            {
                hasWon = game.WasCorrectlyAnswered();
            }
        } while (hasWon);

        File.AppendAllText(@"C:\Training\Snapshot Testing\Tests\output.txt", stringWriter.ToString());
    }
}