using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSize : MonoBehaviour    
{
    [SerializeField] private Camera cam;   

    void Awake ()
    {
        cam.orthographicSize = 10.8f * Screen.height / Screen.width * 0.5f;
    }
}
