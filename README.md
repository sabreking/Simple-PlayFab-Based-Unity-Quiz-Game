# Simple-PlayFab-Based-Unity-Quiz-Game
by Shahryar Saqib
**you can download a running Build from [HERE](https://drive.google.com/file/d/0ByIcHV1vg2B6ZUFwVkdzUXE0dHc/view?usp=sharing) this uses MY playfab back end, if you open the project in your unity, you might need to enter your own playfab credentials
 **
 **Disclaimer** The game might contain Bugs as it is a **Prototype** produced rapidly and hasnt been throughly tested yet or bugs that might have gone under the radar.
  
* Downloads questions from the playfab Titledata, Stores player highscore and Adeptness(used to asses what question difficulty to show player) in the userdata on playfab.
* 2D Game using Unity UI (very simple prototyping intentioned UI)
* Game mechanic: a quiz game with multiple choice questions that are stored somewhere on the internet: playfab.
* Questions have time limit. For example: 5 seconds.
* During each quiz game the game should adapts the difficulty of the questions dynamically based on player performance and keeps it on the servier(PlayFab) for the next game including app restart. (explaination of how i designed this dynamic difficulty system provided below)
* There are questions for at least 3 difficulty levels.
* Support multiple screen ratios, portrait and landscape orientations(Ui should adapt to most orientaitons, and both landscape and potrait, since no perticular target was provided, it was designed on 4:3 but it will adapt , unless the screen aspect isnt rediculus the game should adapt fine).
* Win/Lose mechanic: 5 correct answers in a row to win, 3 wrong answers in a row to lose
* High scores stored in the same system you stored the questions(Being stored in PlayFab, the game automatically downloads the highscore, displays it and updates it on the server if you beat your highscore etc).
* I tried to make tests for the game but cudnt conceptualize the need for tests, since the game wasnt coded in test driven approach, i went with event based archetecture.I can however do so using Nunit framework etc.if so asked.

## Instructions To Test

### Link to your playfab account
* Link your playfab account instructions provided in the INSTRUCTIONS FOLDER.

### Upload provided quesiton's JSON 
* Upload the provided(Inside instructions folder) JSOn title data on your playfab account (These are quesitons that the game will download)

## Event based infrastructure
The eventmanager.cs and Globalsettings.cs are the only singletons in the game. everything else is completly decoupled from everything else. Objects fire events in the eventmanagers which others can listen in.The whole game is running on events. I used the unity's event wrapper instead of delegate events to save time and use unity serialiabity of unity events to conveneiently make a notifications/dialog system using inspector wiring for minor things

## Difficulty Adaptability system
* The DynamicDifficultyAdapter.cs object when present in the scene fires events to shift difficulty and those are picked up by the quesitonsmanager and it starts giving questions based on the new difficulty.
* The DynamicDifficultyAdapter (Totally configurable from the inspector) can be configured to consider a number of questions , i.e hostorical performance. i.e how many questions you answered correctly and what was their difficulty.based on that it calculates an Adeptness score, which is then used to shift difficulty.
* adeptness score takes into consideration, the configurable properties of DynamicDifficultyAdapter which are WHAT PERCENTAGE OF QUESTIONS IN A PERTICULAR DIFFICULTY that the player is able to consistantly answer.
* For example: EASY quesitons get an adeptness score of 1, Medium 2, and hard 3. So if the DynamicDifficultyAdapter is set to shift difficulties at 80% performance in a perticular difficulty. then if DynamicDifficultyAdapter is set to consider a history of 5 last questions then answering 4 easy questions out of 5 in your history will cause the system to start showing medium questions.
* Similarly. If you answer 4 (80%) medium quesitons, correctly in your history i.e get a score if 8, in (5 history setting) it will shift to Hard.

