using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShopDataAuthoring : MonoBehaviour
{
    [Header("ITEMS")]
    [SerializeField]
    private ItemConfigData itemData = new ItemConfigData();

    [Header("SHOPS")]
    [SerializeField]
    private ShopConfigData shopData = new ShopConfigData();

    [Header("ITEM SPAWNS")]
    [SerializeField]
    private ItemSpawnConfigData spawnData = new ItemSpawnConfigData();

    [Header("DEBT")]
    [SerializeField]
    private DebtData debtData = new DebtData();

    [Header("ENDINGS")]
    [SerializeField] private EndingConfigData endingData = new();

#if UNITY_EDITOR
    [ContextMenu("Export All JSON")]
    private void ExportJson()
    {
        string folder = Path.Combine(
            Application.dataPath, "Resources", "Data");

        Directory.CreateDirectory(folder);

        WriteJson(folder, "items.json", itemData);
        WriteJson(folder, "shops.json", shopData);
        WriteJson(folder, "itemSpawns.json", spawnData);
        WriteJson(folder, "debt.json", debtData);
        WriteJson(folder, "endings.json", endingData);

        UnityEditor.AssetDatabase.Refresh();
        Debug.Log($"Đã xuất bốn file JSON vào: {folder}", this);
    }

    private void WriteJson(
        string folder, string fileName, object data)
    {
        string path = Path.Combine(folder, fileName);
        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(path, json);
    }
#endif

#if UNITY_EDITOR
    [ContextMenu("Import Items JSON")]
    private void ImportItemsJson()
    {
        string path = Path.Combine(
            Application.dataPath,
            "Resources",
            "Data",
            "items.json"
        );

        if (!File.Exists(path))
        {
            Debug.LogError($"Không tìm thấy file: {path}", this);
            return;
        }

        try
        {
            string json = File.ReadAllText(path);

            ItemConfigData imported =
                JsonUtility.FromJson<ItemConfigData>(json);

            if (imported == null || imported.items == null)
            {
                Debug.LogError(
                    "Không đọc được danh sách items trong JSON.", this);
                return;
            }

            // Ghi lại dữ liệu trước khi thay đổi để có thể Ctrl+Z.
            UnityEditor.Undo.RecordObject(this, "Import Items JSON");

            itemData = imported;

            if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(this))
            {
                UnityEditor.PrefabUtility
                    .RecordPrefabInstancePropertyModifications(this);
            }

            UnityEditor.EditorUtility.SetDirty(this);

            if (gameObject.scene.IsValid())
            {
                UnityEditor.SceneManagement.EditorSceneManager
                    .MarkSceneDirty(gameObject.scene);
            }

            Debug.Log(
                $"Đã nhập {imported.items.Count} item từ: {path}", this);
        }
        catch (System.Exception exception)
        {
            Debug.LogError(
                $"Import thất bại: {exception.Message}", this);
        }
    }
#endif
}

[Serializable]
public class ShopConfigData
{
    public List<ShopOfferData> normalItems =
        new List<ShopOfferData>();

    public List<ShopOfferData> orionItems =
        new List<ShopOfferData>();
}

[Serializable]
public class ItemSpawnConfigData
{
    public List<string> spawnableItemIds = new();
}