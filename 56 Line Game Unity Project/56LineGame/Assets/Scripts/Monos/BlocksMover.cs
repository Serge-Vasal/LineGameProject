using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksMover : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    private float yDeltaRaw;

    public float blocksMoverYPositionCoroutineDelta;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        GameManager.Instance.blocksMover = this;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING)
        {
            yDeltaRaw = Time.deltaTime * scrollSpeed;
            float yDelta = yDeltaRaw *= -1.0f;    
            transform.position += new Vector3(0.0f, yDelta,0.0f);           
        }
        
    }

    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if (previousState == GameManager.GameState.RUNNING && currentState == GameManager.GameState.HITDESTROYER)
        {            
            StartCoroutine(SmoothStopBlocks(4f));
        }

        if (previousState == GameManager.GameState.GAMEOVER && currentState == GameManager.GameState.RUNNING)
        {
            transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }

    IEnumerator SmoothStopBlocks(float duration)
    {
        float time = 0.5f;
        while (time <= 1f)
        {            
            yDeltaRaw = Time.deltaTime * scrollSpeed*(time-time*time);
            blocksMoverYPositionCoroutineDelta = yDeltaRaw *= -1.0f;
            transform.position += new Vector3(0.0f, blocksMoverYPositionCoroutineDelta, 0.0f);            
            time += Time.deltaTime / duration;
            if (time >= 0.99f)
            {
                GameManager.Instance.StateToGameOver();
                StopAllCoroutines();
            }            
            yield return new WaitForFixedUpdate();
        }  
    }

}
