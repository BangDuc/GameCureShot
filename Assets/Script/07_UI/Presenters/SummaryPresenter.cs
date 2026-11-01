using Assets.Script.Core.GamePlay;
using Assets.Script.Core.GameRoot;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SummaryPresenter : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private UIButtonView continueButton;
    [SerializeField] private Transform earning;
    [SerializeField] private Transform debtSettlement;
    [SerializeField] private SummaryRowView rowPrefab;

    private readonly List<SummaryRowView> rows = new();

    private RunResult runResult;
    private DebtResult debtResult;
    private GameData gameData;

    public event Action Clicked;

    private void OnEnable()
    {
        continueButton.Clicked += HandleClickedButton;
    }

    private void OnDisable()
    {
        continueButton.Clicked -= HandleClickedButton;
    }

    public void Initialize(
        RunResult runResult,
        DebtResult debtResult,
        GameData gameData)
    {
        this.runResult = runResult;
        this.debtResult = debtResult;
        this.gameData = gameData;

        BuildSummary();
    }

    private void BuildSummary()
    {
        ClearRows();

        int seconds = Mathf.FloorToInt(
            Mathf.Max(0f, runResult.survivalTime));

        string survivalTime =
            $"{seconds / 60:00}:{seconds % 60:00}";

        AddRow(earning, "DAY", runResult.day.ToString());
        AddRow(earning, "PHOTOS TAKEN", runResult.photoTaken.ToString());
        AddRow(earning, "INCOME TODAY", $"${runResult.money}");
        AddRow(earning, "SURVIVAL TIME", survivalTime);
        AddRow(earning, "PERMANENT HEALTH",
            $"{gameData.permanentHealth}/4");
        AddRow(debtSettlement, "PREVIOUS DEBT",
            $"${debtResult.previousDebt}");

        AddRow(debtSettlement, "TODAY'S DEBT",
            $"${debtResult.dailyDebt}");
        AddRow(debtSettlement, "TOTAL TO PAY",
            $"${debtResult.requiredAmount}");

        AddRow(debtSettlement, "PAID",
            $"${debtResult.paidAmount}");

        AddRow(debtSettlement, "REMAINING DEBT",
            $"${debtResult.remainingDebt}");

        AddRow(debtSettlement, "MONEY AFTER PAYMENT",
            $"${gameData.money}");
    }

    private void AddRow(
        Transform parent,
        string label,
        string value)
    {
        SummaryRowView row = Instantiate(rowPrefab, parent, false);

        row.Render(label, value);
        row.gameObject.SetActive(true);

        rows.Add(row);
    }

    private void ClearRows()
    {
        foreach (SummaryRowView row in rows)
        {
            if (row == null)
                continue;

            row.gameObject.SetActive(false);
            Destroy(row.gameObject);
        }

        rows.Clear();
    }

    private void HandleClickedButton()
    {
        Clicked?.Invoke();
    }

    private void OnDestroy()
    {
        ClearRows();
    }

    [ContextMenu("Preview Summary")]
    private void PreviewSummary()
    {
        if (!Application.isPlaying)
            return;

        Initialize(
            new RunResult
            {
                day = 1,
                money = 300,
                photoTaken = 5,
                survivalTime = 125f
            },
            new DebtResult
            {
                previousDebt = 0,
                dailyDebt = 200,
                paidAmount = 200,
                remainingDebt = 0,
            },
            new GameData
            {
                currentDay = 1,
                money = 100,
                permanentHealth = 4
            });
    }
}