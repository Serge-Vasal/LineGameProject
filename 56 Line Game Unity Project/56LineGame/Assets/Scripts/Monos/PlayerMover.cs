using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] MovementTouchPad movementTouchPad;    
    [SerializeField] GameObject gameOverDot;    
    [SerializeField] int gameOverDotAmount;
    private Rigidbody2D rBody;   
    private Collider2D playerCollider;
    private Vector2 newPosition;
    private Vector2 previousPosition;
    private Touch[] previousAllTouches;
    private int previousTouchCount;
    private Touch[] currentAllTouches;
    private Vector2 worldPositionDelta;
    private Vector2 clampedworldPositionDelta;
    private float minimumExtent;
    private float sqrMinimumExtent;
    private float partialExtent;
    private float skinWidth = 0.1f;
    private Vector2 movementThisStep;
    private Vector2 forceVector;



    private void Start()
    {       
        GameManager.Instance.playerDot = this.gameObject;
        rBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        previousTouchCount = 0;
        previousAllTouches= Input.touches;
        minimumExtent = playerCollider.bounds.extents.x;       
        sqrMinimumExtent = minimumExtent * minimumExtent;        
        partialExtent = minimumExtent * (1.0f - skinWidth);       
     
    }    

    private void FixedUpdate()
    {

        if (GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING)
        {
            worldPositionDelta = new Vector2(0.0f, 0.0f);
            if (Input.touchCount > 0)
            {                
                

                currentAllTouches = Input.touches;

                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (previousTouchCount > 0 && currentAllTouches[i].phase == TouchPhase.Moved)
                    {                        
                        for (int p = 0; p < previousTouchCount; p++)
                        {
                            if (previousAllTouches[p].fingerId == currentAllTouches[i].fingerId)
                            {
                                Vector2 prevTouchPosition = previousAllTouches[p].position;
                                Vector2 prevTouchPositionWorld = Camera.main.ScreenToWorldPoint(prevTouchPosition);
                                //Debug.Log("prevTouchPositionWorld  " + prevTouchPositionWorld);

                                Vector2 currTouchPosition = currentAllTouches[i].position;
                                Vector2 currTouchPositionWorld = Camera.main.ScreenToWorldPoint(currTouchPosition);
                                //Debug.Log("currTouchPositionWorld  " + currTouchPositionWorld);

                                worldPositionDelta = currTouchPositionWorld - prevTouchPositionWorld;
                            }
                        }
                    }
                }
            }

            clampedworldPositionDelta = new Vector2(worldPositionDelta.x, 0.0f);
            //Debug.Log("clampedworldPositionDelta  " + clampedworldPositionDelta);            

            previousAllTouches = currentAllTouches;
            previousTouchCount = Input.touchCount;

            if (GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING)
            {
                rBody.position += clampedworldPositionDelta;                
                //Debug.Log("rBody position " + rBody.position);      
            }

            movementThisStep = rBody.position - previousPosition;
            float movementSqrMagnitude = movementThisStep.sqrMagnitude;
            float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);            

            int layerMask = 1 << 8;
            layerMask = ~layerMask;
            RaycastHit2D hitInfo = Physics2D.Raycast(previousPosition, movementThisStep, movementMagnitude, layerMask);
            //Debug.Log(hitInfo.collider);
            if (hitInfo.collider != null)
            {
                if (hitInfo.collider.gameObject.tag == "Destroyer" || hitInfo.collider.gameObject.tag == "DestroyerStarter"||
                    hitInfo.collider.gameObject.tag == "DestroyerInteract")
                {
                    
                    //Debug.Log("Raycaster HIT WALL !!!!");
                    //rBody.position = hitInfo.point - (movementThisStep / movementMagnitude) * partialExtent;
                    //Debug.Log("rBody NEW CORRECTED position " + rBody.position);
                    //Debug.Log("clampedworldPositionDelta  " + clampedworldPositionDelta);                   
                    transform.position = hitInfo.point;                    
                    worldPositionDelta = new Vector3(0.0f, 0.0f, 0.0f);
                    previousPosition = new Vector2(0.0f, -2.5f);
                    //Debug.Log("previousPosition  "+previousPosition);
                    clampedworldPositionDelta = new Vector3(0.0f, 0.0f, 0.0f);
                    GameManager.Instance.StateToHitDestroyer();
                }
                //if (hitInfo.collider.gameObject.tag == "Friendly")
                //{

                //    //Vector2 forceVector = collision.collider.gameObject.transform.position-transform.position;
                //    //forceVector.Normalize();
                //    Rigidbody2D friendlyRBody = hitInfo.collider.gameObject.GetComponent<Rigidbody2D>();
                //    friendlyRBody.AddForceAtPosition(movementThisStep * 100f,hitInfo.point);
                //    Debug.Log("Added Force!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                //}
            }

            previousPosition = rBody.position;
        }                  
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Player Rigidbody velocity   " + movementThisStep);
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING)
        {
            if (collision.collider.gameObject.tag == "Destroyer" || collision.collider.gameObject.tag == "DestroyerStarter" ||
                collision.collider.gameObject.tag == "DestroyerInteract")
            {
                //Debug.Log("Hit OnCollisionEnter");
                //Debug.Log("clampedworldPositionDelta  " + clampedworldPositionDelta);

                //Debug.Log("Rigidbody position    " + rBody.position);
                previousPosition = new Vector2(0.0f, -2.5f);
                //Debug.Log("previousPosition  "+previousPosition);
                worldPositionDelta = new Vector3(0.0f, 0.0f, 0.0f);
                clampedworldPositionDelta = new Vector3(0.0f, 0.0f, 0.0f);
                GameManager.Instance.StateToHitDestroyer();
            }
            if (collision.collider.gameObject.tag == "Friendly")
            {
                //Vector2 forceVector = collision.collider.gameObject.transform.position-transform.position;
                //forceVector.Normalize();
                Rigidbody2D friendlyRBody = collision.collider.gameObject.GetComponent<Rigidbody2D>();
                forceVector = friendlyRBody.position - rBody.position;
                if (movementThisStep.x > 0.09f)
                {
                    friendlyRBody.AddForce(forceVector * movementThisStep * 200f);
                }
                else
                {
                    friendlyRBody.AddForce(forceVector * 30f);
                }
            }
        }
    }

}
