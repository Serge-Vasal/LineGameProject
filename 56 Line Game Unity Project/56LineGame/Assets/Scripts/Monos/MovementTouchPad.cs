using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MovementTouchPad : MonoBehaviour
{
    [SerializeField] Camera cam;
    private Vector2 screenPosition;
    private Vector2 worldPositionRaw;
    private Vector2 worldPositionX;

    private void Start()
    {
        worldPositionRaw = new Vector3(0.0f, -2.5f, 0.0f);
    }
    private void FixedUpdate()
    {
        if(Input.touchCount>0)
        {
            screenPosition = Input.GetTouch(0).position;
            worldPositionRaw = cam.ScreenToWorldPoint(screenPosition);
            worldPositionX = new Vector2(worldPositionRaw.x, 0.0f);
        }
        
    } 

    public Vector2 GetPositionDelta()
    {        
        return worldPositionX;
    }
}
