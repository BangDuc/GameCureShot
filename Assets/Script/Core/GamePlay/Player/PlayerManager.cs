using Bang.Lib.Singleton;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField]public MoneyPlayer moneyPlayer;
    [SerializeField] public HealthPlayer healthPlayer;



}
