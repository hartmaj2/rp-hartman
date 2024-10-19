using Client.Components;


/// <summary>
/// Tracks the current state of task completion by the user. 
/// </summary>
public class TaskService
{
    public IEnumerable<Task> FinishedTasks { get; set; }
    public IEnumerable<Task> ActiveTasks { get; set; }
    public IEnumerable<Task> FutureTasks { get; set; }

    public int Score { get; set; }

    /// <summary>
    /// Field is nullable, if no subsribers -> null
    /// </summary>
    public TaskService()
    {
        ActiveTasks = new List<Task>
        {
            new Task 
            { 
                TaskId = "1",
                Description = "Najdi jména pěti nejstarších účastníků tábora",
                PageId = "all-participants",
                ElementIdsToSelect = new List<string> {"age-sorter"}
            }
        };
        FinishedTasks = new List<Task>
        {
            new Task
            {
                TaskId = "0",
                Description = "Přečti si úvodní text",
                PageId = "root-page",
                ElementIdsToSelect = new List<string> {}
            }
        };
        FutureTasks = new List<Task>();
    }


}

/// <summary>
/// Represents a task to be completed by the user. During each task, the user will have to report a bug also.
/// </summary>
public class Task
{
    public required string TaskId { get; set; }
    public required string PageId { get; set; }
    public required string Description { get; set; }

    public required List<string> ElementIdsToSelect { get; set; }
    public Action? OnBugFixed;

    public void FixBug()
    {
        // Trigger the event when the bug is fixed
        OnBugFixed?.Invoke();
    }
}

