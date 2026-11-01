using Assets.Script.Core.GamePlay;
using UnityEngine;

public class BloodEffect : MonoBehaviour, IItemEffect
{
    public bool Apply(ItemUseContext context)
    {
        //if (context == null || context.runController == null)
        //    return false;

        //BuffState buffs = context.runController.Buffs;

        //if (buffs.hasBlood || buffs.bloodUsed)
        //    return false;

        //buffs.hasBlood = true;
        Debug.Log("da dung health");
        return true;
    }
}
