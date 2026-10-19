using Core.Item;
using UnityEngine;

public class UnlockByItem_FadeOut : UnlockByItem_Base
{

    public override void TuongTac()
    {
        base.TuongTac();
        FadeOut();
    }
    public void FadeOut()
    {
        Destroy(this.gameObject);
    }
    
}
