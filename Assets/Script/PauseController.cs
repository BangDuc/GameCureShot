using UnityEngine;
using Michsky.UI.Dark;

public class PauseController : MonoBehaviour
{
    [SerializeField] private ModalWindowManager pauseModal;

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        if (IsPaused)
            return;

        IsPaused = true;
        pauseModal.ModalWindowIn();
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (!IsPaused)
            return;

        Time.timeScale = 1f;
        pauseModal.ModalWindowOut();
        IsPaused = false;
    }

    private void OnDestroy()
    {
        if (IsPaused)
            Time.timeScale = 1f;
    }
}