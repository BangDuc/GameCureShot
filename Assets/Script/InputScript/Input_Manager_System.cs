using Bang.Lib.Singleton;
using System;
using UnityEngine;

public class Input_Manager_System : Singleton<Input_Manager_System>
{
    public event Action OnPress_Space;

    public event Action OnPress_KeyC;

    public event Action<Vector2> On_Axis;


    void Start()
    {
        
    }

    void Update()
    {   //Axis

        OnAxis();

        //Key 

        OnPressed_KeyC();
        OnPressedSpace();

    }

    public void OnAxis()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 currentMoveInput = new Vector2(h, v).normalized;

        if (currentMoveInput != Vector2.zero)
        {
            On_Axis?.Invoke(currentMoveInput);
            return;
        }
        On_Axis?.Invoke(Vector2.zero);
    }

    public void OnPressedSpace()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnPress_Space?.Invoke();
        }
    }

    public void OnPressed_KeyC()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            OnPress_KeyC?.Invoke();
        }
    }



}


