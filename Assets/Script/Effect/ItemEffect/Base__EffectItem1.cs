using UnityEngine;

public class Base__EffectItem2 : MonoBehaviour, IItemEffect
{
    

    public bool Apply(ItemUseContext context)
    {
        Debug.Log("SuDung Hoi Mau");
        return true;
    }

    
}
