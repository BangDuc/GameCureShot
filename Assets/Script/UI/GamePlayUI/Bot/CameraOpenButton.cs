using UnityEngine;
using UnityEngine.UI;

public class CameraOpenButton : MonoBehaviour
{
    Button btn_opencamera;
    
    void Start()
    {
        btn_opencamera= GetComponent<Button>();
        btn_opencamera.onClick.AddListener(OpenCamera);
    }
    public void OpenCamera()
    {
        OverlayController.Instance.EnableCameraOverlay();
    }

   
}
