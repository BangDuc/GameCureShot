//using System;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;
//using UnityEngine.SceneManagement;

//public class EndingSceneController : MonoBehaviour
//{
//    [Header("Ending Database")]
//    [SerializeField] private EndingData[] endingDatabase;

//    [Header("Preview When Opening Ending Scene Directly")]
//    [SerializeField]
//    private EndingId previewEnding =
//        EndingId.NormalEnding;

//    [Header("Transition")]
//    [SerializeField] private CanvasGroup fadeOverlay;
//    [SerializeField] private float fadeDuration = 0.75f;
//    [SerializeField] private string nextSceneName = "MainMenu";

//    private bool isTransitioning;

//    [Header("UI")]
//    [SerializeField] private Image endingImage;
//    [SerializeField] private TMP_Text endingTypeText;
//    [SerializeField] private TMP_Text endingTitleText;
//    [SerializeField] private TMP_Text endingDescriptionText;
//    [SerializeField] private Image accentLine;
//    [SerializeField] private Button actionButton;

//    [Header("Optional")]
//    [SerializeField] private AudioSource musicSource;
//    [SerializeField] private GameObject toBeContinuedSequence;

//    private EndingData currentData;

//    private void Start()
//    {
//        EndingId endingId = EndingRuntime.HasSelection
//            ? EndingRuntime.CurrentEnding
//            : previewEnding;

//        ShowEnding(endingId);
//        if (fadeOverlay != null)
//            StartCoroutine(Fade(1f, 0f, false));
//    }

//    public void ShowEnding(EndingId endingId)
//    {
//        currentData = Array.Find(
//            endingDatabase,
//            data => data != null && data.endingId == endingId
//        );

//        if (currentData == null)
//        {
//            Debug.LogError(
//                $"Không tìm thấy EndingData cho {endingId}.",
//                this
//            );

//            return;
//        }

//        endingTypeText.text = currentData.typeLabel;
//        endingTitleText.text = currentData.endingTitle;
//        endingDescriptionText.text = currentData.description;

//        endingTypeText.color = currentData.accentColor;
//        accentLine.color = currentData.accentColor;

//        ApplyEndingImage();
//        ApplyButtonLabel();
//        ApplyMusic();

//        if (toBeContinuedSequence != null)
//        {
//            bool showSequence =
//                endingId == EndingId.ToBeContinuedEnding;

//            toBeContinuedSequence.SetActive(showSequence);
//        }
//    }

//    private void ApplyEndingImage()
//    {
//        if (endingImage == null)
//            return;

//        endingImage.sprite = currentData.endingImage;
//        endingImage.enabled = currentData.endingImage != null;

//        if (endingImage.enabled)
//            endingImage.color = Color.white;
//    }

//    private void ApplyButtonLabel()
//    {
//        if (actionButton == null)
//            return;

//        TMP_Text[] labels =
//            actionButton.GetComponentsInChildren<TMP_Text>(true);

//        foreach (TMP_Text label in labels)
//            label.text = currentData.actionLabel;
//    }

//    private void ApplyMusic()
//    {
//        if (musicSource == null)
//            return;

//        musicSource.Stop();
//        musicSource.clip = currentData.endingMusic;

//        if (musicSource.clip != null)
//            musicSource.Play();
//    }

//    public void OnActionButtonClicked()
//    {
//        if (!isTransitioning)
//            StartCoroutine(ReturnToMainMenu());
//    }

//    private IEnumerator ReturnToMainMenu()
//    {
//        isTransitioning = true;
//        actionButton.interactable = false;

//        yield return Fade(0f, 1f, true);

//        Time.timeScale = 1f;
//        SceneManager.LoadScene(nextSceneName);
//    }

//    private IEnumerator Fade(
//        float from,
//        float to,
//        bool keepBlocking)
//    {
//        if (fadeOverlay == null)
//            yield break;

//        fadeOverlay.blocksRaycasts = true;
//        fadeOverlay.alpha = from;

//        float elapsed = 0f;
//        float duration = Mathf.Max(0.01f, fadeDuration);

//        while (elapsed < duration)
//        {
//            elapsed += Time.unscaledDeltaTime;
//            fadeOverlay.alpha =
//                Mathf.Lerp(from, to, elapsed / duration);

//            yield return null;
//        }

//        fadeOverlay.alpha = to;
//        fadeOverlay.blocksRaycasts = keepBlocking;
//    }
//}