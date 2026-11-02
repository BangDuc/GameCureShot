using Assets.Script.Core.GameRoot;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnPoint : MonoBehaviour
{
    [SerializeField] public string spawnPointId;
    [SerializeField] public List<string> allowedItemIds = new();

    public bool CanSpawn(string itemId)
    {
        return allowedItemIds.Count == 0
            || allowedItemIds.Contains(itemId); 
    }
}
