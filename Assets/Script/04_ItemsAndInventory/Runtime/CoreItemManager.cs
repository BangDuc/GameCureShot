using Bang.Lib.Singleton;
using UnityEngine;

namespace Core.Item{
    public class ItemManager : Singleton<ItemManager>
    {
        [SerializeField]public ItemLibrary LibraryItem;

        [SerializeField] public ItemInventory InventoryItem;

        [SerializeField] public ItemEffectLibrary ItemEffect;

    }
}

