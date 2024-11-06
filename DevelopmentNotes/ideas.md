# Improvement ideas

## Active ideas

Make bug reporting logic more scalable. Now I have to repeat fragments of code like:
- Subscribing to OnModeChanged in all components manually (could I do this always on inject?)
- Having a list of id's so I can initialize a dictionary that maps the ids to flags that signify if the element is selected 
- Method for getting the class of the element based on if it is selected

## Old ideas

- Add exception handling
- Add other sections needed for my RP
- Add switching between CZ and ENG
- Add authentication
- Add posibility to hide column with allergen list
- Generalize confirm delete modal so I can reuse code for delete participant and delete meal (maybe use generic types and single AddTypeModal.razor component)
- Make the width of my div table meal columns adjust themselves automatically based on the longest entry
- Add male/female 
- Create api manager (service) - all methods to communicate with api will be stored there in one place
- Add possibility to work with file instead of database (generalize controllers)
- Add no diets button in diets filter

