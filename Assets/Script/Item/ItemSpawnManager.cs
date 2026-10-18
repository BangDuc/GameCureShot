using Assets.Script.Core.GameRoot;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager
{
    private readonly GameData gameData;

    private readonly List<ItemData> spawnableItems = new();
    private readonly List<ItemSpawnPoint> spawnPoints = new();

    private static ItemSpawnConfigData spawnConfig;

    public ItemSpawnManager(GameData gameData)
    {
        this.gameData = gameData;
    }

    public static List<string> GetSpawnableItemIds()
    {
        if (spawnConfig != null)
            return spawnConfig.spawnableItemIds;

        TextAsset file =
            Resources.Load<TextAsset>("Data/itemSpawns");

        if (file == null)
        {
            throw new System.InvalidOperationException(
                "Không tìm thấy Resources/Data/itemSpawns.json.");
        }

        ItemSpawnConfigData loaded =
            JsonUtility.FromJson<ItemSpawnConfigData>(file.text);

        if (loaded == null || loaded.spawnableItemIds == null)
        {
            throw new System.InvalidOperationException(
                "Cấu hình itemSpawns không có danh sách spawnableItemIds.");
        }

        spawnConfig = loaded;
        return spawnConfig.spawnableItemIds;
    }
    public void InitializeSpawns()
    {

    }
    public void RegisterSpawnPoint(ItemSpawnPoint itemSpawnPoint)
    {

    }
    public void SpawnItems()
    {

    }
}
