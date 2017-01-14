public class DifficultySystemTray : Dialoq
{
    public void DifficultyChanged(QuestionDifficulty difficulty)
    {
        TriggerDialog("Current Difficulty Set at: " + difficulty + " Player Adeptness Score: " + DynamicDifficultyAdapter.currentPlayerAdeptness);
    }
}