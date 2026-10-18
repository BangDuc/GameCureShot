using Assets.Script.Core.GamePlay;
using UnityEngine;

public class CoinEffect : MonoBehaviour, IItemEffect
{
    public bool Apply(ItemUseContext context)
    {
        if (context == null || context.runController == null)
            return false;

        BuffState buffs = context.runController.Buffs;

        if (buffs.hasCoin && !buffs.coinUsed)
            return false;

        buffs.hasCoin = true;
        buffs.coinUsed = false;

        return true;
    }
}