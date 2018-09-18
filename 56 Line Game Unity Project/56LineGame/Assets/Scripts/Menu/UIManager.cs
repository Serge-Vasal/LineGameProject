using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private StarterMenu starterMenu;
    [SerializeField] private LoadingScreen loadingScreen;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private GameOverMenu gameOverMenu;
    [SerializeField] private Slider loadingBar;
    [SerializeField] private Button pauseInGameButton;
    [SerializeField] private Camera dummyCamera;    
    [SerializeField] private GameObject scoreBoardBG;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text gameOverScore;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }
    public void SetDummyCameraActive(bool active)
    {
        dummyCamera.gameObject.SetActive(active);
    }

    public void SetLoadingScreenActive(bool active)
    {
        loadingScreen.gameObject.SetActive(active);
    }

    public void UpdateLoadingBar(float value)
    {
        loadingBar.value = value;
    }

    public void UpdateScoreText(float score)
    {
        scoreText.text = Mathf.RoundToInt(score).ToString();
    }

    public void UpdateGameOverText(float score)
    {
        gameOverScore.text= "Your Score\n" + Mathf.RoundToInt(score).ToString();
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if (previousState == GameManager.GameState.LOADING && currentState == GameManager.GameState.PREGAME)
        {
            starterMenu.gameObject.SetActive(true);
            starterMenu.FadeIn();
        }
        if (previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)
        {            
            starterMenu.FadeOut();            
        }
        pauseInGameButton.gameObject.SetActive(currentState == GameManager.GameState.RUNNING);        
        pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);
        scoreBoardBG.SetActive(currentState == GameManager.GameState.RUNNING);
        scoreText.gameObject.SetActive(currentState == GameManager.GameState.RUNNING);

        if (previousState == GameManager.GameState.HITDESTROYER && currentState == GameManager.GameState.GAMEOVER)
        {
            pauseInGameButton.gameObject.SetActive(false);
            gameOverMenu.gameObject.SetActive(true);
        }

        if (previousState == GameManager.GameState.GAMEOVER && currentState == GameManager.GameState.RUNNING)
        {
            gameOverMenu.gameObject.SetActive(false);
            pauseInGameButton.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.LOADING)
        {
            return;
        }        
        UpdateLoadingBar(GameManager.Instance.LoadLevelOperation.progress);        
    }
}
