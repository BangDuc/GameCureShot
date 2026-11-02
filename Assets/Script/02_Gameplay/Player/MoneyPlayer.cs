using UnityEngine;

public class MoneyPlayer : MonoBehaviour
{
    [SerializeField] private int money;
    public int Money=>money;

    public void EarnMoney(int e_money)
    {
        money += e_money;
    }

    public void MoneyDeduct(int d_money)
    {
        if (!CanMoneyDeduct(d_money)) return;
        money -= d_money;
    }

    public bool CanMoneyDeduct(int d_money)
    {
        if (money < d_money) return false;
        return true;
    }
}
