using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GetEffect : MonoBehaviour
{
    [SerializeField] string id;
    [SerializeField] public IItemEffect[] list;
    void Start()
    {
        list = GetComponents<IItemEffect>();

        foreach (var effect in list) {

            effect.Apply(new ItemUseContext());
        }
    }

    public void geteffect()
    {
       
    }

    
}
