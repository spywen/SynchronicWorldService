using System;
using SynchronicWorldConsole.SynchronicWorldService;

namespace SynchronicWorldConsole
{
    public class ActionExecutor
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="choiceId"></param>
        public ActionExecutor(int choiceId)
        {
            var infoService = new SynchronicWorldService.InfoServiceClient();
            ConsoleWriter.WriteWithColor(infoService.GetDatabaseStatus().Result, ConsoleColor.Yellow);

            switch (choiceId)
            {
                case 2:
                    var eventService = new SynchronicWorldService.EventServiceClient();
                    var newEvent = new Event
                    {
                        Name = "Event created by console POC",
                        Address = "new address",
                        Date = DateTime.Now.AddMonths(1),
                        Description = "new event",
                        
                    };
                    //eventService.CreateEvent(newEvent);
                    break;
            }

            ConsoleWriter.WriteWithColor(infoService.GetDatabaseStatus().Result, ConsoleColor.Yellow);
        }
    }
}
