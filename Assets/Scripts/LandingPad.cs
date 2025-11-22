using UnityEngine;

public class LandingPad : MonoBehaviour
{
    [SerializeField] private float scoreMultiplier;

    public float GetScoreMultiplier()
    {
        return scoreMultiplier;
    }
}
