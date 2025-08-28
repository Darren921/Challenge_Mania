using System;
using System.Collections;
using TMPro;
using UnityEngine;

public  class GameManager  : MonoBehaviour
{
    public enum GameMode
    {
        Kill,
        Survival,
    }

    public static Action GameStart;  
    public static Action GameEnd;
    internal float TimeToSurvive;
    public static Action ObjectiveUpdate;
    public static GameMode CurGameMode = GameMode.Survival;
    private EnemySpawner spawner;
    [SerializeField] TextMeshProUGUI ObjectiveText;
    

    private void Start()
    {
        spawner = GetComponentInChildren<EnemySpawner>();
        GameStart?.Invoke();
        SetUpGameMode();
        GameEnd += onGameEnd;
        ObjectiveUpdate += OnObjectiveUpdate;
    }

    private void OnObjectiveUpdate()
    {
        switch (CurGameMode)
        {
            case GameMode.Kill:
                ObjectiveText.text = $"Eliminate all enemies, There are {spawner.curEnemyCount} left";
                break;
            case GameMode.Survival:
                ObjectiveText.text = $"SURVIVE, There is {Mathf.RoundToInt(TimeToSurvive)} seconds left";

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void onGameEnd()
    {
        throw new NotImplementedException();
    }

    private void SetUpGameMode()
    {
        switch (CurGameMode)
        {
            case GameMode.Kill:
                OnObjectiveUpdate();
                break;
            case GameMode.Survival:
                TimeToSurvive = 20;
                OnObjectiveUpdate();
                StartCoroutine(Survive());
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IEnumerator Survive()
    {
        while (TimeToSurvive > 0)
        {
            TimeToSurvive -= Time.deltaTime;
            OnObjectiveUpdate();

            if (TimeToSurvive <= 0)
            {
                GameEnd?.Invoke();
            }

            yield return null;
        }
    }
}
