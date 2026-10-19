using Assets.Script.Core.GamePlay;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeCardPresenter : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private UIButtonView infoButton;
    [SerializeField] private PanelView infoPanel;
    [SerializeField] private Transform buffList;
    [SerializeField] private Transform permanentHealth;
    [SerializeField] private BuffRowView buffRowPrefab;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image permanentHealthSegmentPrefab;

    private readonly List<BuffRowView> buffRows = new();
    private readonly List<GameObject> permanentHealthViews = new();

    [Header("System")]
    [SerializeField] private RunController controller;

    private void Start()
    {
        controller = RunController.Instance;
    }

    private void OnEnable()
    {
        infoButton.Clicked += HandleInfoClicked;
    }
    private void Update()
    {
        if (infoPanel.isActiveAndEnabled)
            RenderHealth();
    }

    private void OnDisable()
    {
        infoButton.Clicked -= HandleInfoClicked;
    }

    private void RenderCard()
    {
        RenderHealth();

        foreach (GameObject view in permanentHealthViews)
        {
            view.SetActive(false);
            Destroy(view);
        }

        permanentHealthViews.Clear();

        int segmentCount = Mathf.Clamp(
            controller.PermanentHealth, 0, 4);

        for (int i = 0; i < segmentCount; i++)
        {
            Image view = Instantiate(
                permanentHealthSegmentPrefab,
                permanentHealth,
                false);

            view.gameObject.SetActive(true);
            permanentHealthViews.Add(view.gameObject);
        }
        RenderBuffs();
    }

    private void RenderHealth()
    {
        HealthPlayer health = PlayerManager.Instance.healthPlayer;

        float ratio = health.maxHealth > 0
            ? Mathf.Clamp01((float)health.currentHealth / health.maxHealth)
            : 0f;

        healthBar.rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            450f * ratio);
    }

    private void RenderBuffs()
    {
        ClearBuffRows();

        BuffState buffs = controller.Buffs;

        if (buffs == null) return;

        if (buffs.hasShield && !buffs.shieldUsed)
            AddBuffRow("Shield");

        if (buffs.hasCoin && !buffs.coinUsed)
            AddBuffRow("Coin");

        if (buffs.hasBlood && !buffs.bloodUsed)
            AddBuffRow("Blood");

        if (buffRows.Count == 0)
            AddBuffRow("NO ACTIVE EFFECTS");
    }

    private void AddBuffRow(string buffName)
    {
        BuffRowView view =
            Instantiate(buffRowPrefab, buffList, false);

        view.Render(buffName);
        view.gameObject.SetActive(true);

        buffRows.Add(view);
    }

    private void ClearBuffRows()
    {
        foreach (BuffRowView view in buffRows)
        {
            if (view == null)
                continue;

            view.gameObject.SetActive(false);
            Destroy(view.gameObject);
        }

        buffRows.Clear();
    }
    private void HandleInfoClicked()
    {
        if (infoPanel.isActiveAndEnabled)
        {
            infoPanel.Hide();
            return;
        }

        infoPanel.Show();
        RenderCard();
    }
}
