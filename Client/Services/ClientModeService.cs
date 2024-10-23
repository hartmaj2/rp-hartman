/// <summary>
/// Tracks the current mode of the client web page, the page can be in modes: 1. Normal 2. BugReporting
/// </summary>
public class ClientModeService
{
    private ClientMode _currentMode;

    public ClientMode CurrentMode => _currentMode;

    public event Action? OnModeChangedEvent; // field is nullable, if no subsribers -> null

    public event Action? OnNotReportEvent;

    /// <summary>
    /// Triggers when user confirms selection of suspicious elements
    /// </summary>
    public event Func<Task>? OnBugSelectionConfirmedEvent;

    public event Action? OnTaskListShownEvent;

    public ClientModeService()
    {
        _currentMode = ClientMode.Normal;
    }

    public void SubscribeUniqueOnModeChanged(Action handler)
    {
        OnModeChangedEvent = null;
        OnModeChangedEvent += handler;
    }

    public void SubscribeUniqueOnSelectionConfirmed(Func<Task> handler)
    {
        OnBugSelectionConfirmedEvent = null;
        OnBugSelectionConfirmedEvent += handler;
    }

    public void SubscribeUniqueOnNotReportEvent(Action handler)
    {
        OnNotReportEvent = null;
        OnNotReportEvent += handler;
    }

    public void SubscribeUniqueOnTaskListShownEvent(Action handler)
    {
        OnTaskListShownEvent = null;
        OnTaskListShownEvent += handler;
    }

    /// <summary>
    /// Flips the mode to the other one
    /// </summary>
    public void ToggleMode()
    {
        _currentMode = _currentMode == ClientMode.Normal ? ClientMode.BugReporting : ClientMode.Normal;
        OnModeChangedEvent?.Invoke(); // only invoke when this event has subscribers
    }

    /// <summary>
    /// Triggers the event that will notify the page that the user wants to check the selection of suspicious elements
    /// </summary>
    public void ConfirmBugSelection()
    {
        OnBugSelectionConfirmedEvent?.Invoke();
        ToggleMode();
    }

    public void NotReportBug()
    {
        OnNotReportEvent?.Invoke();
        ToggleMode();
    }

    public void TriggerOnTaskListShownEvent()
    {
        OnTaskListShownEvent?.Invoke();
    }
}


/// <summary>
/// Enumeration for all the possible web page modes
/// </summary>
public enum ClientMode
{
    Normal,
    BugReporting
}