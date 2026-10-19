using System;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    [SerializeField]string id;
    [SerializeField] TargetData data;

    public void InitTarget(){
        data=TargetManager.Instance.library.GetTarget(id);
    }

    private void Start()
    {
        InitTarget();
    }
}
