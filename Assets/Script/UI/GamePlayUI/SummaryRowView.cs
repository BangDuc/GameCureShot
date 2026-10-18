using TMPro;
using UnityEngine;

public class SummaryRowView : MonoBehaviour
{
    [SerializeField] private TMP_Text labelText;
    [SerializeField] private TMP_Text valueText;

    public void Render(string label, string value)
    {
        labelText.text = label;
        valueText.text = value;
    }
}

