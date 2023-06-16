using System;
using System.Collections.Generic;
using System.Data.SQLite;

public class TaskManager
{
    private string connectionString;

    public TaskManager(string databasePath)
    {
        connectionString = $"Data Source = {databasePath}";
    }

    public void AddTask(Task task)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string query = "INSERT INTO Tasks (Name, Desc, Date) VALUES (@Name, @Desc, @Date)";

            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", task.Name);
                command.Parameters.AddWithValue("@Desc", task.Desc);
                command.Parameters.AddWithValue("@Date", task.Date);

                command.ExecuteNonQuery();
            }
        }
    }

    public void DeleteTask(int taskId)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string query = "DELETE FROM Tasks WHERE ID = @ID";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ID", taskId);

                command.ExecuteNonQuery();
            }
        }
    }

    public List<Task> DownloadTasks()
    {
        List<Task> tasks = new List<Task>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT ID, Name, Desc, Date FROM Tasks";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string desc = reader.GetString(2);
                            DateTime date = reader.GetDateTime(3);

                            Task task = new Task
                            {
                                ID = id,
                                Name = name,
                                Desc = desc,
                                Date = date
                            };

                            tasks.Add(task);
                        }
                    }
                }
            }

            return tasks;  
    }
}