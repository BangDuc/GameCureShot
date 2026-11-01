using Assets.Script.Core.GamePlay;
using UnityEngine;

public class ShieldEffect : MonoBehaviour, IItemEffect
{
    public bool Apply(ItemUseContext context)
    {
        //if (context == null || context.runController == null)
        //    return false;

        //BuffState buffs = context.runController.Buffs;

        //if (buffs.hasShield && !buffs.shieldUsed)
        //    return false;

        //buffs.hasShield = true;
        //buffs.shieldUsed = false;
        Debug.Log("da dung shield");
        return true;
    }
}
