public class Notifier : Dialoq
{
    public void CorrectAnswer()
    {
        TriggerDialog("CORRECT");
    }

    public void WrongAnswer()
    {
        TriggerDialog("WRONG");
    }

    public void LoggedIn()
    {
        TriggerDialog("LOGGED IN");
    }

    public void QuestionsDownloaded()
    {
        TriggerDialog("QUESTIONS DOWNLOADED");
    }

    public void DifficultyChanged(QuestionDifficulty questionDifficulty)
    {
        TriggerDialog("Difficulty Shift:" + questionDifficulty);
    }
}