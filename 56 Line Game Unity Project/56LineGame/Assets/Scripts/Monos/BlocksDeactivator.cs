using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksDeactivator : MonoBehaviour
{
    [SerializeField] private BlocksManager blocksManager;

    private string colliderName;

    private void Start()
    {
        colliderName = "Empty";
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name!=colliderName)
        {            
            blocksManager.UpdateBlocks();
        }
        colliderName = collider.name;
    }
    
}
