using Core.Item;
using UnityEngine;
using UnityEngine.Rendering;

public class UsingItem : MonoBehaviour
{
    [SerializeField] public PlayerManager player;
    [SerializeField] public InventoryItemData item;
    [SerializeField] public RunController runController;

    [SerializeField] public Core.Item.ItemManager itemManager;
    private ItemUseContext context;

    private void Start()
    {
        runController = RunController.Instance;
        player = PlayerManager.Instance;
        context = new ItemUseContext(item, player, runController);
        itemManager= Core.Item.ItemManager.Instance;
        itemManager.InventoryItem.OnUsing += OnUsingItem;

    }
    public void OnUsingItem(InventoryItemData itemUsing)
    {
        if (itemUsing.itemEffect == null) 
        {
            return;
        }
        
        itemUsing.itemEffect.ApplyEffect(context);
    }
}
