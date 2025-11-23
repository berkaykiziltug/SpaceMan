using System;
using Unity.Cinemachine;
using UnityEngine;

public class CinemachineCameraZoom2D : MonoBehaviour
{
    private const float NORMAL_ORTHO_SIZE = 10f;
    public static CinemachineCameraZoom2D Instance  { get; private set; }

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

    [SerializeField] private CinemachineCamera cinemachineCamera;
    private float targetOrthographicSize =10f;

    private void Update()
    {
        float zoomSpeed = 2f;
        cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize,
            targetOrthographicSize, zoomSpeed * Time.deltaTime);
    }

    public void SetTargetOrtographicSize(float targetOrthographicSize)
    {
        this.targetOrthographicSize = targetOrthographicSize;
    }

    public void SetNormalOrthoSize()
    {
        SetTargetOrtographicSize(NORMAL_ORTHO_SIZE);
    }
}
