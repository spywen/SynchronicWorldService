using System;

namespace SynchronicWorldConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            var eventService = new SynchronicWorldService.EventServiceClient();
            var events = eventService.GetAllEvents();

            foreach (var eventt in events.Result)
            {
                Console.Write(eventt.Name);
            }

            Console.WriteLine("FIN");
            Console.ReadLine();
        }
    }
}
