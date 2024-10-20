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

## 20.10.2024

- [ ] implement system for evaluating correct element as bug marking
  - [ ] when clicking exactly the broken elements, the bug will get fixed
- [ ] make bug reporting possible in modals
  - [ ] maybe implement custom modal