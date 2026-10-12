using System;
using UnityEngine;

public class EndingPresenter : MonoBehaviour
{
    [SerializeField] private EndingView endingView;

    private bool initialized;

    public event Action ReturnToMenuRequested;

    private void OnEnable()
    {
        endingView.ContinueClicked += HandleContinueClicked;
    }

    private void OnDisable()
    {
        endingView.ContinueClicked -= HandleContinueClicked;
    }

    public void Initialize(EndingData ending)
    {
        endingView.Render(ending);
        initialized = true;
    }

    private void HandleContinueClicked()
    {
        if (!initialized)
            return;

        ReturnToMenuRequested?.Invoke();
    }
}
