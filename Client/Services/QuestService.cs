/// <summary>
/// Tracks the current state of task completion by the user. 
/// </summary>
public class QuestService
{

    private const int POINTS_CORRECT_REPORT = 5;
    private const int POINTS_PARTIALLY_INCORRECT = -1;
    private const int POINTS_COMPLETELY_INCORRECT = -2;

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
                CompetionState = QuestCompletionState.Completed,
                BugTargetIds = new List<string> {},
            },
            new Quest 
            { 
                TaskId = QuestId.WrongAgeSorting,
                Description = "Najdi jména pěti nejstarších účastníků tábora",
                PageId = "all-participants",
                CompetionState = QuestCompletionState.Active,
                BugTargetIds = new List<string> {"age-sorter"},
                BugDescription = "Třídění žáků podle věku třídilo čísla, jako by se jednalo o slova ze slovníku, akorát místo písmenek jsme měli jednotlivé cifry. Ve slovníku rozhodujeme už podle prvního písmenka, že slovo začínající na 'a' musí být před slovem začínajícím na 'b'. U čísel ale nechceme, aby pokud začíná na cifru 1, tak bylo automaticky před číslem začínajícím na 2. To by přece znamenalo, že 10 je menší než 2, a tak se čísla netřídí!",
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
    public BugSelectionResult ResolveQuestSelection(IEnumerable<string> selectedElemsIds, Quest activeQuest)
    {

        // user already found the bug corresponding to the active quest
        if (activeQuest.BugFixed)
        {
            return BugSelectionResult.BugAlreadyFound;
        }

        // The user clicked on confirm selection without having anything selected
        if (!selectedElemsIds.Any())
        {
            return BugSelectionResult.NothingSelected;
        }

        // check if for all elements it is true, that the target ids of the active tasks contains such an id
        bool allSelectedInTarget = selectedElemsIds.All(activeQuest.BugTargetIds.Contains);
        bool allTargetInSelected = activeQuest.BugTargetIds.All(selectedElemsIds.Contains);

        if (allSelectedInTarget && allTargetInSelected)
        {
            activeQuest.BugFixed = true;
            Score += POINTS_CORRECT_REPORT;
            return BugSelectionResult.Correct;
        }
        if (allSelectedInTarget)
        {
            Score += POINTS_PARTIALLY_INCORRECT;
            return BugSelectionResult.MoreElementsCausingBug;
        }
        if (allTargetInSelected)
        {
            Score += POINTS_PARTIALLY_INCORRECT;
            return BugSelectionResult.LessElementsCausingBug;
        }
        Score += POINTS_COMPLETELY_INCORRECT;
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
    public QuestCompletionState CompetionState { get; set; } = QuestCompletionState.Future;

    public required List<string> BugTargetIds { get; init; }
    public bool BugFixed { get; set; } = false;
    public string? BugDescription { get; init; } = null;

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