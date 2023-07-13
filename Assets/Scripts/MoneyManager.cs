using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;

    public int money = 0; // Current amount of money

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void EarnMoney(int amount)
    {
        money += amount;
        Debug.Log("Money Earned: " + amount);
    }

    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            Debug.Log("Money Spent: " + amount);
            return true; // Return true if the player has enough money and the transaction is successful
        }

        Debug.Log("Not enough money to spend: " + amount);
        return false; // Return false if the player does not have enough money
    }

    public void LoseAllMoney()
    {
        money = 0;
        Debug.Log("All money lost");
    }
}
