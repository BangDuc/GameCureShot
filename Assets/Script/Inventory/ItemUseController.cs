using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemUseController : MonoBehaviour
{
    [SerializeField] private InventoryPresenter presenter;
    [SerializeField] private List<ItemEffectContainer> effectPrefabs = new();

    private readonly List<ItemData> boundItems = new();

    private Inventory inventory;
    private ItemUseContext context;

    public void Initialize(
        Inventory inventory,
        ItemUseContext context)
    {
        this.inventory = inventory;
        this.context = context;

        InitializeEffects();
        presenter.Initialize(inventory);
    }

    private void InitializeEffects()
    {
        if (boundItems.Count > 0)
            return;

        foreach (ItemEffectContainer prefab in effectPrefabs)
        {
            if (prefab == null)
                continue;

            ItemData item = ItemManager.GetItemData(prefab.ItemId);

            if (item == null)
            {
                Debug.LogError(
                    $"Không tìm thấy ItemData: {prefab.ItemId}", this);
                continue;
            }

            ItemEffectContainer instance =
                Instantiate(prefab, transform, false);

            instance.gameObject.SetActive(true);

            item.effects = instance.Effects.ToList();
            item.passiveEffects = instance.PassiveEffects.ToList();

            boundItems.Add(item);
        }
    }

    private void OnEnable()
    {
        presenter.UseRequested += HandleUseRequested;
    }

    private void OnDisable()
    {
        presenter.UseRequested -= HandleUseRequested;
    }

    private void HandleUseRequested(InventoryItemData ownedItem)
    {
        if (inventory == null || context == null ||
            ownedItem == null || ownedItem.remainingUses <= 0 ||
            !inventory.Items.Contains(ownedItem))
        {
            return;
        }

        ItemData item = ItemManager.GetItemData(ownedItem.itemId);

        if (item == null || item.effects == null)
            return;

        context.item = ownedItem;

        bool applied = false;

        foreach (IItemEffect effect in item.effects)
        {
            if (effect != null && effect.Apply(context))
                applied = true;
        }

        if (applied)
            inventory.ConsumeUse(ownedItem);
    }
    private void OnDestroy()
    {
        foreach (ItemData item in boundItems)
        {
            item.effects.Clear();
            item.passiveEffects.Clear();
        }

        boundItems.Clear();
    }

    [ContextMenu("Test/Add Effect Items")]
    private void AddTestItems()
    {
        if (inventory == null)
        {
            Debug.LogError(
                "ItemUseController chưa được Initialize.", this);
            return;
        }

        foreach (ItemData item in boundItems)
        {
            inventory.AddItem(new InventoryItemData
            {
                itemId = item.itemId,
                remainingUses = item.initialUses
            });
        }
    }
    [ContextMenu("Test/Initialize Inventory And Run")]
    private void InitializeForTest()
    {
        if (!Application.isPlaying || inventory != null)
            return;

        RunController run = GetComponent<RunController>();

        Inventory testInventory = new Inventory(
            new InventoryData
            {
                items = new List<InventoryItemData>()
            });

        ItemUseContext testContext = new ItemUseContext
        {
            runController = run
        };

        Initialize(testInventory, testContext);

        run.StartRun();
        run.TakeDamage(20);

        AddTestItems();
    }
}