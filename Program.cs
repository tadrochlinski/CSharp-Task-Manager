#pragma warning disable

using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;

namespace ConsoleApp
{
    public class Program
    {
        private static Dictionary<string, Action> menuOptions = new Dictionary<string, Action>()
        {
            {"1", AddTask},
            {"2", RemoveTask},
            {"0", ExitProgram}
        };

        private static TaskManager? taskManager;

        public static void Main(string[] args)
        {
            createDatabase("database.db");

            taskManager = new TaskManager("database.db");

            bool isRunning = true;

            while (isRunning)
            {
                DisplayTasks();
                Console.WriteLine();
                printMenu();

                string? choice = Console.ReadLine()?.Trim();

                if (choice != null && menuOptions.ContainsKey(choice))
                {
                    menuOptions[choice].Invoke();
                }
                else
                {
                    Console.WriteLine("Nieprawidłowy wybór.");
                }

                Console.Clear();
            }
        }

        public static void printMenu()
        {
            Console.WriteLine("=== MENU ===");
            Console.WriteLine("1. Dodaj zadanie");
            Console.WriteLine("2. Usuń zadanie");
            Console.WriteLine("0. Wyjście");
        }

        private static void AddTask()
        {
            Console.WriteLine();
            Console.WriteLine("Wybrano dodawanie zadania.");

            Console.WriteLine("Napisz nazwę zadania: ");
            string name = Convert.ToString(Console.ReadLine());

            Console.WriteLine("Napisz opis zadania: ");
            string desc = Convert.ToString(Console.ReadLine());

            DateTime date = DateTime.Now;

            if(name != "" && desc != "")
            {
                Task task = new Task
                {
                    Name = name,
                    Desc = desc,
                    Date = date
                };

                taskManager.AddTask(task);

            Console.WriteLine("Zadanie dodane.");
            }
            else{
                Console.WriteLine("Wpisano puste pola!");
            }
        }

        private static void RemoveTask()
        {
            Console.WriteLine();
            Console.WriteLine("Wybrano usuwanie zadania.");

            Console.WriteLine("Podaj ID zadania do usunięcia: ");
            if (int.TryParse(Console.ReadLine(), out int taskId))
            {
                taskManager.DeleteTask(taskId);
                Console.WriteLine("Zadanie usunięte.");
            }
            else
            {
                Console.WriteLine("Nieprawidłowy numer ID.");
            }
        }

        private static void DisplayTasks()
        {
            Console.WriteLine("=== Zadania ===");

            List<Task> tasks = taskManager.DownloadTasks();

            if (tasks.Count == 0)
            {
                Console.WriteLine("Brak zadań do wyświetlenia.");
            }
            else
            {
                foreach (Task task in tasks)
                {
                    Console.WriteLine($"ID: {task.ID}, Nazwa: {task.Name}, Opis: {task.Desc}, Data: {task.Date}");
                }
            }
        }

        private static void ExitProgram()
        {
            Console.WriteLine("Wybrano wyjście z programu.");
            Environment.Exit(0);
        }

        public static void createDatabase(string databaseFilename)
        {
            if (!File.Exists(databaseFilename))
            {
                string connectionString = $"Data Source={databaseFilename};";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Tasks (
                        ID INTEGER PRIMARY KEY,
                        Name TEXT,
                        Desc TEXT,
                        Date TEXT
                    )";

                    using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                        command.ExecuteNonQuery();

                    connection.Close();
                }
            }
        }
    }
}