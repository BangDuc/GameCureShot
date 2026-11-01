using System;
using System.Collections.Generic;
using UnityEngine;

public class PhotoManager : MonoBehaviour
{
    [SerializeField] public List<Sprite> librarySpriteCapture;


    private void Awake()
    {
        librarySpriteCapture = new List<Sprite>();
    }



    
}