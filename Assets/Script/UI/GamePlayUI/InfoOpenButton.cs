using UnityEngine;
using UnityEngine.UI;

public class InfoOpenButton : MonoBehaviour
{
    Button btn_info;
    
    void Start()
    {
        btn_info = GetComponent<Button>();
        btn_info.onClick.AddListener(OpenInfo);
    }
    public void OpenInfo()
    {
        OverlayController.Instance.EnableEmployeeOverlay();
    }

   
}
