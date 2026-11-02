using Assets.Script.Core.GamePlay;
using Assets.Script.Core.GameRoot;
using System;
using UnityEngine;

public class SummaryPresenter : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private UIButtonView continueButton;
    [SerializeField] private SummaryHeaderPresenter headerPresenter;
    [SerializeField] private SummaryContentPresenter contentPresenter;

    private RunResult runResult;
    private DebtResult debtResult;
    private GameData gameData;

    public event Action Clicked;

    private void Awake()
    {
        EnsureComponents();
    }

    private void OnEnable()
    {
        EnsureComponents();
        if (continueButton != null)
        {
            continueButton.Clicked -= HandleClickedButton;
            continueButton.Clicked += HandleClickedButton;
        }
    }

    private void OnDisable()
    {
        if (continueButton != null)
        {
            continueButton.Clicked -= HandleClickedButton;
        }
    }

    private void EnsureComponents()
    {
        if (continueButton == null)
        {
            continueButton = GetComponentInChildren<UIButtonView>(true);
        }

        if (headerPresenter == null)
        {
            headerPresenter = GetComponentInChildren<SummaryHeaderPresenter>(true);
        }

        if (contentPresenter == null)
        {
            contentPresenter = GetComponentInChildren<SummaryContentPresenter>(true);
        }
    }

    public void Initialize(
        RunResult runResult,
        DebtResult debtResult,
        GameData gameData)
    {
        this.runResult = runResult;
        this.debtResult = debtResult;
        this.gameData = gameData;

        EnsureComponents();

        if (contentPresenter != null)
        {
            contentPresenter.Initialize(runResult, debtResult, gameData);
        }
        else
        {
            Debug.LogWarning("[SummaryPresenter] Không tìm thấy SummaryContentPresenter!");
        }

        if (headerPresenter != null)
        {
            headerPresenter.Initialize(runResult);
        }
        else
        {
            Debug.LogWarning("[SummaryPresenter] Không tìm thấy SummaryHeaderPresenter!");
        }
    }

    private void HandleClickedButton()
    {
        Clicked?.Invoke();
    }
}
