using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EndingRule
{
    private readonly List<ICondition> conditions;

    public EndingRule(List<ICondition> conditions)
    {
        this.conditions = conditions;
    }

    public bool Evaluate()
    {
        if (conditions.Count == 0)
            return false;

        foreach (ICondition condition in conditions)
        {
            if (condition == null || !condition.Evaluate())
                return false;
        }

        return true;
    }
}
