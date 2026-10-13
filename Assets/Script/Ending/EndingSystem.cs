using Assets.Script.Core.GameRoot;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EndingSystem
{
    private readonly GameData gameData;

    private readonly List<EndingData> endings;

    public EndingSystem(GameData gameData)
    {
        this.gameData = gameData;


        TextAsset file = Resources.Load<TextAsset>("Data/endings");

        if (file == null)
            throw new InvalidOperationException(
                "Không tìm thấy Resources/Data/endings.json.");

        EndingConfigData config =
            JsonUtility.FromJson<EndingConfigData>(file.text);

        if (config == null || config.endings == null)
            throw new InvalidOperationException(
                "endings.json chưa có danh sách endings hợp lệ.");

        endings = config.endings;

        foreach (EndingData ending in endings)
        {
            if (ending == null)
                throw new InvalidOperationException(
                    "Danh sách endings có phần tử null.");

            if (!string.IsNullOrWhiteSpace(ending.imagePath))
            {
                ending.endingImage =
                    Resources.Load<Sprite>(ending.imagePath);
            }
        }
    }

    public EndingData GetEnding(EndingId endingId)
    {
        foreach (EndingData ending in endings)
        {
            if (ending.endingId == endingId)
                return ending;
        }

        throw new InvalidOperationException(
            $"Chưa cấu hình ending {endingId} trong endings.json.");
    }

    public EndingData CheckEnding()
    {
        if (gameData.permanentHealth < 1)
            return GetEnding(EndingId.DeathEnding);

        if (gameData.consecutiveUnpaidDays >= 3)
            return GetEnding(EndingId.DebtEnding);

        if (gameData.currentDay == 7 && !gameData.isSealBroken)
            return GetEnding(EndingId.NormalEnding);

        return null;
    }
}