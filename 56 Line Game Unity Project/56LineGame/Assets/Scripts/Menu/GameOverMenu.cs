using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private Button tryAgainButton;

    private void Start()
    {
        tryAgainButton.onClick.AddListener(HandleTryAgainClicked);
    }

    private void HandleTryAgainClicked()
    {
        GameManager.Instance.StartGame();
    }

}
