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

## 5.11.2024

- [x] fix modals being covered partially by the top bar

- [ ] make bug reporting possible in modals
  - [ ] make elements on the modal highlighted when the mode is bug reporting

- [ ] add a bug with the validator not letting entering a birth number without a value
  - [ ] automatically fix this bug

- [x] translate specific error messages from custom validators to Czech

## 6.11.2024

- [x] fix bug with unsubscribing the underlying page of the modal when modal subscribes to OnClientModeChanged

- [x] change validation logic inside birth number validator dynamically 
  - [x] maybe use the overit splneni aktivniho ukolu button?

## 7.11.2024

- [x] implement delete participant modal using a custom modal

## 12.11.2024

- [x] test if possible to change z-index of elements inside a custom add participant modal
  - [x] implement add participant modal as custom modal
  - [x] try to change z-index of some elements dynamically when report bug button pressed

## 3.12.2024

- [x] use separate pages instead of modals for various tasks (DON'T NEED TO DO)

## 4.12.2024

- [x] fix non working bug reporting when inside participant add form (only when selecting correct element)
- [ ] fix error when the second quest is successful -> the modal with selection result is not showing
  - [ ] maybe I need to add an event callback that will show the selection modal of the parent of my custom modal 
  - [ ] that will also save me from having to store the selection result modal inside the participant form element
- [ ] finish second task implementation by reporting the bug
- [ ] add bug with missing button for deleting a participant

## Near future

- [ ] add a possibility to choose a bug from a list of possible bugs (for bugs that can not be marked)

## Far future

- [ ] add system for evaluating if the quest was completed (not just bug found)
  - [ ] maybe add a button for checking if the necessary change in the system was made (added something, removed something, edited something, etc.)
  - [ ] sometimes, a form will pop up with questions to answer