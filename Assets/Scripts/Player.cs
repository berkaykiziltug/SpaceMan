using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnBeforeForce;

    public event EventHandler OnCoinPickup;

    public event EventHandler<OnLandedEventArgs> OnLanded;

    public class OnLandedEventArgs : EventArgs
    {
        public int onLandedScore;
    }
    
    private Rigidbody2D playerRigidbody2D;
    private float fuelAmount;
    private float fuelAmountMax = 10f;
    
    
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

        fuelAmount = fuelAmountMax;
        playerRigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);
        // Debug.Log(fuelAmount);
        if (fuelAmount <= 0)
        {
            fuelAmount = 0;
            return;
        };
        if (Keyboard.current.upArrowKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            //Pressing any key.
            ConsumeFuel();
        }
        if (Keyboard.current.upArrowKey.IsPressed())
        {
            float force = 700f;
            playerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
            OnUpForce?.Invoke(this, EventArgs.Empty);
        }
        if (Keyboard.current.leftArrowKey.IsPressed())
        {
            float turnSpeed = +100f;
            playerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);;
            OnLeftForce?.Invoke(this, EventArgs.Empty);
        }
        if (Keyboard.current.rightArrowKey.IsPressed())
        {
            float turnSpeed = -100f;
            playerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);;
            OnRightForce?.Invoke(this, EventArgs.Empty);
        }   
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            //Landed on terrain.
            Debug.Log("Crashed on terrain");
            return;
        }
        float softLandingVelocityMagnitude = 4f;
        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
        if (relativeVelocityMagnitude > softLandingVelocityMagnitude)
        {
            //Landed too hard.\
            Debug.Log("Landed too hard");
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = .90f;
        if (dotVector < minDotVector)
        {
            Debug.Log("Landed on a too steep angle");
            return;
        }
        Debug.Log("Succesful landing");
        float maxScoreAmountLandingAngle = 100f;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier* maxScoreAmountLandingAngle;

        float maxScoreAmountForLandingSpeed = 100;
        float landingSpeedScore = (softLandingVelocityMagnitude - relativeVelocityMagnitude) * maxScoreAmountForLandingSpeed;
        Debug.Log($"LandingAngleScore {landingAngleScore}");
        Debug.Log($"LandingSpeedScore {landingSpeedScore}");

        int score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingPad.GetScoreMultiplier());
        
        OnLanded?.Invoke(this, new OnLandedEventArgs
        {
            onLandedScore = score,
        });

        Debug.Log($"Score {score}");
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.TryGetComponent(out FuelPickup fuelPickup))
        {
            float addFuelAmount = 10f;
            fuelAmount += addFuelAmount;
            if (fuelAmount > fuelAmountMax)
            {
                fuelAmount = fuelAmountMax;
            }
            fuelPickup.DestroySelf();
        }
        if (collider2D.gameObject.TryGetComponent(out CoinPickup coinPickup))
        {
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            coinPickup.DestroySelf();
        }
    }

    private void ConsumeFuel()
    {
        float fuelConsumptionAmount = 1f;
        fuelAmount -= fuelConsumptionAmount * Time.deltaTime;
    }

    public float GetSpeedX()
    {
        return playerRigidbody2D.linearVelocity.x;
    }
    public float GetSpeedY()
    {
        return playerRigidbody2D.linearVelocity.y;
    }

    public float GetFuelAmount()
    {
        return  fuelAmount;
    }

    public float GetFuelAmountNormalized()
    {
        return fuelAmount / fuelAmountMax;
    }
}

