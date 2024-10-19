using Client.Components;


/// <summary>
/// Tracks the current state of task completion by the user. 
/// </summary>
public class TaskService
{

    public IEnumerable<Task> AllTasks { get; set; }
    public IEnumerable<Task> CompletedTasks => AllTasks.Where(task => task.CompetionState == TaskCompletionState.Completed);
    public IEnumerable<Task> ActiveTasks => AllTasks.Where(task => task.CompetionState == TaskCompletionState.Active);
    public IEnumerable<Task> FutureTasks => AllTasks.Where(task => task.CompetionState == TaskCompletionState.Future);

    public int Score { get; set; }

    public TaskService()
    {
        AllTasks = new List<Task>
        {
            new Task
            {
                TaskId = "0",
                Description = "Přečti si úvodní text",
                PageId = "root-page",
                ElementIdsToSelect = new List<string> {},
                CompetionState = TaskCompletionState.Completed,
            },
            new Task 
            { 
                TaskId = "1",
                Description = "Najdi jména pěti nejstarších účastníků tábora",
                PageId = "all-participants",
                ElementIdsToSelect = new List<string> {"age-sorter"},
                CompetionState = TaskCompletionState.Active,
            },
        };
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
    
    public TaskCompletionState CompetionState { get; set; } = TaskCompletionState.Future;

}

public enum TaskCompletionState
{
    Completed,
    Active,
    Future
}

