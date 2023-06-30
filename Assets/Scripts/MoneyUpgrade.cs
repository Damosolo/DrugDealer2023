using UnityEngine;

public class MoneyUpgrade : MonoBehaviour
{
    public int baseEarning = 10; // The base amount of money earned from tasks
    public int upgradeCost = 100; // The cost to purchase the upgrade
    public int upgradeIncrement = 5; // The amount by which the earning increases with each upgrade

    private int upgradeLevel = 0; // Current upgrade level

    public void PurchaseUpgrade()
    {
        if (MoneyManager.instance.SpendMoney(upgradeCost))
        {
            upgradeLevel++;
        }
        else
        {
            Debug.Log("Insufficient funds to purchase the upgrade.");
        }
    }

    public int GetCurrentEarning()
    {
        return baseEarning + (upgradeLevel * upgradeIncrement);
    }
}
