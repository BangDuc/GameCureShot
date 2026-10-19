using Assets.Script.Core.GamePlay;
using UnityEngine;

public class CoinEffect : MonoBehaviour, IItemEffect
{
    public bool Apply(ItemUseContext context)
    {
        Debug.Log("da dung coin");
        return true;
    }
}