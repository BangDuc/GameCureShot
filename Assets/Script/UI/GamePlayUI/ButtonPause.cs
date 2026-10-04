using UnityEngine;
using UnityEngine.UI;

public class ButtonPause : MonoBehaviour
{
    [SerializeField] Button button;

    public void Start()
    {
        if (button == null) return;
        button.onClick.AddListener(Pause);

    }
    public void Pause()
    {
        RunController.Instance.Pause();
        OverlayController.Instance.EnablePauseOverlay();
        Debug.Log("pause");
    }
}
