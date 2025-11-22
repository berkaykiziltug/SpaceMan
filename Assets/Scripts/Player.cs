using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D playerRigidbody2D;
    [SerializeField] private float moveSpeed;
    private void Awake()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (Keyboard.current.upArrowKey.IsPressed())
        {
            float force = 700f;
            playerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
        }
        if (Keyboard.current.leftArrowKey.IsPressed())
        {
            float turnSpeed = +100f;
            playerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);;
        }
        if (Keyboard.current.rightArrowKey.IsPressed())
        {
            float turnSpeed = -100f;
            playerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);;
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

        Debug.Log($"Score {score}");
    }
}

