using UnityEngine;

public class MoneyPlayer : MonoBehaviour
{
    [SerializeField] private int money;
    public int Money=>money;

    public void EarnMoney(int e_money)
    {
        money += e_money;
    }

    public void MoneyĐeuct(int d_money)
    {
        if (!CanMoneyĐeuct(d_money)) return;
        money -= d_money;
    }

    public bool CanMoneyĐeuct(int d_money)
    {
        if (money < d_money) return false;
        return true;
    }
}
