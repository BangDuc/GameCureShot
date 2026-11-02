using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Gắn script này lên GameObject bình thường
public class TargetLibrary : DataLibrary<TargetData>
{
    Dictionary<string, TargetData> _targets;
    private void Awake()
    {
        
        InitializeData();
    }

    public void InitializeData()
    {
        LoadCurrentFile();
        _targets = GetDictionary();
    }
    public TargetData GetTarget(string id)
    {
        if (string.IsNullOrEmpty(id) || _targets == null) return null;

        _targets.TryGetValue(id, out TargetData target);
        return target;
    }

    public bool HasTarget(string id)
    {
        if (string.IsNullOrEmpty(id) || _targets == null) return false;
        return _targets.ContainsKey(id);
    }

    public bool TryGetTarget(string id, out TargetData target)
    {
        if (string.IsNullOrEmpty(id) || _targets == null)
        {
            target = null;
            return false;
        }
        return _targets.TryGetValue(id, out target);
    }

    public List<TargetData> GetTargetsByType(TargetType type)
    {
        if (_targets == null) return new List<TargetData>();

        return _targets.Values
            .Where(t => t.TargetType == type)
            .ToList();
    }
    public List<TargetData> FindTargetsByName(string keyword)
    {
        if (string.IsNullOrEmpty(keyword) || _targets == null) return new List<TargetData>();

        return _targets.Values
            .Where(t => t.TargetName != null && t.TargetName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
            .ToList();
    }

    public List<TargetData> GetTargetsByRewardRange(int minReward, int maxReward)
    {
        if (_targets == null) return new List<TargetData>();

        return _targets.Values
            .Where(t => t.MoneyReward >= minReward && t.MoneyReward <= maxReward)
            .ToList();
    }
    public TargetData GetRandomTarget(TargetType? filterType = null)
    {
        if (_targets == null || _targets.Count == 0) return null;

        IEnumerable<TargetData> pool = _targets.Values;
        if (filterType.HasValue)
        {
            pool = pool.Where(t => t.TargetType == filterType.Value);
        }

        var list = pool.ToList();
        if (list.Count == 0) return null;

        int randomIndex = UnityEngine.Random.Range(0, list.Count);
        return list[randomIndex];
    }




    [ContextMenu("Save")]
    public void Save()
    {
        SaveCurrentFile();
    }
}