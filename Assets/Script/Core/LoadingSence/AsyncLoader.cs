using Bang.Lib.Singleton;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncLoader : Singleton<AsyncLoader>
{
    public void StartLoading(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // Tải ngầm Scene mới
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            // Tiến trình tải từ 0.0 đến 1.0 (Dùng để làm thanh Loading)
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log($"Đang tải: {progress * 100}%");

            yield return null; // Chờ sang khung hình tiếp theo
        }
    }
}