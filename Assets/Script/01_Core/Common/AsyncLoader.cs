using Bang.Lib.Singleton;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncLoader : SingletonPersistent<AsyncLoader>
{
    public void StartLoading(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        Debug.Log($"<color=cyan>[AsyncLoader] Bắt đầu tải ngầm scene: {sceneName}</color>");
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        if (operation == null)
        {
            Debug.LogError($"<color=red>[AsyncLoader] Không thể tải scene '{sceneName}'! Hãy đảm bảo scene đã được thêm vào EditorBuildSettings.</color>");
            yield break;
        }

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }

        Debug.Log($"<color=green>[AsyncLoader] Tải hoàn tất scene: {sceneName}</color>");
    }
}
