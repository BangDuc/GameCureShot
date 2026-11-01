using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WorldStateData
{
    public List<string> solvedPuzzleIds;
    public List<string> unlockedObjectIds;
    public List<string> collectedItemIds;
    public List<ItemSpawnData> itemSpawnLocations;
}
