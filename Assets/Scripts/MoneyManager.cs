using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance; // Singleton instance of the Money Manager

    public int playerMoney = 0; // Current amount of player money

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void EarnMoney(int amount)
    {
        playerMoney += amount;
    }

    public bool SpendMoney(int amount)
    {
        if (playerMoney >= amount)
        {
            playerMoney -= amount;
            return true; // Purchase successful
        }
        else
        {
            return false; // Not enough money to make the purchase
        }
    }
}
