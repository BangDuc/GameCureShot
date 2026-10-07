using Assets.Script.Core.GameRoot;
using System.Collections.Generic;
using UnityEngine;

public class SavePresenter : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private SaveSlotView slotPrefab;
    private readonly List<SaveSlotView> views = new();

    private void Awake()
    {

    }

    public void BuildProfile()
    {
        List<SaveInfo> saves = GetListSaveInfos();
        saves.Sort((a, b) => a.slotId.CompareTo(b.slotId));

        ClearSlots();

        foreach (SaveInfo save in saves)
        {
            SaveSlotView view = Instantiate(slotPrefab, content, false);
            view.gameObject.SetActive(true);
            view.Render(save.slotId, save.saveName, save);
            view.Selected += HandleSaveSlotClicked;
            views.Add(view);
        }
    }

    public List<SaveInfo> GetListSaveInfos()
    {
        return SaveSystem.GetSaveInfos() ?? new List<SaveInfo>();
    }

    private void HandleSaveSlotClicked(int slotId)
    {

        if (!isActiveAndEnabled)
            return;

        Debug.Log($"vừa nhấn save {slotId}", this);
    }

    private void ClearSlots()
    {
        foreach (SaveSlotView view in views)
        {
            if (view == null)
                continue;

            view.Selected -= HandleSaveSlotClicked;
            view.gameObject.SetActive(false);
            Destroy(view.gameObject);
        }

        views.Clear();
    }

    private void OnDestroy()
    {
        ClearSlots();
    }
}