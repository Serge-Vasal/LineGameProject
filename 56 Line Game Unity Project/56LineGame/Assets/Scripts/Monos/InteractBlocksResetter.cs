using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBlocksResetter : MonoBehaviour
{
    private Vector2 defaultPosition;
    private Quaternion defaultRotation;

    private void Awake()
    {
        defaultPosition = transform.localPosition;        
        defaultRotation = transform.localRotation;
    }    

    private void OnEnable()
    {
        transform.localPosition = defaultPosition;
        transform.localRotation = defaultRotation;
    }


}
