# Simple-PlayFab-Based-Unity-Quiz-Game
by Shahryar Saqib
-Downloads questions from the playfab Titledata, Stores player highscore and Adeptness(used to asses what question difficulty to show player) in the userdata on playfab.
## Instructions To Test
### Link to your playfab account
-Link your playfab account instructions provided in the INSTRUCTIONS FOLDER.
###Upload provided quesiton's JSON 
-Upload the provided(Inside instructions folder) JSOn title data on your playfab account (These are quesitons that the game will download)
##Event based infrastructure
The eventmanager.cs and Globalsettings.cs are the only singletons in the game. everything else is completly decoupled from everything else. Objects fire events in the eventmanagers which others can listen in.

## Difficulty Adaptability system
-The DynamicDifficultyAdapter.cs object when present in the scene fires events to shift difficulty and those are picked up by the quesitonsmanager and it starts giving questions based on the new difficulty.
-The DynamicDifficultyAdapter (Totally configurable from the inspector) can be configured to consider a number of questions , i.e hostorical performance. i.e how many questions you answered correctly and what was their difficulty.based on that it calculates an Adeptness score, which is then used to shift difficulty.
-adeptness score takes into consideration, the configurable properties of DynamicDifficultyAdapter which are WHAT PERCENTAGE OF QUESTIONS IN A PERTICULAR DIFFICULTY that the player is able to consistantly answer.
-For example: EASY quesitons get an adeptness score of 1, Medium 2, and hard 3. So if the DynamicDifficultyAdapter is set to shift difficulties at 80% performance in a perticular difficulty. then if DynamicDifficultyAdapter is set to consider a history of 5 last questions then answering 4 easy questions out of 5 in your history will cause the system to start showing medium questions.
-Similarly. If you answer 4 (80%) medium quesitons, correctly in your history i.e get a score if 8, in (5 history setting) it will shift to Hard.

