using System;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource thrusterAudioSource;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        player.OnBeforeForce += Player_OnBeforeForce;
        player.OnUpForce += Player_OnUpForce;
        player.OnLeftForce += Player_OnLeftForce;
        player.OnRightForce += Player_OnRightForce;
        thrusterAudioSource.Pause();
    }

    private void Player_OnRightForce(object sender, EventArgs e)
    {
        if (!thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Play();
        }
    }

    private void Player_OnLeftForce(object sender, EventArgs e)
    {
        if (!thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Play();
        }
    }

    private void Player_OnUpForce(object sender, EventArgs e)
    {
        if (!thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Play();
        }
    }

    private void Player_OnBeforeForce(object sender, EventArgs e)
    {
        thrusterAudioSource.Pause();
    }
}
