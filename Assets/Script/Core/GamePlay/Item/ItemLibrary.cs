using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemLibrary : DataLibrary<ItemData>
{
    private Dictionary<string, ItemData> _items;

    private void Awake()
    {
        InitializeData();
    }

    /// <summary>
    /// Load dữ liệu từ file JSON và build cache Dictionary để tra cứu O(1)
    /// </summary>
    public void InitializeData()
    {
        LoadCurrentFile();
        _items = GetDictionary();
    }

    /// <summary>
    /// Lấy thông tin Item theo itemId (trả về null nếu không tìm thấy)
    /// </summary>
    public ItemData GetItem(string id)
    {
        if (string.IsNullOrEmpty(id) || _items == null) return null;

        _items.TryGetValue(id, out ItemData item);
        return item;
    }

    /// <summary>
    /// Kiểm tra xem Item có tồn tại trong thư viện hay không
    /// </summary>
    public bool HasItem(string id)
    {
        if (string.IsNullOrEmpty(id) || _items == null) return false;
        return _items.ContainsKey(id);
    }

    /// <summary>
    /// Safe getter trả về bool và out parameter
    /// </summary>
    public bool TryGetItem(string id, out ItemData item)
    {
        if (string.IsNullOrEmpty(id) || _items == null)
        {
            item = null;
            return false;
        }
        return _items.TryGetValue(id, out item);
    }

    /// <summary>
    /// Tìm kiếm danh sách Item theo từ khóa tên (không phân biệt hoa thường)
    /// </summary>
    public List<ItemData> FindItemsByName(string keyword)
    {
        if (string.IsNullOrEmpty(keyword) || _items == null) return new List<ItemData>();

        return _items.Values
            .Where(item => item.itemName != null && item.itemName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
            .ToList();
    }

    /// <summary>
    /// Tìm kiếm Item có mô tả chứa từ khóa cần tìm
    /// </summary>
    public List<ItemData> FindItemsByDescription(string keyword)
    {
        if (string.IsNullOrEmpty(keyword) || _items == null) return new List<ItemData>();

        return _items.Values
            .Where(item => item.description != null && item.description.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
            .ToList();
    }

    /// <summary>
    /// Lấy ngẫu nhiên 1 item trong thư viện
    /// </summary>
    public ItemData GetRandomItem()
    {
        if (_items == null || _items.Count == 0) return null;

        var list = _items.Values.ToList();
        int randomIndex = UnityEngine.Random.Range(0, list.Count);
        return list[randomIndex];
    }

    /// <summary>
    /// Lấy danh sách tất cả Item hiện có
    /// </summary>
    public List<ItemData> GetAllItems()
    {
        if (_items == null) return new List<ItemData>();
        return _items.Values.ToList();
    }

    [ContextMenu("Save")]
    public void Save()
    {
        SaveCurrentFile();
    }
}