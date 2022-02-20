using System.Collections.Generic;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts
{
    public class MoneyManager
    {

        private int _playerMoney;
        private int playerMoney
        {
            get => _playerMoney;
            set
            {
                _playerMoney = value;
                NotifyPlayerMoneyChangedListeners(_playerMoney);
            }
        }

        List<IPlayerMoneyChangedListener> moneyChangedListeners = new List<IPlayerMoneyChangedListener>();

        public MoneyManager(int startingMoney)
        {
            playerMoney = startingMoney;
        }

        public bool CanAfford(int cost) => cost <= playerMoney;

        public void AddBalance(int amount)
        {
            playerMoney += amount;
        }

        public bool TrySubtractBalance(int cost)
        {
            if (!CanAfford(cost))
                return false;

            playerMoney -= cost;
            return true;
        }

        public void AddPlayerMoneyChangedListener(IPlayerMoneyChangedListener listener) {
            moneyChangedListeners.Add(listener);
            listener.OnPlayerMoneyChanged(playerMoney);
        }

        public void RemovePlayerMoneyChangedListener(IPlayerMoneyChangedListener listener) => moneyChangedListeners.Remove(listener);

        public void ClearPlayerMoneyChangedListeners() => moneyChangedListeners.Clear();

        private void NotifyPlayerMoneyChangedListeners(int newAmount)
        {
            foreach (var listener in moneyChangedListeners)
            {
                listener.OnPlayerMoneyChanged(newAmount);
            }
        }
    }
}
