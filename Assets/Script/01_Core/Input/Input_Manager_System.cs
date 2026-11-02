using Bang.Lib.Singleton;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input_Manager_System : Singleton<Input_Manager_System>
{
    public event Action OnPress_Space;
    public event Action OnPress_KeyC;
    public event Action<Vector2> On_Axis;

    private void Update()
    {
        OnAxis();
        OnPressed_KeyC();
        OnPressedSpace();
    }

    public void OnAxis()
    {
        float h = 0f;
        float v = 0f;

        // 1. New Input System Keyboard
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) h -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) h += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) v -= 1f;
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) v += 1f;
        }

        // 2. Fallback Legacy Input System
        if (Mathf.Approximately(h, 0f) && Mathf.Approximately(v, 0f))
        {
            try
            {
                h = Input.GetAxisRaw("Horizontal");
                v = Input.GetAxisRaw("Vertical");
            }
            catch {}
        }

        Vector2 currentMoveInput = new Vector2(h, v);
        if (currentMoveInput.sqrMagnitude > 1f)
        {
            currentMoveInput.Normalize();
        }

        On_Axis?.Invoke(currentMoveInput);
    }

    public void OnPressedSpace()
    {
        bool pressed = false;
        if (Keyboard.current != null)
        {
            pressed = Keyboard.current.spaceKey.wasPressedThisFrame;
        }
        if (!pressed)
        {
            try { pressed = Input.GetKeyDown(KeyCode.Space); } catch {}
        }

        if (pressed)
        {
            OnPress_Space?.Invoke();
        }
    }

    public void OnPressed_KeyC()
    {
        bool pressed = false;
        if (Keyboard.current != null)
        {
            pressed = Keyboard.current.cKey.wasPressedThisFrame;
        }
        if (!pressed)
        {
            try { pressed = Input.GetKeyDown(KeyCode.C); } catch {}
        }

        if (pressed)
        {
            OnPress_KeyC?.Invoke();
        }
    }
}
