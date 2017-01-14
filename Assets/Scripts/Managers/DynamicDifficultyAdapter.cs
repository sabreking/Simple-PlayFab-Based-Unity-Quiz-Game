using System;
using System.Collections.Generic;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class DynamicDifficultyAdapter : BroadcasterAndReceiver
{
    public static float currentPlayerAdeptness;
    public int numberOfQuestionHistoryToConsider = 5;
    [Header("What percentage of questions in difficulty to consistantly answer to jump difficulty")] [Tooltip("i.e 0.8 means if 80% of easy are consistantly answered it would shift to medium")] public float percentQualifier = 0.8f;
    [SerializeField] private CalculatedAdeptnessQualifiers _calculatedAdeptnessQualifiers;
    public Queue<float> historyQueue;
    private QuestionDifficulty _lastQuestionDifficultyAnnounced;

    protected override void Start()
    {
        base.Start();
        historyQueue = new Queue<float>();
        _calculatedAdeptnessQualifiers = new CalculatedAdeptnessQualifiers(numberOfQuestionHistoryToConsider*percentQualifier, numberOfQuestionHistoryToConsider*2*percentQualifier, numberOfQuestionHistoryToConsider*3*percentQualifier);
    }

    protected override void SubscribeToEvents()
    {
        base.SubscribeToEvents();
        EventSys.onPlayerLoggedIn.AddListener(GetUserData);
        EventSys.onQuestionAnsweredCorrectly.AddListener(QuestionAnsweredCorrectly);
        EventSys.onQuestionAnsweredInCorrectlyOrTimesUp.AddListener(QuestionAnsweredWrongly);
        EventSys.onGameWon.AddListener(SetOrUpdateUserData);
        EventSys.onGameLost.AddListener(SetOrUpdateUserData);
    }

    private void InitializeHistoryQueue()
    {
        for (var i = 0; i < numberOfQuestionHistoryToConsider; i++)
        {
            historyQueue.Enqueue(currentPlayerAdeptness);
        }
        CalculateAdeptness();
    }

    private void SetOrUpdateUserData()
    {
        var request = new UpdateUserDataRequest {Data = new Dictionary<string, string> {{"Adeptness", currentPlayerAdeptness.ToString()}}};

        PlayFabClientAPI.UpdateUserData(request, result => { Debug.Log("Successfully updated Adeptness data"); }, error =>
        {
            Debug.Log("Got error setting user data for adeptness");
            Debug.Log(error.ErrorDetails);
        });
    }

    private void GetUserData()
    {
        var request = new GetUserDataRequest {PlayFabId = PlayFabManager.playFabId, Keys = null};

        PlayFabClientAPI.GetUserData(request, result =>
        {
            Debug.Log("Got Adeptness data:");
            if ((result.Data == null) || (result.Data.Count == 0))
            {
                Debug.Log("No Adeptness data available");
                SetOrUpdateUserData();
            } else
            {
                currentPlayerAdeptness = float.Parse(result.Data["Adeptness"].Value);
                Debug.Log(currentPlayerAdeptness);
                InitializeHistoryQueue();
            }
        }, error =>
        {
            Debug.Log("Got error retrieving Adeptness data:");
            Debug.Log(error.ErrorMessage);
        });
    }

    private void QuestionAnsweredCorrectly(Question question)
    {
        AddToHistory(question.DifficultyValue); //easy get 1, med get 2, hard get 3
        CalculateAdeptness();
        AnnouceChangeInDifficultyIfApplicable();
    }

    private void QuestionAnsweredWrongly()
    {
        AddToHistory(0.0f);
        CalculateAdeptness();
        AnnouceChangeInDifficultyIfApplicable();
    }

    private void AddToHistory(float adeptnessScore)
    {
        if (historyQueue.Count >= numberOfQuestionHistoryToConsider)
        {
            historyQueue.Dequeue(); // remove the first item to keep the queue same size
        } else
        {
            historyQueue.Enqueue(adeptnessScore);
        }
    }

    private void CalculateAdeptness()
    {
        currentPlayerAdeptness = historyQueue.Average();
    }

    private void AnnouceChangeInDifficultyIfApplicable()
    {
        var decidedDifficulty = DecideQuestionDifficulty();
        if (decidedDifficulty != _lastQuestionDifficultyAnnounced)
        {
            EventSys.onChangeDifficulty.Invoke(decidedDifficulty);
            _lastQuestionDifficultyAnnounced = decidedDifficulty;
        }
    }

    private QuestionDifficulty DecideQuestionDifficulty()
    {
        if (currentPlayerAdeptness*numberOfQuestionHistoryToConsider <= _calculatedAdeptnessQualifiers.shiftToEasyAt)
        {
            return QuestionDifficulty.Easy;
        }
        if (currentPlayerAdeptness*numberOfQuestionHistoryToConsider <= _calculatedAdeptnessQualifiers.shiftToMediumAt)
        {
            return QuestionDifficulty.Medium;
        }
        return QuestionDifficulty.Hard;
    }
}

[Serializable]
public struct CalculatedAdeptnessQualifiers
{
    public float shiftToEasyAt;
    public float shiftToMediumAt;
    public float shiftToHardAt;

    public CalculatedAdeptnessQualifiers(float shiftToEasyAt, float shiftToMediumAt, float shiftToHardAt)
    {
        this.shiftToEasyAt = shiftToEasyAt;
        this.shiftToMediumAt = shiftToMediumAt;
        this.shiftToHardAt = shiftToHardAt;
    }
}