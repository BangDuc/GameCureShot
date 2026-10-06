using Assets.Script.Core.GameRoot;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveSystem
{
    string folderPath = Path.Combine(Application.persistentDataPath, "Saves");
    public void Save(GameData data, int slotId)
    {
        string filePath = Path.Combine(folderPath, $"slot_{slotId}.json");
        Directory.CreateDirectory(folderPath);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }
    public GameData Load(int slotId)
    {
        string filePath = Path.Combine(folderPath, $"slot_{slotId}.json");
        if (!HasSave(slotId)) return null;
        string json = File.ReadAllText(filePath);
        GameData data = JsonUtility.FromJson<GameData>(json);
        return data; 
    }

    public void Delete(int slotId)
    {
        string filePath = Path.Combine(folderPath, $"slot_{slotId}.json");
        if (HasSave(slotId))
        {
            File.Delete(filePath);
        }
    }

    public bool HasSave(int slotId)
    {
        string filePath = Path.Combine(folderPath, $"slot_{slotId}.json");
        return File.Exists(filePath);
    }

    public List<SaveInfo> GetSaveInfos()
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
            string idText = filename.Substring(5);
            if (!int.TryParse(idText, out int slotId))
                continue;
            GameData data= Load(slotId);
            if (data == null)
                continue;
            SaveInfo info = new SaveInfo
            {
                slotId = slotId,
                saveName = $"Day" + data.currentDay,
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
}
