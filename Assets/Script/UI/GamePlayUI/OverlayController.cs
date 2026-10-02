using Bang.Lib.Singleton;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OverlayController : Singleton<OverlayController>
{

    [SerializeField] public List<CanvasGroup> canvas;
    [SerializeField] public EmployeeCardUI employeeCardUI;

    [SerializeField] public PauseController pauseController;
    public CanvasGroup currentOverlay;

    private void Start()
    {
        employeeCardUI= GetComponent<EmployeeCardUI>();
        pauseController = GetComponent<PauseController>();
        currentOverlay = null;
        if (canvas.Count <= 0) return;
        foreach(var c in canvas)
        {
            c.gameObject.SetActive(false);
        }
    }
    
    public void ResetOverlay()
    {
        currentOverlay = null;
        if (canvas.Count <= 0) return;
        foreach (var c in canvas)
        {
            c.gameObject.SetActive(false);
        }
    }
    public void EnableCameraOverlay()
    {
        if (currentOverlay == canvas[0])
        {
            ResetOverlay();
            return;
        }
        ResetOverlay();
        canvas[0].gameObject.SetActive(true);
        currentOverlay = canvas[0];
    }
    public void EnableEmployeeOverlay()
    {
        if (currentOverlay == canvas[1])
        {
            employeeCardUI.Toggle();
            ResetOverlay();
            return;
        }
        ResetOverlay();
        canvas[1].gameObject.SetActive(true);
        employeeCardUI.Toggle();
        currentOverlay = canvas[1];
    }
    public void EnablePauseOverlay()
    {
        if (currentOverlay == canvas[2])
        {
            pauseController.ResumeGame();
            ResetOverlay();
            return;
        }
        ResetOverlay();
        canvas[2].gameObject.SetActive(true);
        pauseController.PauseGame();
        currentOverlay = canvas[2];
    }
}
