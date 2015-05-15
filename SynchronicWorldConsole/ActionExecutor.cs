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
                case 3:
                    GetEvent();
                    break;
                case 4:
                    GetAllEvents();
                    break;
                case 5:
                    UpdateEvent();
                    break;
                case 6:
                    DeleteEvent();
                    break;
                case 7:
                    CreatePerson();
                    break;
                case 8:
                    GetPerson();
                    break;
                case 9:
                    UpdatePerson();
                    break;
                case 10:
                    DeletePerson();
                    break;

            }

            ConsoleWriter.WriteWithColor(infoService.GetDatabaseStatus().Result, ConsoleColor.Yellow);
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

        #region events
        /// <summary>
        /// 2 - Create event
        /// </summary>
        private void CreateEvent()
        {
            var enumsService = new SynchronicWorldService.EnumsServiceClient();
            var eventTypes = enumsService.GetAllEventsType().Result;
            var eventStatus = enumsService.GetAllEventsStatus().Result;

            var newEvent = new Event
            {
                Name = "Event created by console POC",
                Address = "new address",
                Date = DateTime.Now.AddMonths(1),
                Description = "new event",
                Type = eventTypes.First(),
                Status = eventStatus.First()
            };
            var eventService = new SynchronicWorldService.EventServiceClient();
            var response = eventService.CreateEvent(newEvent);
            AnalyseReport(response.Report);
        }

        /// <summary>
        /// 3 - Get event
        /// </summary>
        private void GetEvent()
        {
            var eventService = new SynchronicWorldService.EventServiceClient();
            var allEventsResponse = eventService.GetAllEvents();
            var firstEventId = allEventsResponse.Result.First().Id;
            var response = eventService.GetEvent(firstEventId);
            AnalyseReport(response.Report);

            Console.WriteLine("Id : {0}, Name : {1}, Status : {2}, Type : {3}", response.Result.Id, response.Result.Name, response.Result.Status.Value, response.Result.Type.Value);
        }

        /// <summary>
        /// 4 - Get all events
        /// </summary>
        private void GetAllEvents()
        {
            var eventService = new SynchronicWorldService.EventServiceClient();
            var response = eventService.GetAllEvents();
            AnalyseReport(response.Report);

            foreach (var eventt in response.Result)
            {
                Console.WriteLine("Id : {0}, Name : {1}, Status : {2}, Type : {3}", eventt.Id, eventt.Name, eventt.Status.Value, eventt.Type.Value);
            }
        }

        /// <summary>
        /// 5 - Update event
        /// </summary>
        private void UpdateEvent()
        {
            var eventService = new SynchronicWorldService.EventServiceClient();
            var allEventsResponse = eventService.GetAllEvents();
            var firstEventId = allEventsResponse.Result.First().Id;
            var eventToUpdate = eventService.GetEvent(firstEventId).Result;

            Console.WriteLine("OLD EVENT (before update name): Id : {0}, Name : {1}, Status : {2}, Type : {3}", eventToUpdate.Id, eventToUpdate.Name, eventToUpdate.Status.Value, eventToUpdate.Type.Value);

            eventToUpdate.Name = eventToUpdate.Name + " Updated ";

            var response = eventService.UpdateEvent(eventToUpdate);

            Console.WriteLine("UPDATED EVENT -> Id : {0}, Name : {1}, Status : {2}, Type : {3}", response.Result.Id, response.Result.Name, response.Result.Status.Value, response.Result.Type.Value);
            AnalyseReport(response.Report);
        }

        /// <summary>
        /// 6 - Delete event
        /// </summary>
        private void DeleteEvent()
        {
            var eventService = new SynchronicWorldService.EventServiceClient();
            var allEventsResponse = eventService.GetAllEvents();

            var response = eventService.DeleteEvent(allEventsResponse.Result.First().Id);
            AnalyseReport(response.Report);
        }
        #endregion

        #region person
        /// <summary>
        /// 7 - Create person
        /// </summary>
        private void CreatePerson()
        {
            var newPerson = new Person
            {
                Name = "Edouardo",
                Nickname = "Edouard"
            };
            var personService = new SynchronicWorldService.PersonServiceClient();
            var response = personService.CreatePerson(newPerson);
            AnalyseReport(response.Report);
        }

        /// <summary>
        /// 8 - Delete person
        /// </summary>
        private void GetPerson()
        {
            var personService = new SynchronicWorldService.PersonServiceClient();
            var response = personService.GetPerson(1);

            Console.WriteLine("Id : {0}, Name : {1}, Nickname : {2}", response.Result.Id, response.Result.Name, response.Result.Nickname);
            AnalyseReport(response.Report);
        }

        /// <summary>
        /// 9 - Update person
        /// </summary>
        private void UpdatePerson()
        {
            var personService = new SynchronicWorldService.PersonServiceClient();
            var personToUpdate = personService.GetPerson(1).Result;

            Console.WriteLine("PERSON BEFORE UPDATE -> Id : {0}, Name : {1}, Nickname : {2}", personToUpdate.Id, personToUpdate.Name, personToUpdate.Nickname);

            personToUpdate.Name = personToUpdate.Name + " UPDATED ";

            var response = personService.UpdatePerson(personToUpdate);
            Console.WriteLine("Id : {0}, Name : {1}, Nickname : {2}", response.Result.Id, response.Result.Name, response.Result.Nickname);
            AnalyseReport(response.Report);
        }

        /// <summary>
        /// 10- Delete person
        /// </summary>
        private void DeletePerson()
        {
            var personService = new SynchronicWorldService.PersonServiceClient();
            var response = personService.DeletePerson(2);
            AnalyseReport(response.Report);
        }
        #endregion

        
    }
}
