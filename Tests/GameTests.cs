using System;
using System.IO;
using System.Threading.Tasks;
using Trivia;
using VerifyXunit;
using Xunit;

namespace Tests;

public class GameTests
{
    [Fact]
    public Task GameWithTwoPlayers()
    {
        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var game = new Game();

        game.Add("Victor");
        game.Add("Tao");

        var random = new Random(123);
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

        return Verifier.Verify(stringWriter.ToString());
    }
}