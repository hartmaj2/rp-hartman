/// <summary>
/// Tracks the current state of task completion by the user. 
/// </summary>
public class QuestService
{

    public IEnumerable<Quest> AllQuests { get; set; }
    public IEnumerable<Quest> CompletedQuests => AllQuests.Where(quest => quest.CompetionState == QuestCompletionState.Completed);
    public IEnumerable<Quest> ActiveQuests => AllQuests.Where(quest => quest.CompetionState == QuestCompletionState.Active);
    public IEnumerable<Quest> FutureQuests => AllQuests.Where(quest => quest.CompetionState == QuestCompletionState.Future);

    public int Score { get; set; }

    public QuestService()
    {
        AllQuests = new List<Quest>
        {
            new Quest
            {
                TaskId = QuestId.DefaultQuest,
                Description = "Přečti si úvodní text",
                PageId = "root-page",
                ElementIdsToSelect = new List<string> {},
                CompetionState = QuestCompletionState.Completed,
            },
            new Quest 
            { 
                TaskId = QuestId.WrongAgeSorting,
                Description = "Najdi jména pěti nejstarších účastníků tábora",
                PageId = "all-participants",
                ElementIdsToSelect = new List<string> {"age-sorter"},
                CompetionState = QuestCompletionState.Active,
            },
        };
    }

    public Quest GetById(QuestId taskId)
    {
        return AllQuests.First(task => task.TaskId == taskId);
    }

    /// <summary>
    /// Checks if what elements user selected corresponds to some active task.
    /// </summary>
    /// <param name="selectedElemsIds"> List of ids of the selected elements on the page </param>
    /// <returns> The result of the selection (partially correct, completely incorrect, correct etc.) </returns>
    public QuestSelectionResult ResolveQuestSelection(IEnumerable<string> selectedElemsIds)
    {
        return QuestSelectionResult.Correct;
    }

}

/// <summary>
/// Represents a task to be completed by the user. During each task, the user will have to report a bug also.
/// </summary>
public class Quest
{
    public required QuestId TaskId { get; set; }
    public required string PageId { get; set; }
    public required string Description { get; set; }

    public required List<string> ElementIdsToSelect { get; set; }
    
    public QuestCompletionState CompetionState { get; set; } = QuestCompletionState.Future;

}

/// <summary>
/// Represents whether the task is completed, active or not yet started.
/// </summary>
public enum QuestCompletionState
{
    Completed,
    Active,
    Future
}

/// <summary>
/// List of possible results of selection. Used to print custom messages to user.
/// </summary>
public enum QuestSelectionResult
{
    MoreElementsCausingBug,
    LessElementsCausingBug,
    CompletelyWrong,
    Correct
}


/// <summary>
/// Serves as unique ids for the tasks
/// </summary>
public enum QuestId
{
    DefaultQuest,
    WrongAgeSorting
}