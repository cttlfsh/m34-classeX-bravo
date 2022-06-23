using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    public GameObject dollyTrack;
    public GameObject dollyCart;
    public GameObject mainCamera;
    public Transform dollyCamera;
    public bool isCinematicPlaying;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }
}
