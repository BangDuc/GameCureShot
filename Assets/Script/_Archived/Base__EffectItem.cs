using UnityEngine;

public class Base__EffectItem : MonoBehaviour, IItemEffect
{

    public bool Apply(ItemUseContext context)
    {
        Debug.Log("SuDung Item");
        return true;
    }

    
}
