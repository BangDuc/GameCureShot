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

            NormalizeEndingId(ending);

            if (!string.IsNullOrWhiteSpace(ending.imagePath))
            {
                ending.endingImage =
                    Resources.Load<Sprite>(ending.imagePath);
            }
        }
    }

    private void NormalizeEndingId(EndingData ending)
    {
        if (string.IsNullOrWhiteSpace(ending.endingType)) return;

        string type = ending.endingType.Trim();
        if (type.Equals("Debt", StringComparison.OrdinalIgnoreCase) ||
            type.Equals("DebtEnding", StringComparison.OrdinalIgnoreCase))
        {
            ending.endingId = EndingId.DebtEnding;
        }
        else if (type.Equals("Death", StringComparison.OrdinalIgnoreCase) ||
                 type.Equals("DeathEnding", StringComparison.OrdinalIgnoreCase))
        {
            ending.endingId = EndingId.DeathEnding;
        }
        else if (type.Equals("To Be Continued", StringComparison.OrdinalIgnoreCase) ||
                 type.Equals("ToBeContinued", StringComparison.OrdinalIgnoreCase) ||
                 type.Equals("ToBeContinuedEnding", StringComparison.OrdinalIgnoreCase))
        {
            ending.endingId = EndingId.ToBeContinuedEnding;
        }
        else if (type.Equals("Normal", StringComparison.OrdinalIgnoreCase) ||
                 type.Equals("NormalEnding", StringComparison.OrdinalIgnoreCase))
        {
            ending.endingId = EndingId.NormalEnding;
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
        if (gameData == null) return null;

        Debug.Log($"<color=cyan>[EndingSystem] Kiểm tra điều kiện Ending - Ngày: {gameData.currentDay}, Máu vĩnh viễn: {gameData.permanentHealth}/4, Nợ chưa trả liên tiếp: {gameData.consecutiveUnpaidDays} ngày, Nợ tồn: ${gameData.outstandingDebt}</color>");

        if (gameData.permanentHealth < 1)
        {
            Debug.Log("<color=red>[EndingSystem] Người chơi hết máu vĩnh viễn (permanentHealth < 1) -> Kích hoạt DeathEnding!</color>");
            return GetEnding(EndingId.DeathEnding);
        }

        if (gameData.consecutiveUnpaidDays >= 3)
        {
            Debug.Log($"<color=magenta>[EndingSystem] Người chơi chưa trả nợ liên tiếp {gameData.consecutiveUnpaidDays} ngày (>= 3) -> Kích hoạt DebtEnding!</color>");
            return GetEnding(EndingId.DebtEnding);
        }

        if (gameData.currentDay >= 7 && !gameData.isSealBroken)
        {
            Debug.Log("<color=green>[EndingSystem] Đã hoàn thành 7 ngày và phong ấn chưa bị phá vỡ -> Kích hoạt NormalEnding!</color>");
            return GetEnding(EndingId.NormalEnding);
        }

        return null;
    }
}
