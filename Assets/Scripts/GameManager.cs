using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

     private static int levelNumber = 1;
    [SerializeField] private List<GameLevel> gameLevelList;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    private int score;
    private float time;
    private bool isTimerActive;

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

        LoadCurrentLevel();
    }

    private void LoadCurrentLevel()
    {
        foreach (var gameLevel in gameLevelList)
        {
            if (gameLevel.GetLevelNumber() == levelNumber)
            {
               GameLevel spawnedGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
               Player.Instance.transform.position = spawnedGameLevel.GetPlayerStartPosition();
               cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetCameraStartPosition();
               CinemachineCameraZoom2D.Instance.SetTargetOrtographicSize(spawnedGameLevel.GetZoomedOutOrthographicSize());
            }
        }
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
        SceneManager.LoadScene(0);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(0);
    }

    public int GetLevel()
    {
        return levelNumber;
    }
}
