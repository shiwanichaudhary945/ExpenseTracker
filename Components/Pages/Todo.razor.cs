using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace ExpenseTracker.Components.Pages
{
    public partial class Todo : ComponentBase
    {
        private string newTask = string.Empty;
        private List<TodoItem> todoList = new();
        private string message = string.Empty;

        // Adds a new task to the list
        private void AddTask()
        {
            if (!string.IsNullOrWhiteSpace(newTask))
            {
                todoList.Add(new TodoItem { Id = Guid.NewGuid(), TaskName = newTask, IsCompleted = false });
                newTask = string.Empty;
                message = "Task added successfully!";
            }
        }

        // Removes a task from the list by GUID
        private void RemoveTask(Guid taskId)
        {
            var taskToRemove = todoList.FirstOrDefault(t => t.Id == taskId);
            if (taskToRemove != null)
            {
                todoList.Remove(taskToRemove);
                message = "Task removed successfully!";
            }
            else
            {
                message = "Task not found.";
            }
        }

        // Toggles the completion status of a task
        private void ToggleTaskStatus(TodoItem task)
        {
            task.IsCompleted = !task.IsCompleted;
            message = $"Task '{task.TaskName}' status toggled!";
        }

        // Marks a task as being edited
        private void EditTask(TodoItem task)
        {
            task.IsBeingEdited = true;
            message = $"Editing task '{task.TaskName}'...";
        }

        // Updates a task and stops editing
        private void UpdateTask(TodoItem task)
        {
            task.IsBeingEdited = false;
            message = $"Task '{task.TaskName}' updated!";
        }

        // Saves tasks to JSON format on the desktop
        private void SaveTasks()
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = Path.Combine(desktopPath, "tasks.json");

                var json = JsonSerializer.Serialize(todoList);
                System.IO.File.WriteAllText(filePath, json);

                message = "Tasks saved successfully to the Desktop!";
            }
            catch (Exception ex)
            {
                message = $"Error saving tasks: {ex.Message}";
            }
        }

        // Loads tasks from a JSON file on the desktop
        private void LoadTasks()
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = Path.Combine(desktopPath, "tasks.json");

                if (System.IO.File.Exists(filePath))
                {
                    var json = System.IO.File.ReadAllText(filePath);
                    todoList = JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new();
                    message = "Tasks loaded successfully from the Desktop!";
                }
                else
                {
                    message = "No tasks file found on the Desktop.";
                }
            }
            catch (Exception ex)
            {
                message = $"Error loading tasks: {ex.Message}";
            }
        }

        // To-Do Item model
        private class TodoItem
        {
            public Guid Id { get; set; } // Unique identifier for each task using GUID
            public string TaskName { get; set; } = string.Empty;
            public bool IsCompleted { get; set; }
            public bool IsBeingEdited { get; set; } = false; // Indicates if the task is being edited
        }
    }
}
