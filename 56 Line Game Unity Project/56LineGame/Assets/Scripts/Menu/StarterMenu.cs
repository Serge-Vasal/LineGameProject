using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarterMenu : MonoBehaviour
{
    [SerializeField] private Animation starterMenuAnimator;
    [SerializeField] private AnimationClip fadeOutAnimation;
    [SerializeField] private AnimationClip fadeInAnimation;
    [SerializeField] private Button StartButton;    

    private void Start()
    {       
        StartButton.onClick.AddListener(HandleStartClicked);        
    }

    public void OnFadeOutComplete()
    {
        gameObject.SetActive(false);
    }

    public void OnFadeInComplete()
    {
        UIManager.Instance.SetLoadingScreenActive(false);
    }

    public void FadeIn()
    {
        starterMenuAnimator.Stop();
        starterMenuAnimator.clip = fadeInAnimation;
        starterMenuAnimator.Play();
    }

    public void FadeOut()
    {
        UIManager.Instance.SetDummyCameraActive(false);
        starterMenuAnimator.Stop();
        starterMenuAnimator.clip = fadeOutAnimation;
        starterMenuAnimator.Play();
    }

    private void HandleStartClicked()
    {
        GameManager.Instance.StartGame();               
    }
}
