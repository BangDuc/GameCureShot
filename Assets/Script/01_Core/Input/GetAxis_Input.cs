using UnityEngine;
using UnityEngine.InputSystem;

public class GetAxis_Input : MonoBehaviour
{
    [SerializeField] public Vector2 axisInput;

    private void OnEnable()
    {
        if (Input_Manager_System.Instance != null)
        {
            Input_Manager_System.Instance.On_Axis -= SetAxisInput;
            Input_Manager_System.Instance.On_Axis += SetAxisInput;
        }
    }

    private void Start()
    {
        if (Input_Manager_System.Instance != null)
        {
            Input_Manager_System.Instance.On_Axis -= SetAxisInput;
            Input_Manager_System.Instance.On_Axis += SetAxisInput;
        }
    }

    private void OnDisable()
    {
        if (Input_Manager_System.IsExist)
        {
            Input_Manager_System.Instance.On_Axis -= SetAxisInput;
        }
    }

    private void SetAxisInput(Vector2 axis)
    {
        axisInput = axis;
    }
}
