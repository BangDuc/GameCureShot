using Assets.Script.Core.GameRoot;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Unity.XR.GoogleVr;

public static class SaveSystem
{
    private static string folderPath = Path.Combine(Application.persistentDataPath, "Saves");
    public static void Save(GameData data, int slotId)
    {
        string filePath = Path.Combine(folderPath, $"slot_{slotId}.json");
        Directory.CreateDirectory(folderPath);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
        for(int i = slotId+1; i < 8; i++)
        {
            if (HasSave(i)) Delete(i);
        }    
    }
    public static GameData Load(int slotId)
    {
        string filePath = Path.Combine(folderPath, $"slot_{slotId}.json");
        if (!HasSave(slotId)) return null;
        string json = File.ReadAllText(filePath);
        GameData data = JsonUtility.FromJson<GameData>(json);
        return data; 
    }

    public static void Delete(int slotId)
    {
        string filePath = Path.Combine(folderPath, $"slot_{slotId}.json");
        if (HasSave(slotId))
        {
            File.Delete(filePath);
        }
    }

    public static bool HasSave(int slotId)
    {
        string filePath = Path.Combine(folderPath, $"slot_{slotId}.json");
        return File.Exists(filePath);
    }

    public static List<SaveInfo> GetSaveInfos()
    {
        List<SaveInfo> list = new();
        if(!Directory.Exists(folderPath))
        {
            return list;
        }    
        string[] files = Directory.GetFiles(folderPath, "slot_*.json");
        foreach(string file in files)
        {
            string filename = Path.GetFileNameWithoutExtension(file);
            string[] idText = filename.Split("_");
            if (!int.TryParse(idText[1], out int slotId))
                continue;
            GameData data= Load(slotId);
            if (data == null)
                continue;
            SaveInfo info = new SaveInfo
            {
                slotId = slotId,
                saveName = slotId == 0 ? "Auto Save" : "Day "+slotId,
                day = data.currentDay,
                money = data.money,
                isSealBroken = data.isSealBroken,
                permanentHealth = data.permanentHealth,
                phase = data.currentPhase,
                timestamp = File.GetLastWriteTimeUtc(file),
                playTime = data.playtime
            };
            list.Add(info);
        }
        return list;
    }
    public static GameData GetLastSaveInfo()
    {
        int max = 0;
        string[] files = Directory.GetFiles(folderPath, "slot_*.json");
        foreach(string file in files)
        {
            string filename = Path.GetFileNameWithoutExtension(file);
            string[] idText = filename.Split("_");
            if (!int.TryParse(idText[1], out int slotId))
            {
                if (slotId > max) max = slotId;
            }
            else continue;
        }
        return Load(max);
    }
}
