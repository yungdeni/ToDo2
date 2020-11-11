using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ToDo2
{
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
        public override string ToString()
        {
            return String.Format("{0}#{1}#{2}", date, status, title);
        }
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
            do
            {
                
                Console.Write(">");
                command = Console.ReadLine();
                commandWord = command.Split(' ');
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
                    string[] dirs = Directory.GetFiles(@"c:\users\frage", "*.lis");
                    foreach (string dir in dirs)
                    {
                        Console.WriteLine(dir);
                    }
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
                    int index = int.Parse(commandWord[1]) - 1;
                    Activity temp = todoList[index];
                    if (commandWord[2] == "up")
                    {
                        todoList[index] = todoList[index - 1];
                        todoList[index - 1] = temp;
                    }
                    else if (commandWord[2] == "down")
                    {
                        todoList[index] = todoList[index + 1];
                        todoList[index + 1] = temp;
                    }
                }
                else if (commandWord[0] == "delete")
                {
                    int index = int.Parse(commandWord[1]) - 1;
                    todoList.RemoveAt(index);
                }
                else if (commandWord[0] == "add")
                {
                    todoList.Add(new Activity(commandWord[1], commandWord[2]));
                }
                else if (commandWord[0] == "set")
                {
                    int index = int.Parse(commandWord[1]) - 1;
                    todoList[index].Status = commandWord[2];
                }
            } while (commandWord[0] != "quit");
        }

        private static void LoadTodolist(string[] commandWord, out string currentPath, out List<Activity> todoList)
        {
            List<Activity> temp = new List<Activity>();
            currentPath = commandWord[1];
            string[] lines = File.ReadAllLines(currentPath);
            foreach (string line in lines)
            {
                string[] i = line.Split('#');
                temp.Add(new Activity(i[0], i[1], i[2]));

            }
            todoList = temp;
        }

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

        private static void SaveToFile(string[] commandWord, string currentPath, List<Activity> todoList)
        {
            string[] toSave = new string[todoList.Count()];
            for (int i = 0; i < todoList.Count(); i++)
            {
                toSave[i] = (todoList[i].ToString());
            }
            if (commandWord.Length == 1)
            {
                File.WriteAllLines(currentPath, toSave);
            }
            else
            {
                File.WriteAllLines(commandWord[1], toSave);
            }
        }
    }
}
