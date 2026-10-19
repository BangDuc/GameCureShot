using Bang.Lib.Singleton;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField]public MoneyPlayer moneyPlayer;
    [SerializeField] public HealthPlayer healthPlayer;


    public void GetHurt(int damage )
    {
        healthPlayer.HealthDeduct(damage);
        EventBus.Publish("OnPlayerGetHurt");
    }


}
