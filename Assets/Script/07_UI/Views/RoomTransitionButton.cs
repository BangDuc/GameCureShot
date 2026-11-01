using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RoomTransitionButton : MonoBehaviour
{
    [SerializeField] private RoomId targetRoom;
    [SerializeField] private bool toggleMode = false;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        if (button != null) button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        if (RoomManager.Instance == null) return;

        if (toggleMode)
            RoomManager.Instance.ToggleRoom();
        else
            RoomManager.Instance.SwitchRoom(targetRoom);
    }
}

