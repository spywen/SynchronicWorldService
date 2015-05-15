using System;

namespace SynchronicWorldConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            var appShouldClosed = false;

            //APPLICATION INFORMATION
            Console.WriteLine("@@@ Synchronic World POC tester @@@\n");
            Console.WriteLine("Enter your choice then press ENTER\n");

            while (!appShouldClosed)
            {
                //Display actions
                var actions = new Actions();
                foreach (var action in actions.ActionsList)
                {
                    Console.WriteLine(String.Format("{0}- {1}@ {2}", action.Item1, action.Item2, action.Item3));
                }

                //Enable user choice
                int choiceId;
                do
                {
                    var choice = Console.ReadLine();
                    Int32.TryParse(choice, out choiceId);

                    if (choiceId == 0 || choiceId > actions.ActionsList.Count)
                    {
                        Console.WriteLine("Please insert your choice... (for example 2, to execute the second action)");
                        choiceId = 0;
                    }
                } while (choiceId == 0);

                if (choiceId != 1)
                {
                    //Execute action
                    var actionExecutor = new ActionExecutor(choiceId);
                    
                    Console.WriteLine("\nPress ENTER to return to the Menu...");
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    appShouldClosed = true;
                }
            }
        }
    }
}
