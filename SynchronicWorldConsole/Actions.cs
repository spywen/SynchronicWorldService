using System;
using System.Collections.Generic;

namespace SynchronicWorldConsole
{
    public class Actions
    {
        public List<Tuple<int, string, string>> ActionsList { get; set; }

        public Actions()
        {
            ActionsList = new List<Tuple<int, string, string>>
            {
                new Tuple<int, string, string>(1, "APPLICATION", "Close"),
                new Tuple<int, string, string>(2, "EVENT", "Create"),
                new Tuple<int, string, string>(3, "EVENT", "Get"),
                new Tuple<int, string, string>(4, "EVENT", "Get all"),
                new Tuple<int, string, string>(5, "EVENT", "Update"),
                new Tuple<int, string, string>(6, "EVENT", "Delete"),
                new Tuple<int, string, string>(7, "PERSON", "Create"),
                new Tuple<int, string, string>(8, "PERSON", "Get"),
                new Tuple<int, string, string>(9, "PERSON", "Update"),
                new Tuple<int, string, string>(10, "PERSON", "Delete"),
                new Tuple<int, string, string>(11, "EVENT", "Find events by name"),
                new Tuple<int, string, string>(12, "EVENT", "Find events by date"),
                new Tuple<int, string, string>(13, "EVENT", "Find events between two dates"),
                new Tuple<int, string, string>(14, "EVENT", "Find events by status"),
                new Tuple<int, string, string>(15, "EVENT", "Find events by type"),
                new Tuple<int, string, string>(16, "EVENT", "Delete closed events"),
                new Tuple<int, string, string>(17, "EVENT", "Update all events that have a pending status to open status"),
                new Tuple<int, string, string>(18, "PERSON", "Add person to an open event"),
                new Tuple<int, string, string>(19, "PERSON", "Retrieve all the people link to an open event"),
                new Tuple<int, string, string>(20, "CONTRIBUTION", "Get contribution for an event"),
                new Tuple<int, string, string>(21, "CONTRIBUTION", "Get contributions for a person"),
                new Tuple<int, string, string>(22, "CONTRIBUTION", "Delete contributions for a person for all open events"),
            };
        }
    }
}
