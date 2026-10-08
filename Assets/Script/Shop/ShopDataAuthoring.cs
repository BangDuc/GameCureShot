using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShopDataAuthoring : MonoBehaviour
{
    [SerializeField]
    private ShopConfigData data = new ShopConfigData();

#if UNITY_EDITOR
    [ContextMenu("Export JSON")]
    private void ExportJson()
    {
        string folder = Path.Combine(
            Application.dataPath, "Resources", "Data");

        Directory.CreateDirectory(folder);

        string path = Path.Combine(folder, "shops.json");
        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(path, json);

        UnityEditor.AssetDatabase.Refresh();
        Debug.Log($"Đã xuất: {path}", this);
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