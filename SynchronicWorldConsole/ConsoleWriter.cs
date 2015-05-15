using System;

namespace SynchronicWorldConsole
{
    public static class ConsoleWriter
    {
        public static void WriteWithColor(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
