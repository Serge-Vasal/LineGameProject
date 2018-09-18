using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseInGameButton : MonoBehaviour
{
    [SerializeField] private Button pauseInGameButton;

    private void Start()
    {
        pauseInGameButton.onClick.AddListener(HandlePauseClicked);
    }

    private void HandlePauseClicked()
    {
        GameManager.Instance.TogglePause();
    }

}
