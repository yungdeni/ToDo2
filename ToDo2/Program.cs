using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ToDo2
{
    /// <summary>
    /// Represents an entry in a todolist
    /// </summary>
    class Activity
    {
        string date;
        string status;
        string title;
        public string Status
        {
            set
            {
                if (value == "väntande")
                    status = "v";
                else if (value == "pågående")
                {
                    status = "p";
                }
                else if (value == "avklarad")
                {
                    status = "*";
                }
                else
                {
                    Console.WriteLine("Status kan bara vara väntande, pågående eller avklarad");
                }
            }
            get { return this.status; }
        }
        public Activity(string inputDate, string inputStatus, string inputTitle)
        {
            date = inputDate;
            status = inputStatus;
            title = inputTitle;
        }
        public Activity(string inputDate, string inputTitle)
        {
            date = inputDate;
            status = "v";
            title = inputTitle;
        }
        /// <summary>
        /// Outputs the class fields into a '#' seperated format for saving
        /// </summary>
        /// <returns>A string</returns>
        public override string ToString()
        {
            return String.Format("{0}#{1}#{2}", date, status, title);
        }
        /// <summary>
        /// Prints the class fields date, status and title
        /// </summary>
        public void Print()
        {
            Console.WriteLine("{0,6} {1} {2}", date, status, title);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string command;
            string[] commandWord;
            string currentPath = "";
            List<Activity> todoList = new List<Activity>();
            Console.WriteLine("Welcome to todo");
            // COMMAND PROMPT
            do
            {
                Console.Write(">");
                command = Console.ReadLine();
                commandWord = command.Split(' ');
                //CommandWord[0] is the name of the command
                if (commandWord[0] == "quit")
                {
                    Console.WriteLine("Bye");
                }
                else if (commandWord[0] == "load")
                {
                    LoadTodolist(commandWord, out currentPath, out todoList);
                }
                else if (commandWord[0] == "find")
                {
                    FindFiles();
                }
                else if (commandWord[0] == "save")
                {
                    SaveToFile(commandWord, currentPath, todoList);
                }
                else if (commandWord[0] == "print")
                {
                    PrintTodoList(commandWord, todoList);
                }
                else if (commandWord[0] == "move")
                {
                    MoveActivity(commandWord, todoList);
                }
                else if (commandWord[0] == "delete")
                {
                    DeleteActivity(commandWord, todoList);
                }
                else if (commandWord[0] == "add")
                {
                    AddActivity(commandWord, todoList);
                }
                else if (commandWord[0] == "set")
                {
                    SetStatus(commandWord, todoList);
                }              
            } while (commandWord[0] != "quit");
        }
        /// <summary>
        /// A static method for adding activities to a list
        /// </summary>
        /// <param name="commandWord">stringarray containing the values to be added</param>
        /// <param name="todoList">The list we add Activitys too</param>
        private static void AddActivity(string[] commandWord, List<Activity> todoList)
        {
            if (commandWord.Length < 3)
            {
                Console.WriteLine("Skriv in ett datum");
                string inputDate = Console.ReadLine();
                Console.WriteLine("Skriv in en aktivitet");
                string inputTitle = Console.ReadLine();
                todoList.Add(new Activity(inputDate, inputTitle));
            }
            else
            {
                todoList.Add(new Activity(commandWord[1], commandWord[2]));
            }
        }
        /// <summary>
        /// A static method to remove activities from a list
        /// </summary>
        /// <param name="commandWord">A string array containing the index into the list</param>
        /// <param name="todoList">The list we remove values from</param>
        private static void DeleteActivity(string[] commandWord, List<Activity> todoList)
        {
            try
            {
                int index = int.Parse(commandWord[1]) - 1;
                todoList.RemoveAt(index);
            }
            catch (FormatException)
            {
                Console.WriteLine("Du måste ange ett heltal");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Du försökte ta bort en aktivitet utanför listan");
            }
        }
        /// <summary>
        /// A static method to move activites in the list
        /// </summary>
        /// <param name="commandWord">A string array containing the index and direction</param>
        /// <param name="todoList">The list we move values in</param>
        private static void MoveActivity(string[] commandWord, List<Activity> todoList)
        {
            //index-1 because the array is 0-indexed
            try
            {
                int index = int.Parse(commandWord[1]) - 1;
                Activity temp = todoList[index];
                if (commandWord[2] == "up")
                {
                    //swap the order of the activity with the previous one
                    todoList[index] = todoList[index - 1];
                    todoList[index - 1] = temp;
                }
                else if (commandWord[2] == "down")
                {
                    //swap the order of the activity with the next one
                    todoList[index] = todoList[index + 1];
                    todoList[index + 1] = temp;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Du måste ange ett heltal");

            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Flytten misslyckades, du angav en aktivitet" +
                    " utanför listan eller försökte flytta den utanför");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Flytten misslyckades, du angav en aktivitet" +
                    " utanför listan");
            }
        }
        /// <summary>
        /// A static method to find the path to files ending with .lis in the users home directory
        /// </summary>
        private static void FindFiles()
        {
            //Find all files in the homedirectory with the extension .lis
            try
            {
                string homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string[] dirs = Directory.GetFiles(homeDirectory, "*.lis");
                foreach (string dir in dirs)
                {
                    Console.WriteLine(dir);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// A static method to change the status of an activity
        /// </summary>
        /// <param name="commandWord">A string array containing the index and status to change</param>
        /// <param name="todoList">A list of activities</param>
        private static void SetStatus(string[] commandWord, List<Activity> todoList)
        {
            try
            {
                int index = int.Parse(commandWord[1]) - 1;
                todoList[index].Status = commandWord[2];
            }
            catch (FormatException)
            {
                Console.WriteLine("Ange ett heltal");
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Skriv t.ex set 1 avklarad");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Den aktiviteten finns inte");
            }
        }
        /// <summary>
        /// A static method to load activities from a textfile into a list
        /// </summary>
        /// <param name="commandWord">A string array containing the path</param>
        /// <param name="currentPath">A string that saves the path used for later when saving</param>
        /// <param name="todoList">A list containing activities</param>
        private static void LoadTodolist(string[] commandWord, out string currentPath, out List<Activity> todoList)
        {
            if (commandWord.Length == 1)
            {
                Console.WriteLine("Ange filsökväg");
                currentPath = Console.ReadLine();
            }
            else
            {
                currentPath = commandWord[1];
            }
            List<Activity> temp = new List<Activity>();
            try
            {
                string[] lines = File.ReadAllLines(currentPath);
                foreach (string line in lines)
                {
                    string[] i = line.Split('#');
                    temp.Add(new Activity(i[0], i[1], i[2]));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            todoList = temp;
        }
        /// <summary>
        /// Prints out the activities from a list in a nice format
        /// </summary>
        /// <param name="commandWord">String array containing the arguments for different formats</param>
        /// <param name="todoList">The list containing the activities</param>
        private static void PrintTodoList(string[] commandWord, List<Activity> todoList)
        {
            Console.WriteLine("N  datum  S rubrik");
            Console.WriteLine("-------------------------------------------");
            if (commandWord.Length == 1)
            {
                for (int i = 0; i < todoList.Count; i++)
                {
                    if (todoList[i].Status != "*")
                    {
                        Console.Write(i + 1 + ": ");
                        todoList[i].Print();
                    }
                }
            }
            else if (commandWord[1] == "done")
            {
                for (int i = 0; i < todoList.Count; i++)
                {
                    if (todoList[i].Status == "*")
                    {
                        Console.Write(i + 1 + ": ");
                        todoList[i].Print();
                    }
                }
            }
            else if (commandWord[1] == "all")
            {
                for (int i = 0; i < todoList.Count; i++)
                {
                    Console.Write(i + 1 + ": ");
                    todoList[i].Print();
                }
            }
            Console.WriteLine("-------------------------------------------");
        }
        /// <summary>
        /// A static method that saves a list of activites to a file
        /// </summary>
        /// <param name="commandWord">string array containing userinputed path</param>
        /// <param name="currentPath">The path that the file was loaded from</param>
        /// <param name="todoList">A list containing the activites to be saved</param>
        private static void SaveToFile(string[] commandWord, string currentPath, List<Activity> todoList)
        {
            string[] toSave = new string[todoList.Count()];
            for (int i = 0; i < todoList.Count(); i++)
            {
                toSave[i] = (todoList[i].ToString());
            }
            try
            {
                if (commandWord.Length == 1)
                {
                    File.WriteAllLines(currentPath, toSave);
                }
                else
                {
                    File.WriteAllLines(commandWord[1], toSave);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not save file " + e.Message);
            }
        }
    }
}
