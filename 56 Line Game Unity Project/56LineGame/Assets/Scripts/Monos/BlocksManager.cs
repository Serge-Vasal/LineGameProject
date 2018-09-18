using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksManager : MonoBehaviour
{
    [SerializeField] private Transform blocksMover;    
    [SerializeField] private int amountOfActiveBlocks;
    [SerializeField] private int amountOfGameOverDots;

    [SerializeField] GameObject[] starterBlocksPrefabs;
    [SerializeField] GameObject[] mainBlocksPrefabs;    
    private List<GameObject> starterBlocksReserve;
    private List<GameObject> mainBlocksReserve;
    private List<GameObject> starterDeadBlocks;
    private List<GameObject> mainDeadBlocks;

    private List<GameObject> activeBlocksList;     

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);        
        activeBlocksList = new List<GameObject>();
        starterDeadBlocks = new List<GameObject>();
        mainDeadBlocks = new List<GameObject>();

        starterBlocksReserve = new List<GameObject>();
        for (int i = 0; i < starterBlocksPrefabs.Length; i++)
        {
            GameObject obj = (GameObject)Instantiate(starterBlocksPrefabs[i]);
            obj.SetActive(false);
            starterBlocksReserve.Add(obj);
        }

        mainBlocksReserve = new List<GameObject>();
        for (int i = 0; i < mainBlocksPrefabs.Length; i++)
        {
            GameObject obj = (GameObject)Instantiate(mainBlocksPrefabs[i]);
            obj.SetActive(false);
            mainBlocksReserve.Add(obj);
        }           
    }    

    private void StartNewGame()
    {
        GameObject obj;
        if (starterDeadBlocks.Count > 0)
        {
            obj = starterDeadBlocks[Random.Range(0, starterDeadBlocks.Count)].gameObject;
            activeBlocksList.Add(obj);
            starterDeadBlocks.Remove(obj);
            obj.transform.parent = blocksMover.transform;
            obj.SetActive(true);
        }
        else
        {
            obj = starterBlocksReserve[Random.Range(0, starterBlocksReserve.Count)].gameObject;
            activeBlocksList.Add(obj);
            starterBlocksReserve.Remove(obj);
            obj.transform.parent = blocksMover.transform;
            obj.SetActive(true);
        }      

        for(int i = 0; i < amountOfActiveBlocks-1; i++)
        {
            if (mainBlocksReserve.Count > 0)
            {
                obj = mainBlocksReserve[Random.Range(0, mainBlocksReserve.Count)].gameObject;
                activeBlocksList.Add(obj);
                mainBlocksReserve.Remove(obj);
            }
            else
            {
                obj = mainDeadBlocks[Random.Range(0, mainDeadBlocks.Count)].gameObject;                
                activeBlocksList.Add(obj);
                mainDeadBlocks.Remove(obj);
            }
            
            
            obj.transform.parent = activeBlocksList[i].gameObject.transform;
            obj.transform.localPosition = new Vector3(0.0f, 19.2f, 0.0f);
            obj.SetActive(true);
        }
        Debug.Log("activeBlocksListCount  " + activeBlocksList.Count);
        Debug.Log("mainBlocksReserveListCount  " + mainBlocksReserve.Count);
        Debug.Log("mainDeadBlocksListCount  " + mainDeadBlocks.Count);
    }

    public void UpdateBlocks()
    {
        activeBlocksList[1].gameObject.transform.parent = blocksMover.transform;
        activeBlocksList[0].gameObject.SetActive(false);

        if (activeBlocksList[0].gameObject.tag == "DestroyerStarter")
        {
            starterDeadBlocks.Add(activeBlocksList[0].gameObject);
            activeBlocksList.RemoveAt(0);
        }
        else if (activeBlocksList[0].gameObject.tag == "Destroyer")
        {
            mainDeadBlocks.Add(activeBlocksList[0].gameObject);
            activeBlocksList.RemoveAt(0);
        }
        else if (activeBlocksList[0].gameObject.tag == "DestroyerInteract")
        {
            mainDeadBlocks.Add(activeBlocksList[0].gameObject);
            activeBlocksList.RemoveAt(0);            
        }           

        if (mainBlocksReserve.Count > 0)
        {
            GameObject obj = mainBlocksReserve[Random.Range(0, mainBlocksReserve.Count)].gameObject;
            activeBlocksList.Add(obj);
            mainBlocksReserve.Remove(obj);
            obj.transform.parent = activeBlocksList[activeBlocksList.Count-2].gameObject.transform;
            obj.transform.localPosition = new Vector3(0.0f, 19.2f, 0.0f);
            obj.SetActive(true);
        }
        else
        {
            GameObject obj = mainDeadBlocks[Random.Range(0, mainDeadBlocks.Count)].gameObject;
            activeBlocksList.Add(obj);
            mainDeadBlocks.Remove(obj);
            obj.transform.parent = activeBlocksList[activeBlocksList.Count - 2].gameObject.transform;
            obj.transform.localPosition = new Vector3(0.0f, 19.2f, 0.0f);
            obj.SetActive(true);
        }
        //Debug.Log("UPDATED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        //Debug.Log("activeBlocksListCount  " + activeBlocksList.Count);
        //Debug.Log("mainBlocksReserveListCount  " + mainBlocksReserve.Count);
        //Debug.Log("starterDeadBlocksListCount  " + starterDeadBlocks.Count);
        //Debug.Log("mainDeadBlocksListCount  " + mainDeadBlocks.Count);
        //Debug.Log("ActiveBlocks 0  " + activeBlocksList[0]);
        //Debug.Log("ActiveBlocks 1  " + activeBlocksList[1]);
        //Debug.Log("ActiveBlocks 2  " + activeBlocksList[2]);        
    }   

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if (previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)
        {            
            StartNewGame();
        }        

        if (previousState == GameManager.GameState.HITDESTROYER && currentState == GameManager.GameState.GAMEOVER)
        {                       
            for (int i =0;i< activeBlocksList.Count; i++)
            {               
                GameObject obj = activeBlocksList[i].gameObject;
                if (obj.tag == "DestroyerStarter")
                {
                    starterDeadBlocks.Add(obj);
                    
                    obj.SetActive(false);
                }
                else 
                {
                    mainDeadBlocks.Add(obj);
                    
                    obj.SetActive(false);
                }                 
            }
            activeBlocksList.Clear();       
        }

        if (previousState == GameManager.GameState.GAMEOVER && currentState == GameManager.GameState.RUNNING)
        {            
            StartNewGame();
        }
    }

}
