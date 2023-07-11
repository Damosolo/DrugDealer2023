using UnityEngine;
using TMPro;

public class StockManager : MonoBehaviour
{
    public static StockManager instance;

    public int weedStock = 0; // Current weed stock count
    public int cocaineStock = 0; // Current cocaine stock count
    public int maxStock = 10; // Maximum stock allowed

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddWeedStock(int amount)
    {
        weedStock += amount;

        // Clamp the stock count to stay within the allowed range
        weedStock = Mathf.Clamp(weedStock, 0, maxStock);
    }

    public void AddCocaineStock(int amount)
    {
        cocaineStock += amount;

        // Clamp the stock count to stay within the allowed range
        cocaineStock = Mathf.Clamp(cocaineStock, 0, maxStock);
    }

    public void RemoveWeedStock(int amount)
    {
        weedStock -= amount;

        // Clamp the stock count to stay within the allowed range
        weedStock = Mathf.Clamp(weedStock, 0, maxStock);
    }

    public void RemoveCocaineStock(int amount)
    {
        cocaineStock -= amount;

        // Clamp the stock count to stay within the allowed range
        cocaineStock = Mathf.Clamp(cocaineStock, 0, maxStock);
    }
}
