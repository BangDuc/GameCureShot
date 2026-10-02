using UnityEngine;

public class OuterPageController : MonoBehaviour
{
    [Header("Outer Tabs")]
    [SerializeField] private OuterTabButton shopTab;
    [SerializeField] private OuterTabButton orionTab;

    [Header("Pages")]
    [SerializeField] private GameObject shopPage;
    [SerializeField] private GameObject orionPage;

    private void Start()
    {
        SelectTab(shopTab);
    }

    public void SelectTab(OuterTabButton selectedTab)
    {
        if (selectedTab != shopTab && selectedTab != orionTab)
            return;

        bool shopSelected = selectedTab == shopTab;

        shopTab.SetSelected(shopSelected);
        orionTab.SetSelected(!shopSelected);

        shopPage.SetActive(shopSelected);
        orionPage.SetActive(!shopSelected);
    }
}