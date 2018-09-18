using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.explosion = this.gameObject;
        gameObject.SetActive(false);
    }


}
