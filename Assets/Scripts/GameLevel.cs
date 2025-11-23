using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [SerializeField] private int levelNumber;
    [SerializeField] private Transform playerStartPositionTransform;

    public int GetLevelNumber()
    {
        return levelNumber;
    }

    public Vector3 GetPlayerStartPosition()
    {
        return playerStartPositionTransform.position;
    }
}
