using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class DataLibrary<T> : MonoBehaviour where T : IIdentifiable
{
    [Header("Cấu Hình File")]
    [SerializeField] protected PathLocation location = PathLocation.Resources;
    [Tooltip("Đường dẫn file (không cần đuôi .json nếu ở Resources)")]
    [SerializeField] protected string jsonFileName = "Data/";



    [Header("Dữ Liệu")]
    public List<T> dataList = new();

    [ContextMenu("Load Selected File")]
    public virtual void LoadCurrentFile()
    {
        dataList = JsonDataService.LoadList<T>(jsonFileName, location);
        Debug.Log($"[{GetType().Name}] Đã load {dataList.Count} phần tử từ file: {jsonFileName}");
    }

    [ContextMenu("Save Selected File")]
    public virtual void SaveCurrentFile()
    {
        JsonDataService.SaveList(dataList, jsonFileName, location);
    }

    /// <summary>
    /// Chuyển đổi nhanh List sang Dictionary tra cứu theo ID
    /// </summary>
    public virtual Dictionary<string, T> GetDictionary()
    {
        var dict = new Dictionary<string, T>();
        foreach (var item in dataList)
        {
            if (item != null && !string.IsNullOrEmpty(item.Id) && !dict.ContainsKey(item.Id))
            {
                dict.Add(item.Id, item);
            }
        }
        return dict;
    }
}

public interface IIdentifiable
{
    string Id { get; }
}