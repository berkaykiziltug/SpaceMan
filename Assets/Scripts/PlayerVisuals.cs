using System;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftThrusterParticleSystem;
    [SerializeField] private ParticleSystem middleThrusterParticleSystem;
    [SerializeField] private ParticleSystem rightThrusterParticleSystem;
    [SerializeField] private GameObject crashExplosionVFX;
    
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

    private void Start()
    {
        Player.Instance.OnLanded += PlayerOnLanded;
    }
    private void PlayerOnLanded(object sender, Player.OnLandedEventArgs e)
    {
        switch(e.landingType)
        {
            case Player.LandingType.TooFastLanding: 
            case Player.LandingType.TooSteepAngle:
            case Player.LandingType.WrongLandingArea:
               //Crash
               Instantiate(crashExplosionVFX, transform.position, Quaternion.identity);
               gameObject.SetActive(false);
                break;
        }
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

    private void SetEnabledThrusterParticleSystem(ParticleSystem particleSystem, bool enabled)
    {
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        emissionModule.enabled = enabled;
    }
}
