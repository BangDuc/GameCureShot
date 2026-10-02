using UnityEngine;


public class BangMonoBehaviour : MonoBehaviour
{
    private void Awake()
    {
        LoadComponent();
    }
    [ContextMenu("LoadComponet")]
    protected virtual void LoadComponent()
    {

    }
}
