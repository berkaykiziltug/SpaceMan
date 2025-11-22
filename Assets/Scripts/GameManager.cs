using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    
    private int score;
    private float time;

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
    }

    private void Update()
    {
        time += Time.deltaTime;
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
}
