# RosterAPI
## Author: Jake Galligan
## Introduction:
This API allows for users to create custom teams, filled with rosters of unique players. Users have full control over both the creation and properties of each team and player in order to create their own sports franchises

## Swagger Documentation
Swagger documentation is available for all endpoints. It is recommended to go to [swagger editor](https://editor.swagger.io) and copy and paste swagger.yaml file into the terminal

## Note
Some slight changes in data structures were made to account for issues in updating context. Originally, each team had a playerList property and within the PUT method players would successfully be added to this property. However, these changes would not be saved to the global context and were inaccessible to other methods within the context(through debugging I narrowed the issue to either inability to properly save non primitive data types or inability for the context to pick up changes within the data structure of a child). After trying over a dozen attempts to resolve the issue, such as alerting for updates within the state that data had been modified as well and reloading context for new queries to update child properties, all to no avail, I decided to reformat the player data structure to include a teamId property on each player in order to keep track what team they are apart of. With this change, the API is still fully functional according to desired queries.

## Reflection
### Author Notes:
This was a great experience in learning Microsoft Entity framework in order to construct APIs, I got to pick up on how to approach backends within this environment, and am impressed by how efficient ME is. I  also enjoyed the sports theme of the task as most times when I am practicing new libraries/frameworks I often build a similar application based around teams/players.
