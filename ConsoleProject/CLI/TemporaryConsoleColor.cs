using System;

namespace ConsoleProject.CLI
{
    public class TemporaryConsoleColor : IDisposable
    {
        private readonly ConsoleColor _color;

        public TemporaryConsoleColor(ConsoleColor newColor)
        {
            _color = Console.ForegroundColor;
            Console.ForegroundColor = newColor;
        }

        public static implicit operator TemporaryConsoleColor(ConsoleColor color) => new(color);

        public void Dispose()
        {
            Console.ForegroundColor = _color;
        }
    }
}
