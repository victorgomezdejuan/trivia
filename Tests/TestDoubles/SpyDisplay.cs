using System;
using System.Collections.Generic;
using Trivia;

namespace Tests.TestDoubles;

public class SpyDisplay : IDisplay
{
    private readonly List<string> _messages = new();

    public void Show(string message) => _messages.Add(message);

    public string GetMessages() => string.Join(Environment.NewLine, _messages);
}