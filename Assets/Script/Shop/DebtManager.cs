using Assets.Script.Core.GameRoot;
using System;
using UnityEngine;

public class DebtManager
{
    private readonly GameData gameData;
    private readonly DebtData debtData;

    public DebtManager(GameData gameData)
    {
        if (gameData == null)
            throw new ArgumentNullException(nameof(gameData));

        if (debtData == null)
            GetDebtData();

        if (debtData.dailyDebts == null)
            throw new ArgumentException(
                "DebtData.dailyDebts chưa được khởi tạo.", nameof(debtData));

        this.gameData = gameData;
    }
    public int GetRequiredDebt(int day)
    {
        if (day < 1)
            throw new ArgumentOutOfRangeException(nameof(day));

        foreach (DailyDebtData dailyDebt in debtData.dailyDebts)
        {
            if (dailyDebt == null || dailyDebt.day != day)
                continue;

            if (dailyDebt.requiredAmount < 0)
                throw new InvalidOperationException(
                    $"Định mức nợ của ngày {day} phải từ 0 trở lên.");

            return dailyDebt.requiredAmount;
        }

        throw new InvalidOperationException(
            $"Chưa cấu hình nợ cho ngày {day}.");
    }

    public DebtResult PayDailyDebt()
    {
        if (gameData.money < 0 || gameData.outstandingDebt < 0 ||
            gameData.consecutiveUnpaidDays < 0)
        {
            throw new InvalidOperationException(
                "Tiền, nợ tồn và số ngày chưa trả đủ phải từ 0 trở lên.");
        }

        int requiredAmount = checked(
            GetRequiredDebt(gameData.currentDay) + gameData.outstandingDebt);
        int paidAmount = Math.Min(gameData.money, requiredAmount);
        int remainingDebt = requiredAmount - paidAmount;
        int unpaidDays = remainingDebt > 0
            ? checked(gameData.consecutiveUnpaidDays + 1)
            : 0;

        gameData.money -= paidAmount;
        gameData.outstandingDebt = remainingDebt;
        gameData.consecutiveUnpaidDays = unpaidDays;

        return new DebtResult
        {
            requiredAmount = requiredAmount,
            paidAmount = paidAmount,
            remainingDebt = remainingDebt
        };
    }

    public void AddDebt(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount));
        gameData.outstandingDebt = checked(gameData.outstandingDebt + amount);
    }

    public DebtData GetDebtData()
    {
        TextAsset debtFile = Resources.Load<TextAsset>("Data/debt");

        if (debtFile == null)
            throw new InvalidOperationException(
                "Không tìm thấy Assets/Resources/Data/debt.json.");

        DebtData debtData =
            JsonUtility.FromJson<DebtData>(debtFile.text);
        return debtData;
    }
}
