using UnityEngine;

public class ShopTabController : MonoBehaviour
{
    [Header("Tabs")]
    [SerializeField] private ShopTabButton purchaseTab;
    [SerializeField] private ShopTabButton cameraTab;

    [Header("Shop Contents")]
    [SerializeField] private GameObject purchaseContent;
    [SerializeField] private GameObject cameraContent;

    [Header("Description Contents")]
    [SerializeField] private GameObject purchaseDetailContent;
    [SerializeField] private GameObject cameraDetailContent;

    private void Start()
    {
        SelectTab(purchaseTab);
    }

    public void SelectTab(ShopTabButton selectedTab)
    {
        if (selectedTab != purchaseTab && selectedTab != cameraTab)
            return;

        bool isPurchase = selectedTab == purchaseTab;

        // Trạng thái hai tab
        purchaseTab.SetSelected(isPurchase);
        cameraTab.SetSelected(!isPurchase);

        // Nội dung lớn bên trái
        purchaseContent.SetActive(isPurchase);
        cameraContent.SetActive(!isPurchase);

        // Description bên phải
        purchaseDetailContent.SetActive(isPurchase);
        cameraDetailContent.SetActive(!isPurchase);
    }

    public void ShowPurchase()
    {
        SelectTab(purchaseTab);
    }

    public void ShowCamera()
    {
        SelectTab(cameraTab);
    }
}