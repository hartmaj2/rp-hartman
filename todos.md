# Todos for rocnikovy projekt


## 2.10.2024

- [x] add intro screen - text in the middle (now just for the organizer)
- [x] add dropdown with tasks list


## 3.10.2024

- [x] add intro screen - game description + rules, button to start game


## 5.10.2024

- [x] introduce a service that tracks whether the client is in bug reporting mode
- [x] add overlay after clicking `Report a bug` button
- [x] make buttons inside the layout different when BugReporting mode active


## 6.10.2024

- [x] make selectable items glow and above the overlay when BugReporting mode active
- [x] make items highlighted green when selected in BugReporting mode 


## 19.10.2024

- [x] translate all text in the app to Czech
  - [x] use MealType in MealFormData instead of string (the Client and Server apps are dependent anyway)
  - [x] then I could get rid of MealService
- [x] added TaskService class
  - [x] added Task class to represent a task with its id,description etc.
  - [x] made a single list of all tasks, CompletedTasks are now a read only property


## 23.10.2024

- [x] add bug fixed field to every task which will be something different that task completed (it will be possible to complete the quest without reporting/fixing the bug)
- [x] add dummy button for triggering a quest completion
- [x] implement system for evaluating if selected elements really cause the bug
  - [x] when clicking exactly the broken elements, the bug will get fixed (for the task, the bugfixed field will be set to true)
  - [x] add code into the broken component that makes it behave depending on the state of the corresponding task

- [x] add score 
- [x] add message to task list saying that the bug for active quest was reported successfuly

- [x] fix bug when initializing a component multiple times, the handler subscribed multiple times also

- [x] fix error with task list not rerendering when shown
  - [x] add corresponding event to ClientModeService
  - [x] trigger the event when the ShowTaskList button in MainLayout is clicked
  - [x] subscribe to the event in TaskList component by doing StateHasChanged

## 31.10.2024

- [x] after correctly reporting a bug:
  - [ ] say name of the bug (what was the bug)
  - [x] state that it was fixed
  - [x] how many points increased
- [x] incorrectly reporting:
  - [x] how many points decreased

- [x] think about more quests to be done with corresponding bugs

## 4.11.2024

- [ ] make bug reporting possible in modals
  - [x] fix problem with highlighting elements on the page behind the modal
  - [ ] make elements on the modal highlighted when the mode is bug reporting

- [x] add entry to denik.md about the bug with tasks list not rendering on 23.10.2024

## Near future

- [ ] add a possibility to choose a bug from a list of possible bugs (for bugs that can not be marked)

## Far future

- [ ] add system for evaluating if the quest was completed (not just bug found)
  - [ ] maybe add a button for checking if the necessary change in the system was made (added something, removed something, edited something, etc.)
  - [ ] sometimes, a form will pop up with questions to answer