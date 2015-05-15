using System;
using System.Linq;
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

            ConsoleWriter.WriteWithColor("Execute WCF Service method...", ConsoleColor.Gray);
            switch (choiceId)
            {
                case 2:
                    CreateEvent();
                    break;
            }

            ConsoleWriter.WriteWithColor(infoService.GetDatabaseStatus().Result, ConsoleColor.Yellow);
        }

        /// <summary>
        /// 2 - Create event
        /// </summary>
        private void CreateEvent()
        {
            var enumsService = new SynchronicWorldService.EnumsServiceClient();
            var eventTypes = enumsService.GetAllEventsType().Result;
            var eventStatus = enumsService.GetAllEventsStatus().Result;

            var eventService = new SynchronicWorldService.EventServiceClient();
            var newEvent = new Event
            {
                Name = "Event created by console POC",
                Address = "new address",
                Date = DateTime.Now.AddMonths(1),
                Description = "new event",
                Type = eventTypes.First(),
                Status = eventStatus.First()
            };
            var response = eventService.CreateEvent(newEvent);
            AnalyseReport(response.Report);
        }

        /// <summary>
        /// Analyse report and display it into the console
        /// </summary>
        /// <param name="report"></param>
        private void AnalyseReport(SynchronicWorldService.Report report)
        {
            report.ErrorList.ForEach(ConsoleWriter.LogError);
            report.ErrorList.ForEach(ConsoleWriter.LogInfo);
        }
    }
}
