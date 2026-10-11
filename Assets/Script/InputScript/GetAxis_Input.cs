using UnityEngine;

public class GetAxis_Input : MonoBehaviour
{
    [SerializeField]public Vector2 axisInput;

    void SetAxisInput(Vector2 axis)
    {
        axisInput = axis;
    }

    private void Start()
    {
        Input_Manager_System.Instance.On_Axis += SetAxisInput;
    }
}
