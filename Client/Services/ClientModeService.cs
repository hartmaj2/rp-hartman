// Tracks the current mode of the client web page, the page can be in modes: 1. Normal 2. BugReporting
public class ClientModeService
{
    private ClientMode _currentMode;

    public ClientModeService()
    {
        _currentMode = ClientMode.Normal;
    }

    // Flips the mode to the other one
    public void ToggleMode()
    {
        _currentMode = _currentMode == ClientMode.Normal ? ClientMode.BugReporting : ClientMode.Normal;
    }
}


// Enumeration for all the possible web page modes
public enum ClientMode
{
    Normal,
    BugReporting
}