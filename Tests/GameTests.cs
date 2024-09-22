using System;
using System.IO;
using System.Threading.Tasks;
using Tests.TestDoubles;
using Trivia;
using VerifyXunit;
using Xunit;

namespace Tests;

public class GameTests
{
    [Fact]
    public Task PlaySeveralGamesAndTestOutput()
    {
        var gameOutputs = new string[10];

        for (var seed = 0; seed < 10; seed++)
        {
            var output = PlayGame(seed);
            gameOutputs[seed] = output; 
        }

        return Verifier.Verify(gameOutputs);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(2, true)]
    [InlineData(3, true)]
    public void IsPlayableTest(int numberOfPlayers, bool expected)
    {
        var game = new Game();

        for (int i = 0; i < numberOfPlayers; i++)
        {
            game.Add($"Player {i}");
        }

        Assert.Equal(expected, game.IsPlayable());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void HowManyPlayersTest(int numberOfPlayers)
    {
        var game = new Game();

        for (int i = 0; i < numberOfPlayers; i++)
        {
            game.Add($"Player {i}");
        }

        Assert.Equal(numberOfPlayers, game.HowManyPlayers());
    }

    [Fact]
    public void WasCorrectlyAnswered_ShouldDisplayCorrectMessage_WhenPlayerIsNotInPenaltyBox()
    {
        // Arrange
        var spyDisplay = new SpyDisplay();
        var game = new Game(spyDisplay);

        game.Add("Irrelevant player");

        // Act
        game.WasCorrectlyAnswered();

        // Assert
        Assert.Contains("Answer was correct!", spyDisplay.GetMessages());
    }

    [Fact]
    public void WasCorrectlyAnswered_ShouldDisplayCorrectMessage_WhenPlayerIsInPenaltyBoxAndIsGettingOutOfIt()
    {
        // Arrange
        var spyDisplay = new SpyDisplay();
        var game = new Game(spyDisplay);

        game.Add("Irrelevant player");

        game.Roll(1);
        game.WrongAnswer(); // Player is in penalty box

        game.Roll(1); // Player is getting out of penalty box

        // Act
        game.WasCorrectlyAnswered();

        // Assert
        Assert.Contains("Answer was correct!", spyDisplay.GetMessages());
    }

    private static string PlayGame(int seed)
    {
        var random = new Random(seed);

        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var game = new Game();

        AddPlayers(game, random);
        PlayGameUntilAPlayerWins(game, random);

        return stringWriter.ToString();
    }

    private static void AddPlayers(Game game, Random random)
    {
        int numberOfPlayers = random.Next(5) + 1;

        for (int i = 0; i < numberOfPlayers; i++)
        {
            game.Add($"Player {i}");
        }
    }

    private static void PlayGameUntilAPlayerWins(Game game, Random random)
    {
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
    }
}