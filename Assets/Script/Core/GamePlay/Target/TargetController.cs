using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public class TargetController : MonoBehaviour
{
    [SerializeField]string id;
    [SerializeField] TargetData data;
    List<IAffect> listAffects;
    [SerializeField] private List<MonoBehaviour> affectComponents = new List<MonoBehaviour>();
    public void InitTarget(){
        data=TargetManager.Instance.library.GetTarget(id);
        GetListAffects();
    }

    public void GetListAffects()
    {
        IAffect[] affects = GetComponents<IAffect>();
        listAffects=affects.ToList();
        affectComponents = affects.OfType<MonoBehaviour>().ToList();
    }

    private void Start()
    {
        InitTarget();

    }
}
