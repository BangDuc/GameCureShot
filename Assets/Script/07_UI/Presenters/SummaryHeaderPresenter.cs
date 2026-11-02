using Assets.Script.Core.GamePlay;
using TMPro;
using UnityEngine;

public class SummaryHeaderPresenter : MonoBehaviour
{
    [Header("UI Text References")]
    [SerializeField] private TMP_Text dayTitleText;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text SubtitleText;

    [Header("Configuration - Complete (Survived)")]
    [SerializeField] private string completeResultText = "COMPLETE";
    [SerializeField] [TextArea(2, 3)] private string completeSubtitleText = "YOU SURVIVED. BUT THE STORY IS FAR FROM OVER...";
    [SerializeField] private Color completeColor = new Color(0.2f, 0.9f, 0.4f, 1f);

    [Header("Configuration - Fail (Dead / Run Failed)")]
    [SerializeField] private string failResultText = "FAIL";
    [SerializeField] [TextArea(2, 3)] private string failSubtitleText = "YOU WERE ELIMINATED. BUT THE DEBT CONTRACT REMAINS...";
    [SerializeField] private Color failColor = new Color(0.95f, 0.25f, 0.25f, 1f);

    private RunResult runResult;

    private void Awake()
    {
        EnsureReferences();
    }

    public void Initialize(RunResult runResult)
    {
        this.runResult = runResult;
        EnsureReferences();
        BuildSummary();
    }

    private void EnsureReferences()
    {
        if (dayTitleText == null || resultText == null || SubtitleText == null)
        {
            TMP_Text[] texts = GetComponentsInChildren<TMP_Text>(true);
            foreach (var t in texts)
            {
                if (t == null) continue;
                string lowerName = t.gameObject.name.ToLower();

                if (dayTitleText == null && (lowerName.Contains("day") || lowerName.Contains("title")))
                {
                    dayTitleText = t;
                }
                else if (resultText == null && lowerName.Contains("result"))
                {
                    resultText = t;
                }
                else if (SubtitleText == null && (lowerName.Contains("sub") || lowerName.Contains("desc")))
                {
                    SubtitleText = t;
                }
            }
        }
    }

    private void BuildSummary()
    {
        int day = runResult != null ? runResult.day : 1;
        bool isFailed = runResult == null || runResult.failed;

        // 1. Cập nhật tiêu đề ngày
        if (dayTitleText != null)
        {
            dayTitleText.text = $"DAY {day} SUMMARY";
        }

        // 2. Cập nhật trạng thái Result (FAIL khi chết / thất bại, COMPLETE khi sống sót / thành công)
        if (resultText != null)
        {
            resultText.text = isFailed ? failResultText : completeResultText;
            resultText.color = isFailed ? failColor : completeColor;
        }

        // 3. Cập nhật Subtitle theo từng trạng thái
        if (SubtitleText != null)
        {
            SubtitleText.text = isFailed ? failSubtitleText : completeSubtitleText;
        }

        Debug.Log($"<color={(isFailed ? "red" : "green")}>[SummaryHeaderPresenter] Ngày: {day} | Kết quả: {(isFailed ? "FAIL" : "COMPLETE")}</color>");
    }

    [ContextMenu("Test Preview Complete")]
    public void TestPreviewComplete()
    {
        runResult = new RunResult { day = 1, failed = false };
        BuildSummary();
    }

    [ContextMenu("Test Preview Fail")]
    public void TestPreviewFail()
    {
        runResult = new RunResult { day = 1, failed = true };
        BuildSummary();
    }
}
