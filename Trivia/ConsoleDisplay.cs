using System;

namespace Trivia;

public class ConsoleDisplay : IDisplay
{
    public void Show(string message)
    {
        Console.WriteLine(message);
    }
}