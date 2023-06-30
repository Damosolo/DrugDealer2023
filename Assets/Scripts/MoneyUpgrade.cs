using UnityEngine;

public class MoneyUpgrade : MonoBehaviour
{
    public int moneyCost = 100; // Cost of the money upgrade

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MoneyManager moneyManager = MoneyManager.instance;

            // Check if the player has enough money to purchase the upgrade
            if (moneyManager.SpendMoney(moneyCost))
            {
                UpgradeObject();
                gameObject.SetActive(false); // Disable the upgrade object after purchase
            }
        }
    }

    private void UpgradeObject()
    {
        // Implement your logic to upgrade the object here
        Debug.Log("Object Upgraded!");
    }
}
