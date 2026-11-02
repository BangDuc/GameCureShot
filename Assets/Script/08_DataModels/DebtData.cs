using System;
using System.Collections.Generic;

[Serializable]
public class DebtData
{
    public List<DailyDebtData> dailyDebts = new();
}

[Serializable]
public class DailyDebtData
{
    public int day;
    public int requiredAmount;
}