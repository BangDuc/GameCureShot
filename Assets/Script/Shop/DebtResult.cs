using UnityEngine;

public class DebtResult
{
    public int previousDebt;
    public int dailyDebt;
    public int paidAmount;
    public int remainingDebt;

    public int requiredAmount => previousDebt + dailyDebt;
}