using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{    
    private string currentLevelName = string.Empty;
    private List<GameObject> instancedSystemPrefabs;
    private AsyncOperation ao;
    private GameState currentGameState = GameState.LOADING;
    private float score;

    public GameObject explosion;
    public BlocksMover blocksMover;
    public GameObject playerDot;
    public GameObject[] SystemPrefabs;
    public enum GameState
    {
        LOADING,
        PREGAME,
        RUNNING,
        PAUSED,
        HITDESTROYER,
        GAMEOVER
    }
    public Events.EventGameState OnGameStateChanged;

    public AsyncOperation LoadLevelOperation
    {
        get { return ao; }
    }

    public GameState CurrentGameState
    {
        get { return currentGameState; }
        private set { currentGameState = value; }
    }

    private void Start()
    {
        score = 0f;
        DontDestroyOnLoad(gameObject);        
        instancedSystemPrefabs = new List<GameObject>();        
        InstantiateSystemPrefabs();
        LoadLevel("MainScene");        
    }

    void OnLoadOperationComplete(AsyncOperation ao)
    {
        UpdateState(GameState.PREGAME);
        UIManager.Instance.UpdateLoadingBar(GameManager.Instance.LoadLevelOperation.progress);
    }        

    void OnUnloadOperationComplete(AsyncOperation ao)
    {
        Debug.Log("Unload Complete");
    }

    void UpdateState(GameState state)
    {
        GameState previousGameState = currentGameState;
        currentGameState = state;

        switch (currentGameState)
        {
            case GameState.LOADING:
                Time.timeScale = 1.0f;
                Debug.Log(GameManager.Instance.CurrentGameState);
                break;

            case GameState.PREGAME:
                Time.timeScale = 1.0f;
                Debug.Log(GameManager.Instance.CurrentGameState);
                break;

            case GameState.RUNNING:
                Time.timeScale = 1.0f;
                Debug.Log(GameManager.Instance.CurrentGameState);
                break;

            case GameState.PAUSED:
                Time.timeScale = 0.0f;
                Debug.Log(GameManager.Instance.CurrentGameState);
                break;

            case GameState.HITDESTROYER:
                Time.timeScale = 1.0f;
                
                Debug.Log(GameManager.Instance.CurrentGameState+" Game State");
                break;

            case GameState.GAMEOVER:
                Time.timeScale = 1.0f;
                UIManager.Instance.UpdateGameOverText(score);
                Debug.Log(GameManager.Instance.CurrentGameState);
                break;

            default:
                break;
        }
        OnGameStateChanged.Invoke(currentGameState, previousGameState);
    }

    void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        for(int i = 0; i < SystemPrefabs.Length; ++i)
        {
            prefabInstance=Instantiate(SystemPrefabs[i]);
            instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    public void LoadLevel(string levelName)
    {
        ao=SceneManager.LoadSceneAsync(levelName,LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to load level " + levelName);
            return;
        }
        ao.completed += OnLoadOperationComplete;        
        currentLevelName = levelName;
    }

    public void UnloadLevel(string levelName)
    {
        AsyncOperation ao=SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to unload level " + levelName);
            return;
        }
        ao.completed += OnUnloadOperationComplete;
    }

    public void StartGame()
    {
        score = 0f;
        UIManager.Instance.UpdateScoreText(score);
        playerDot.SetActive(true);        
        UpdateState(GameState.RUNNING);        
    }

    public void TogglePause()
    {
        UpdateState(currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
    }

    public void StateToHitDestroyer()
    {
        explosion.transform.position = playerDot.transform.position;        
        playerDot.transform.position = new Vector2(0.0f, -2.5f);
        explosion.SetActive(true);        
        playerDot.SetActive(false);
        
        UpdateState(GameState.HITDESTROYER);
        StartCoroutine(ExplosionFinalMovement());
    }

    public void StateToGameOver()
    {
        explosion.SetActive(false);
        UpdateState(GameState.GAMEOVER);
    }    

    IEnumerator ExplosionFinalMovement()
    {
        float time = 0.5f;
        while (time <= 1f)
        {
            explosion.transform.position += new Vector3(0.0f, blocksMover.blocksMoverYPositionCoroutineDelta, 0.0f);
            time += Time.deltaTime/4.0f;
            yield return new WaitForFixedUpdate();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        for(int i = 0; i < instancedSystemPrefabs.Count; ++i)
        {
            Destroy(instancedSystemPrefabs[i]);
        }
        instancedSystemPrefabs.Clear();
    }

    private void FixedUpdate()
    {
        if (currentGameState == GameState.RUNNING)
        {
            score += Time.deltaTime;
            UIManager.Instance.UpdateScoreText(score);    
        }        
    }
}
