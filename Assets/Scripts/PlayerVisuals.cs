using System;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftThrusterParticleSystem;
    [SerializeField] private ParticleSystem middleThrusterParticleSystem;
    [SerializeField] private ParticleSystem rightThrusterParticleSystem;
    
    private Player player;

    private void Awake()
    {
        player =  GetComponent<Player>();
        player.OnUpForce += PlayerOnUpForce;
        player.OnLeftForce += PlayerOnLeftForce;
        player.OnRightForce += PlayerOnRightForce;
        player.OnBeforeForce += PlayerOnBeforeForce;
        
        SetEnabledThrusterParticleSystem(middleThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(leftThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(rightThrusterParticleSystem, false);
    }

    private void PlayerOnBeforeForce(object sender, EventArgs e)
    {
        SetEnabledThrusterParticleSystem(middleThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(leftThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(rightThrusterParticleSystem, false);
    }

    private void PlayerOnRightForce(object sender, EventArgs e)
    {
        SetEnabledThrusterParticleSystem(leftThrusterParticleSystem, true);
    }

    private void PlayerOnLeftForce(object sender, EventArgs e)
    {
        SetEnabledThrusterParticleSystem(rightThrusterParticleSystem, true);
    }

    private void PlayerOnUpForce(object sender, EventArgs e)
    {
        SetEnabledThrusterParticleSystem(middleThrusterParticleSystem, true);
        SetEnabledThrusterParticleSystem(leftThrusterParticleSystem, true);
        SetEnabledThrusterParticleSystem(rightThrusterParticleSystem, true);
    }

    private void Start()
    {
      
    }

    private void SetEnabledThrusterParticleSystem(ParticleSystem particleSystem, bool enabled)
    {
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        emissionModule.enabled = enabled;
    }
}
