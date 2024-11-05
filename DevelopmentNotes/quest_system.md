# Quest system of the game

The goal of the game is to complete a certain number of quests. These quests are managed by the `QuestService` class that manages a list of `Quest` instances.

## What does a quest consist of 

A quest consists of the following properies:

- id - this must be unique and is of type `QuestId` which is an enum 
  - this allows me to retrieve the quest by its id whenever I would like to in the code
  - this might not be a good idea but since the quests are static and won't change during the run of the program, it should be fine

- short description - should be a one line description of the task the user has to complete (visible in tasks list)

- description appendix - additional information that the user will need to compete the quest (will show under the short description in the task list)

- page id - so far not used in the program 
  - is helpful when thinking, where the bug actually is if you forget what bug it was

- completion state - enum with three possible states the quest can be in
  - completed
  - active
  - future

- bug target ids - the ids of the components that are causing the bug
  - this is used for checking whether the user marked relevant fields as causing the bug

- bug fixed - marks whether the player fixed the bug (THIS MIGHT NOT BE THE SAME AS COMPLETING THE QUEST IN THE FINAL PROGRAM)
  - after the bug is fixed, it should be easier to complete the quest

- bug description - text describing the bug showed to the user after they correctly reported the bug
  - can be a long text


## How are the quests stored

The quests are stored in a List. I also tried to make most functions work with just IEnumerable so it is not dependent on specific data structure. Also, there will be just about 10 or so tasks so I don't need to think too hard about any efficient data structure since the time for access,update etc. will always be constant. (since the size is a constant)

The main thing to focus on should be modularity and ease of use of the data structure.

## Non written rules that the system follows

The quests have to be completed in the order they are stored in the data structure.

Every time, only a single quest is marked as active. 