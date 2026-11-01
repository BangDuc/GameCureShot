using UnityEngine;

public class UnlockByItem_Equiped : UnlockByItem_Base
{
    [SerializeField]FlexibleItemFlyTransition EquipedByDOTWeen;

    public override void TuongTac()
    {
        base.TuongTac();
        EquipedByDOTWeen.PlayTransition();
    }
}
