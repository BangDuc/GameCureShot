using System;
using UnityEngine;

public class SaveInfo
{
    public int slotId { get; set; }
    public string saveName { get; set; }
    public int day { get; set; }
    public int money { get; set; }
    public int phase { get; set; }
    public int permanentHealth { get; set; }
    public float playTime { get; set; }
    public DateTime timestamp { get; set; }
    public bool isSealBroken { get; set; }
}
