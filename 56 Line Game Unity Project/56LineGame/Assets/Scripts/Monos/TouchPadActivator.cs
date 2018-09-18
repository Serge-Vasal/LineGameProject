using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPadActivator : MonoBehaviour
{
    [SerializeField] private MovementTouchPad movementTouchPad;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        movementTouchPad.gameObject.SetActive(currentState == GameManager.GameState.RUNNING);
    }     
}
