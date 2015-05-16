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
            var actions = new Actions().ActionsList;
            var infoService = new SynchronicWorldService.InfoServiceClient();
            ConsoleWriter.WriteWithColor(infoService.GetDatabaseStatus().Result, ConsoleColor.Yellow);

            ConsoleWriter.WriteWithColor("Execute WCF Service method...", ConsoleColor.Gray);
            var currentAction = actions.Where(x => x.Item1 == choiceId).First();
            ConsoleWriter.WriteWithColor(String.Format("{0} - {1}", currentAction.Item2, currentAction.Item3), ConsoleColor.Green);

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
                case 11:
                    FindByName();
                    break;
                case 12:
                    FindByDate();
                    break;
                case 13:
                    FindByDates();
                    break;
                case 14:
                    FindByStatus();
                    break;
                case 15:
                    FindByType();
                    break;
                case 16:
                    DeleteClosedEvent();
                    break;
                case 17:
                    UpdatePendingEventsAsOpen();
                    break;
                case 18:
                    AddPersonToAnOpenEvent();
                    break;
                case 19:
                    FindPeopleLinkToOpenEvent();
                    break;
                case 20:
                    GetContributionsForOpenEvent();
                    break;
                case 21:
                    GetPersonContributions();
                    break;
                case 22:
                    DeleteContributions();
                    break;
            }

            ConsoleWriter.WriteWithColor(infoService.GetDatabaseStatus().Result, ConsoleColor.Yellow);
        }

        /// <summary>
        /// Analyse report and display it into the console
        /// </summary>
        /// <param name="report"></param>
        private bool AnalyseReport(SynchronicWorldService.Report report)
        {
            report.ErrorList.ForEach(ConsoleWriter.LogError);
            report.InfoList.ForEach(ConsoleWriter.LogInfo);
            return report.ErrorList.Count == 0;
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
            if (AnalyseReport(response.Report))
            {
                Console.WriteLine("Id : {0}, Name : {1}, Status : {2}, Type : {3}", response.Result.Id, response.Result.Name, response.Result.Status.Value, response.Result.Type.Value);
            }
        }

        /// <summary>
        /// 4 - Get all events
        /// </summary>
        private void GetAllEvents()
        {
            var eventService = new SynchronicWorldService.EventServiceClient();
            var response = eventService.GetAllEvents();
            if (AnalyseReport(response.Report))
            {
                foreach (var eventt in response.Result)
                {
                    Console.WriteLine("Id : {0}, Name : {1}, Status : {2}, Type : {3}", eventt.Id, eventt.Name, eventt.Status.Value, eventt.Type.Value);
                }
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

            if (AnalyseReport(response.Report))
            {
                Console.WriteLine("UPDATED EVENT -> Id : {0}, Name : {1}, Status : {2}, Type : {3}", response.Result.Id, response.Result.Name, response.Result.Status.Value, response.Result.Type.Value);
            }
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

            if (AnalyseReport(response.Report))
            {
                Console.WriteLine("Id : {0}, Name : {1}, Nickname : {2}", response.Result.Id, response.Result.Name, response.Result.Nickname);
            }
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
            if (AnalyseReport(response.Report))
            {
                Console.WriteLine("Id : {0}, Name : {1}, Nickname : {2}", response.Result.Id, response.Result.Name, response.Result.Nickname);
            }
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

        #region find events
        public void FindByName()
        {
            var eventService = new SynchronicWorldService.EventServiceClient();
            var searchObj = new SynchronicWorldService.EventSearchFacade();
            ConsoleWriter.WriteWithColor("Please enter an event name to search ??", ConsoleColor.Gray);
            searchObj.Name = Console.ReadLine();

            var response = eventService.SearchForEvents(searchObj);
            if (AnalyseReport(response.Report))
            {
                foreach (var eventt in response.Result)
                {
                    Console.WriteLine("Id : {0}, Name : {1}, Status : {2}, Type : {3}", eventt.Id, eventt.Name, eventt.Status.Value, eventt.Type.Value);
                }
            }
        }

        public void FindByDate()
        {
            var eventService = new SynchronicWorldService.EventServiceClient();
            var searchObj = new SynchronicWorldService.EventSearchFacade();
            searchObj.Date = new DateTime(2015,12,6);
            Console.WriteLine("Search events by date : 06-12-2015");

            var response = eventService.SearchForEvents(searchObj);
            if (AnalyseReport(response.Report))
            {
                foreach (var eventt in response.Result)
                {
                    Console.WriteLine("Id : {0}, Name : {1}, Status : {2}, Type : {3}", eventt.Id, eventt.Name, eventt.Status.Value, eventt.Type.Value);
                }
            }
        }

        public void FindByDates()
        {
            var eventService = new SynchronicWorldService.EventServiceClient();
            var searchObj = new SynchronicWorldService.EventSearchFacade();
            searchObj.StartDate = new DateTime(2015, 1, 1);
            searchObj.EndDate = new DateTime(2017, 1, 1);
            Console.WriteLine("Search events between two dates : 01-01-2015 - 01-01-2017");

            var response = eventService.SearchForEvents(searchObj);
            if (AnalyseReport(response.Report))
            {
                foreach (var eventt in response.Result)
                {
                    Console.WriteLine("Id : {0}, Name : {1}, Status : {2}, Type : {3}", eventt.Id, eventt.Name, eventt.Status.Value, eventt.Type.Value);
                }
            }
        }

        public void FindByStatus()
        {
            var eventService = new SynchronicWorldService.EventServiceClient();
            var searchObj = new SynchronicWorldService.EventSearchFacade();

            searchObj.EventStatusCode = SynchronicWorldService.EventStatusCode.Open;
            Console.WriteLine("Search events by status : Open");

            var response = eventService.SearchForEvents(searchObj);
            if (AnalyseReport(response.Report))
            {
                foreach (var eventt in response.Result)
                {
                    Console.WriteLine("Id : {0}, Name : {1}, Status : {2}, Type : {3}", eventt.Id, eventt.Name, eventt.Status.Value, eventt.Type.Value);
                }
            }
        }

        public void FindByType()
        {
            var eventService = new SynchronicWorldService.EventServiceClient();
            var searchObj = new SynchronicWorldService.EventSearchFacade();

            searchObj.EventTypeCode = SynchronicWorldService.EventTypeCode.Party;
            Console.WriteLine("Search events by type : Party");

            var response = eventService.SearchForEvents(searchObj);
            if (AnalyseReport(response.Report))
            {
                foreach (var eventt in response.Result)
                {
                    Console.WriteLine("Id : {0}, Name : {1}, Status : {2}, Type : {3}", eventt.Id, eventt.Name, eventt.Status.Value, eventt.Type.Value);
                }
            }
        }
        #endregion

        #region event++
        private void DeleteClosedEvent()
        {
            var eventService = new SynchronicWorldService.EventServiceClient();
            var response = eventService.DeleteClosedEvents();
            AnalyseReport(response.Report);
        }

        private void UpdatePendingEventsAsOpen()
        {
            var eventService = new SynchronicWorldService.EventServiceClient();
            var response = eventService.UpgradePendingEventsAsOpen();
            AnalyseReport(response.Report);
        }
        #endregion

        #region person++
        private void AddPersonToAnOpenEvent()
        {
            var personService = new SynchronicWorldService.PersonServiceClient();
            Console.WriteLine("Add person with id 2 to event with id 2");
            var response = personService.SuscribeUserToAnOpenEvent(2, 2);
            AnalyseReport(response.Report);
        }
        private void FindPeopleLinkToOpenEvent()
        {
            var personService = new SynchronicWorldService.PersonServiceClient();
            Console.WriteLine("Find people link to the event 2");
            var response = personService.FindPeopleLinkToOpenEvent(2);
            if (AnalyseReport(response.Report))
            {
                foreach(var person in response.Result){
                    Console.WriteLine("Id : {0}, Name : {1}, Nickname : {2}", person.Id, person.Name, person.Nickname);
                }
            }
        }
        #endregion

        #region contribution
        private void GetContributionsForOpenEvent()
        {
            var contribService = new SynchronicWorldService.ContributionServiceClient();
            Console.WriteLine("Get contributions of the event 2");
            var response = contribService.GetEventContributions(2);
            if (AnalyseReport(response.Report))
            {
                foreach (var contrib in response.Result)
                {
                    Console.WriteLine("Id : {0}, Name : {1}, Quantity : {2}, Type : {3}", contrib.Id, contrib.Name, contrib.Quantity, contrib.Type.Value);
                }
            }
        }

        private void GetPersonContributions()
        {
            var contribService = new SynchronicWorldService.ContributionServiceClient();
            Console.WriteLine("Get contributions of the person 1");
            var response = contribService.GetPersonContributions(1);
            if (AnalyseReport(response.Report))
            {
                foreach (var contrib in response.Result)
                {
                    Console.WriteLine("Id : {0}, Name : {1}, Quantity : {2}, Type : {3}", contrib.Id, contrib.Name, contrib.Quantity, contrib.Type.Value);
                }
            }
        }

        private void DeleteContributions()
        {
            var contribService = new SynchronicWorldService.ContributionServiceClient();
            Console.WriteLine("Delete contributions of the person 2");
            var response = contribService.DeleteAllPersonContributionsForOpenEvents(1);
            AnalyseReport(response.Report);
        }
        #endregion
    }
}
