using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button continueButton;

    private void Start()
    {
        continueButton.onClick.AddListener(HandleContinueClicked);
    }

    private void HandleContinueClicked()
    {
        GameManager.Instance.TogglePause();
    }

}
