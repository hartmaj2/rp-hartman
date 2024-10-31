using BlazorBootstrap;


/// <summary>
/// Tracks the current state of task completion by the user. 
/// </summary>
public class QuestService
{

    private const int CORRECT_SELECTIONS_POINTS_GAIN = 5;
    private const int PARTIALLY_INCORRECT_POINTS_GAIN = -1;
    private const int COMPLETELY_INCORRECT_POINTS_GAIN = -2;

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
    /// Returns what result the bug selection corresponds do while also adjusting score and the fact, whether the bug of active quest was fixed
    /// </summary>
    /// <param name="selectedElemsIds"> List of ids of the selected elements on the page </param>
    /// <returns> The result of the selection (partially correct, completely incorrect, correct etc.) </returns>
    public BugSelectionResult ResolveQuestSelection(IEnumerable<string> selectedElemsIds, Quest activeQuest)
    {
        BugSelectionResult result = GetBugSelectionResult(selectedElemsIds,activeQuest);
        if (result == BugSelectionResult.Correct)
        {
            activeQuest.BugFixed = true;
        }
        Score += GetPointIncrease(result);
        return result;
    }

    /// <summary>
    /// Returs what result this bug selection corresponds to without doing any side effects
    /// </summary>
    /// <param name="selectedElemsIds"></param>
    /// <param name="activeQuest"></param>
    /// <returns> The result of the selection (partially correct, completely incorrect, correct etc.) </returns>
    public BugSelectionResult GetBugSelectionResult(IEnumerable<string> selectedElemsIds, Quest activeQuest)
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
            return BugSelectionResult.Correct;
        }
        if (allSelectedInTarget)
        {
            return BugSelectionResult.MoreElementsCausingBug;
        }
        if (allTargetInSelected)
        {
            return BugSelectionResult.LessElementsCausingBug;
        }
        return BugSelectionResult.CompletelyWrong;

    }

    /// <summary>
    /// Returns what text should be shown to user according to the result of their selection
    /// </summary>
    /// <param name="selectionResult"> The result of users selection </param>
    /// <returns> The text to be printed to the user </returns>
    /// <exception cref="Exception"> If the selectionResult is invalid </exception>
    public string GetTextForSelectionResult(BugSelectionResult selectionResult)
    {
        return selectionResult switch 
        {
            BugSelectionResult.MoreElementsCausingBug => "Jsi na dobré cestě, ale chybu způsobují i nějaké další prvky, které nemáš vybrané.",
            BugSelectionResult.LessElementsCausingBug => "Pozor! Máš vybrané i prvky, které chybu nezpůsobují.",
            BugSelectionResult.CompletelyWrong => "Špatná volba!",
            BugSelectionResult.Correct => "Dobrá práce!",
            BugSelectionResult.NothingSelected => "Nemáš vybrané žádné prvky!",
            BugSelectionResult.BugAlreadyFound => "Chybu pro tento úkol už máš nahlášenou!",
            _ => throw new Exception("Uncovered BugSelectionResult enum by a switch statement")
        };
    }

    /// <summary>
    /// Calculates points increase based on the result of the bug selection
    /// </summary>
    /// <param name="selectionResult"> The result of users selection </param>
    /// <returns> The amount of points the user should get </returns>
    /// <exception cref="Exception"> If the selectionResult is invalid </exception>
    public int GetPointIncrease(BugSelectionResult selectionResult)
    {
        return selectionResult switch
        {
            BugSelectionResult.Correct => CORRECT_SELECTIONS_POINTS_GAIN,
            BugSelectionResult.MoreElementsCausingBug => PARTIALLY_INCORRECT_POINTS_GAIN,
            BugSelectionResult.LessElementsCausingBug => PARTIALLY_INCORRECT_POINTS_GAIN,
            BugSelectionResult.CompletelyWrong => COMPLETELY_INCORRECT_POINTS_GAIN,
            BugSelectionResult.NothingSelected => 0,
            BugSelectionResult.BugAlreadyFound => 0,
            _ => throw new Exception("Wrong selection result argument"),
        };
    }

    /// <summary>
    /// Returns the text that should be shown to user when their score updated by pointsGain
    /// </summary>
    /// <param name="pointsGain"> How many points the user gains </param>
    /// <returns> The text to be printed to user </returns>
    public string GetPointsUpdateText(int pointsGain)
    {
        
        if (pointsGain == 0)
        {
            return "Neztrácíš ani nezískáváš žádné body.";
        }
        string text = "";
        if (pointsGain < 0) text = "Ztrácíš";
        else if (pointsGain > 0) text = "Získáváš";
        text += " " + Math.Abs(pointsGain) + " ";
        if (Math.Abs(pointsGain) == 1) text += "bod.";
        else if (Math.Abs(pointsGain) == 2 || Math.Abs(pointsGain) == 3 || Math.Abs(pointsGain) == 4) text += "body.";
        else text += "bodů.";
        return text;
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