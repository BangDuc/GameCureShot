using System;
using System.IO;
using UnityEngine;

public class ItemDataAuthoring : MonoBehaviour
{
    [SerializeField]
    private ItemConfigData data = new ItemConfigData();
#if UNITY_EDITOR
    [ContextMenu("Export JSON")]
    private void ExportJson()
    {
        string folder = Path.Combine(
            Application.dataPath, "Resources", "Data");

        Directory.CreateDirectory(folder);
        string path = Path.Combine(folder, "items.json");
        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(path, json);

        UnityEditor.AssetDatabase.Refresh();
        Debug.Log($"Đã xuất: {path}", this);
    }
#endif
}