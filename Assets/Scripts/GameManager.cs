using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

     private static int levelNumber = 1;

     public event EventHandler OnGamePaused;
     public event EventHandler OnGameUnpaused;
     
    [SerializeField] private List<GameLevel> gameLevelList;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    private int score;
    private float time;
    private bool isTimerActive;
    private static int totalScore = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Player.Instance.OnCoinPickup+= PlayerOnCoinPickup;
        Player.Instance.OnLanded += PlayerOnLanded;
        Player.Instance.OnStateChanged += PlayerOnStateChanged;
        GameInput.Instance.OnMenuButtonPressed += GameInput_OnMenuButtonPressed;
        LoadCurrentLevel();
    }

    private void GameInput_OnMenuButtonPressed(object sender, EventArgs e)
    {
        PauseUnpauseGame();
    }

    public static void ResetStaticData()
    {
        levelNumber = 1;
        totalScore = 0;
    }

    private void PauseUnpauseGame()
    {
        if (Time.timeScale == 1f)
        {
            PauseGame();
        }
        else
        {
            UnpauseGame();
        }
    }

    private void LoadCurrentLevel()
    {
        GameLevel gameLevel = GetGameLevel();
        GameLevel spawnedGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
        Player.Instance.transform.position = spawnedGameLevel.GetPlayerStartPosition();
        cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetCameraStartPosition();
        CinemachineCameraZoom2D.Instance.SetTargetOrtographicSize(spawnedGameLevel.GetZoomedOutOrthographicSize());
    }

    private GameLevel GetGameLevel()
    {
        foreach (var gameLevel in gameLevelList)
        {
            if (gameLevel.GetLevelNumber() == levelNumber)
            {
                return gameLevel;
            }
        }
        return null;
    }
    private void PlayerOnStateChanged(object sender, Player.OnStateChangedEventArgs e)
    {
        isTimerActive = e.state == Player.State.Normal;
        if (e.state == Player.State.Normal)
        {
            cinemachineCamera.Target.TrackingTarget = Player.Instance.transform;
            CinemachineCameraZoom2D.Instance.SetNormalOrthoSize();
        }
    }

    private void Update()
    {
        if (isTimerActive)
        {
            time += Time.deltaTime;
        }
    }

    private void PlayerOnLanded(object sender, Player.OnLandedEventArgs e)
    {
        AddScore(e.onLandedScore);
    }

    private void PlayerOnCoinPickup(object sender, EventArgs e)
    {
        AddScore(500);
    }

    public void AddScore(int scoreAmount)
    {
        score += scoreAmount;
        Debug.Log(score);
    }

    public int GetScore()
    {
        return score;
    }

    public float GetTime()
    {
        return time;
    }

    public void GoToNextLevel()
    {
        levelNumber++;
        totalScore += score;
        if (GetGameLevel() == null)
        {
            //No More Levels
            SceneLoader.LoadScene(SceneLoader.Scene.GameOverScene);
        }
        else
        {
            //Still have more levels
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        }
        
    }

    public int GetTotalScore()
    {
        return totalScore;
    }

    public void RetryLevel()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
    }

    public int GetLevel()
    {
        return levelNumber;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        OnGameUnpaused?.Invoke(this, EventArgs.Empty);
    }
}
