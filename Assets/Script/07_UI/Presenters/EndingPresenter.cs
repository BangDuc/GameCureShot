using System;
using UnityEngine;

public class EndingPresenter : MonoBehaviour
{
    [SerializeField] private EndingView endingView;

    private bool initialized;

    public event Action ReturnToMenuRequested;

    private void Awake()
    {
        EnsureEndingView();
    }

    private void OnEnable()
    {
        EnsureEndingView();
        if (endingView != null)
        {
            endingView.ContinueClicked -= HandleContinueClicked;
            endingView.ContinueClicked += HandleContinueClicked;
        }
    }

    private void OnDisable()
    {
        if (endingView != null)
        {
            endingView.ContinueClicked -= HandleContinueClicked;
        }
    }

    public void EnsureEndingView()
    {
        if (endingView == null)
        {
            endingView = GetComponent<EndingView>();
            if (endingView == null)
            {
#if UNITY_2023_1_OR_NEWER
                endingView = UnityEngine.Object.FindFirstObjectByType<EndingView>(FindObjectsInactive.Include);
#else
                endingView = UnityEngine.Object.FindObjectOfType<EndingView>(true);
#endif
            }
            if (endingView == null)
            {
                endingView = gameObject.AddComponent<EndingView>();
            }
        }
    }

    public void Setup(EndingView view)
    {
        endingView = view;
        if (endingView != null)
        {
            endingView.ContinueClicked -= HandleContinueClicked;
            endingView.ContinueClicked += HandleContinueClicked;
        }
    }

    public void Initialize(EndingData ending)
    {
        EnsureEndingView();

        if (endingView != null)
        {
            endingView.Render(ending);
            initialized = true;
        }
        else
        {
            Debug.LogError("[EndingPresenter] Không tìm thấy EndingView để Render!");
        }
    }

    private void HandleContinueClicked()
    {
        if (!initialized)
            return;

        ReturnToMenuRequested?.Invoke();
    }
}
