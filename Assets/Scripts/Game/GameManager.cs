using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public enum GameMode
    {
        Kill = 1,
        Survival = 2,
    }

    public static Action GameStart;
    public static Action GameEnd;
    internal float TimeToSurvive;
    private PlayerController player;
    public static Action ObjectiveUpdate;
    public static GameMode CurGameMode;
    private EnemySpawner spawner;
    [SerializeField] TextMeshProUGUI ObjectiveText;
    [SerializeField] private GameObject Endscreen;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] TextMeshProUGUI ContinueOrRestartText;
    [SerializeField] TextMeshProUGUI ScoreText;


    private void Start()
    {
        Time.timeScale = 1;
        player = FindFirstObjectByType<PlayerController>();
        spawner = GetComponentInChildren<EnemySpawner>();
        GameStart?.Invoke();
        SetUpGameMode();
        GameEnd += onGameEnd;
        ObjectiveUpdate += OnObjectiveUpdate;
        player.playerOnDeath += onGameEnd;
    }


    private void Update()
    {
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
        if (player.Health > 0)
        {
            OpenEndScreen();
            GameModifiers.stageNumber += 1;
            GameModifiers.score += GameModifiers.stageNumber * (player.enemiesKilled * 100) *
                                   GameModifiers.overallScoreModifer;
            ScoreText.text = $"Current Score: {GameModifiers.score}";
            resultText.text = "Mission Completed!";
            ContinueOrRestartText.text = "Continue?";
            print("Mission Completed!");
        }
        else
        {
            OpenEndScreen();
            print("Failed");
            GameModifiers.stageNumber = 1;
            ContinueOrRestartText.text = "Restart?";
            GameModifiers.score = GameModifiers.score;
            ScoreText.text = $"Overall Score: {GameModifiers.score}";
            resultText.text = "Mission Failed...";
        }
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void ToChallengeScreen()
    {
        SceneManager.LoadScene("ChallengeSelection");
    }

    private void OpenEndScreen()
    {
        Time.timeScale = 0;
        if (GameEnd is not null)
            Endscreen.SetActive(true);
    }

    private void OnDestroy()
    {
        GameEnd -= onGameEnd;
        ObjectiveUpdate -= OnObjectiveUpdate;
        if (player != null)
            player.playerOnDeath -= onGameEnd;

        StopAllCoroutines();
    }

    private void SetUpGameMode()
    {
        switch (CurGameMode)
        {
            case GameMode.Kill:

                break;
            case GameMode.Survival:
                TimeToSurvive = 30;
                OnObjectiveUpdate();
                StartCoroutine(Survive());
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        OnObjectiveUpdate();
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