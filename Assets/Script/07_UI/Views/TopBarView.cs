using TMPro;
using UnityEngine;

public class TopBarView : MonoBehaviour
{
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text cashText;
    [SerializeField] private TMP_Text debtText;
    [SerializeField] private TMP_Text healthText;
    
    public void Render(string day, string cash, string debt, string health)
    {
        dayText.text = day;
        cashText.text = cash;
        debtText.text = debt;
        healthText.text = health;
    }
}
