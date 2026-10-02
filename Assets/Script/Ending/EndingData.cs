using UnityEngine;

[CreateAssetMenu(
    fileName = "NewEndingData",
    menuName = "Game/Ending Data"
)]
public class EndingData : ScriptableObject
{
    [Header("Identity")]
    public EndingId endingId;

    [Header("Text")]
    public string typeLabel;
    public string endingTitle;

    [TextArea(3, 6)]
    public string description;

    [Header("Media")]
    public Sprite endingImage;
    public AudioClip endingMusic;

    [Header("Style")]
    public Color accentColor = new Color(
        0.447f,
        0.835f,
        0.91f,
        1f
    );

    [Header("Button")]
    public string actionLabel = "CONTINUE";
}