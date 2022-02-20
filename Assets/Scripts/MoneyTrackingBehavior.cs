using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class MoneyTrackingBehavior : MonoBehaviour, IPlayerMoneyChangedListener
{
    public GameManager manager;

    private Text label;

    private void Start()
    {
        label = GetComponent<Text>();
        var moneyManager = manager.MoneyManager;
        moneyManager.AddPlayerMoneyChangedListener(this);
    }

    public void OnPlayerMoneyChanged(int newAmount)
    {
        Debug.Log("Hey, it's happening");
        label.text = $"${newAmount}";
    }
}
