using TMPro;
using UnityEngine;


public class BuffRowView : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;

    public void Render(string buffName)
    {
        nameText.text = buffName;
    }
}
