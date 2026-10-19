using UnityEngine;

public interface IUnlockByItem
{
    public ItemData GetItemNeeded();
    public void Unlocked();


}
