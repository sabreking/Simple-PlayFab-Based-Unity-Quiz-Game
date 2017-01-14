using System;

[Serializable]
public class Question
{
    public QuestionDifficulty questionDifficulty;
    public string questionString;
    public string[] answerStrings;
    public int correctIndex;
    public bool isAlreadyShownOnce;

    public float ScoreValue
    {
        get
        {
            switch (questionDifficulty)
            {
                case QuestionDifficulty.Easy:
                    return GlobalSettings.Instance.scoreAwardForEasyQuestions;
                case QuestionDifficulty.Medium:
                    return GlobalSettings.Instance.scoreAwardForEasyQuestions*GlobalSettings.Instance.scoreMultiplierForMediumQuestions;
                case QuestionDifficulty.Hard:
                    return GlobalSettings.Instance.scoreAwardForEasyQuestions*GlobalSettings.Instance.scoreMultiplierForHardQuestions;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    public float DifficultyValue
    {
        get
        {
            switch (questionDifficulty)
            {
                case QuestionDifficulty.Easy:
                    return 1f;
                case QuestionDifficulty.Medium:
                    return 2f;
                case QuestionDifficulty.Hard:
                    return 3f;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public Question(int correctIndex, string[] answerStrings, string questionString, QuestionDifficulty questionDifficulty)
    {
        this.correctIndex = correctIndex;
        this.answerStrings = answerStrings;
        this.questionString = questionString;
        this.questionDifficulty = questionDifficulty;
        isAlreadyShownOnce = false;
    }
}