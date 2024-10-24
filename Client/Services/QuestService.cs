/// <summary>
/// Tracks the current state of task completion by the user. 
/// </summary>
public class QuestService
{

    public IEnumerable<Quest> AllQuests { get; init; }
    public IEnumerable<Quest> CompletedQuests => AllQuests.Where(quest => quest.CompetionState == QuestCompletionState.Completed);
    public Quest ActiveQuest => AllQuests.Where(quest => quest.CompetionState== QuestCompletionState.Active).First();
    public IEnumerable<Quest> FutureQuests => AllQuests.Where(quest => quest.CompetionState == QuestCompletionState.Future);

    private int _score = 10;

    public int Score 
    { 
        get => _score;  
        private set
        {
            _score = Math.Max(0,value);
        }
    }

    public QuestService()
    {
        AllQuests = new List<Quest>
        {
            new Quest
            {
                TaskId = QuestId.DefaultQuest,
                Description = "Přečti si úvodní text",
                PageId = "root-page",
                TargetIds = new List<string> {},
                CompetionState = QuestCompletionState.Completed,
            },
            new Quest 
            { 
                TaskId = QuestId.WrongAgeSorting,
                Description = "Najdi jména pěti nejstarších účastníků tábora",
                PageId = "all-participants",
                TargetIds = new List<string> {"age-sorter"},
                CompetionState = QuestCompletionState.Active,
            },
        };
    }

    public Quest GetById(QuestId taskId)
    {
        return AllQuests.First(task => task.TaskId == taskId);
    }

    /// <summary>
    /// Tells a page element, if the state should be broken or not depending on the quest state
    /// </summary>
    /// <param name="taskId"> Id of the quest that the broken element corresponds to </param>
    /// <returns> True if it should work correctly (is fixed or not yet relevant) </returns>
    public bool ShouldWorkCorrectly(QuestId taskId)
    {
        return GetById(taskId).BugFixed;
    }

    /// <summary>
    /// Checks if what elements user selected corresponds to some active task.
    /// </summary>
    /// <param name="selectedElemsIds"> List of ids of the selected elements on the page </param>
    /// <returns> The result of the selection (partially correct, completely incorrect, correct etc.) </returns>
    public BugSelectionResult ResolveQuestSelection(IEnumerable<string> selectedElemsIds)
    {

        Quest active = ActiveQuest;

        // user already found the bug corresponding to the active quest
        if (active.BugFixed)
        {
            return BugSelectionResult.BugAlreadyFound;
        }

        // The user clicked on confirm selection without having anything selected
        if (!selectedElemsIds.Any())
        {
            return BugSelectionResult.NothingSelected;
        }

        // check if for all elements it is true, that the target ids of the active tasks contains such an id
        bool allSelectedInTarget = selectedElemsIds.All(active.TargetIds.Contains);
        bool allTargetInSelected = active.TargetIds.All(selectedElemsIds.Contains);

        if (allSelectedInTarget && allTargetInSelected)
        {
            active.BugFixed = true;
            Score += 5;
            return BugSelectionResult.Correct;
        }
        if (allSelectedInTarget)
        {
            Score -= 1;
            return BugSelectionResult.MoreElementsCausingBug;
        }
        if (allTargetInSelected)
        {
            Score -= 1;
            return BugSelectionResult.LessElementsCausingBug;
        }
        Score -= 2;
        return BugSelectionResult.CompletelyWrong;

    }

}

/// <summary>
/// Represents a task to be completed by the user. During each task, the user will have to report a bug also.
/// </summary>
public class Quest
{
    public required QuestId TaskId { get; init; }
    public required string PageId { get; init; }
    public required string Description { get; init; }

    public required List<string> TargetIds { get; init; }
    
    public QuestCompletionState CompetionState { get; set; } = QuestCompletionState.Future;

    public bool BugFixed { get; set; } = false;

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
public enum BugSelectionResult
{
    MoreElementsCausingBug,
    LessElementsCausingBug,
    CompletelyWrong,
    Correct,
    NothingSelected,
    BugAlreadyFound
}


/// <summary>
/// Serves as unique ids for the tasks
/// </summary>
public enum QuestId
{
    DefaultQuest,
    WrongAgeSorting
}