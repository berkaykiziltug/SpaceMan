using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{

    private const float GRAVITY_NORMAL = 0.7f;
    public static Player Instance { get; private set; }
    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnBeforeForce;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
        
    }
    public event EventHandler OnCoinPickup;

    public event EventHandler<OnLandedEventArgs> OnLanded;

    public class OnLandedEventArgs : EventArgs
    {
        public LandingType landingType;
        public float dotVector;
        public float landingSpeed;
        public float scoreMultiplier;
        public int onLandedScore;
    }

    public enum LandingType
    {
        Success,
        WrongLandingArea,
        TooSteepAngle,
        TooFastLanding,
    }

    public enum State
    {
        WaitingToStart,
        Normal,
        GameOver,
    }
    
    private Rigidbody2D playerRigidbody2D;
    private float fuelAmount;
    private float fuelAmountMax = 10f;
    private State state;
    
    
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
        state =State.WaitingToStart;
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerRigidbody2D.gravityScale = 0;
    }



    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);
        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (GameInput.Instance.IsUpActionPressed()||
                    GameInput.Instance.IsLeftActionPressed() ||
                    GameInput.Instance.IsRightActionPressed())
                {
                    //Pressing any key.
                    ConsumeFuel();
                    playerRigidbody2D.gravityScale = GRAVITY_NORMAL;
                    SetState(State.Normal);
                }

                break;
            case State.Normal:
                if (fuelAmount <= 0)
                {
                    fuelAmount = 0;
                    return;
                }

                if (GameInput.Instance.IsUpActionPressed()||
                    GameInput.Instance.IsLeftActionPressed() ||
                    GameInput.Instance.IsRightActionPressed())
                {
                    ConsumeFuel();
                }

                if (GameInput.Instance.IsUpActionPressed())
                {
                    float force = 700f;
                    playerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
                    OnUpForce?.Invoke(this, EventArgs.Empty);
                }

                if (GameInput.Instance.IsLeftActionPressed())
                {
                    float turnSpeed = +100f;
                    playerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
                    ;
                    OnLeftForce?.Invoke(this, EventArgs.Empty);
                }

                if  (GameInput.Instance.IsRightActionPressed())
                {
                    float turnSpeed = -100f;
                    playerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
                    ;
                    OnRightForce?.Invoke(this, EventArgs.Empty);
                }

                break;
            case State.GameOver:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            //Landed on terrain.
            Debug.Log("Crashed on terrain");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.WrongLandingArea,
                dotVector = 0,
                landingSpeed = 0f,
                scoreMultiplier = 0,
                onLandedScore = 0,
            });
            SetState(State.GameOver);
            return;
        }
        float softLandingVelocityMagnitude = 4f;
        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
        if (relativeVelocityMagnitude > softLandingVelocityMagnitude)
        {
            //Landed too hard.\
            Debug.Log("Landed too hard");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooFastLanding,
                dotVector = 0,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
                onLandedScore = 0,
            });
            SetState(State.GameOver);
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = .90f;
        if (dotVector < minDotVector)
        {
            Debug.Log("Landed on a too steep angle");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooSteepAngle,
                dotVector = dotVector,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
                onLandedScore = 0,
            });
            SetState(State.GameOver);
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
            landingType = LandingType.Success,
            dotVector = dotVector,
            landingSpeed = relativeVelocityMagnitude,
            scoreMultiplier = landingPad.GetScoreMultiplier(),
            onLandedScore = score,
        });
        
        SetState(State.GameOver);
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

    private void SetState(State state)
    {
        this.state = state;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
        {
            state =state
        });
    }
}

