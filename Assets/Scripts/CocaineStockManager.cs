using UnityEngine;

public class CocaineStockManager : MonoBehaviour
{
    public static CocaineStockManager instance;

    public int currentStock = 0; // Current cocaine stock count
    public int maxStock = 10; // Maximum cocaine stock allowed

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddCocaineStock(int amount)
    {
        currentStock += amount;

        // Clamp the stock count to stay within the allowed range
        currentStock = Mathf.Clamp(currentStock, 0, maxStock);
    }

    public bool RemoveCocaineStock(int amount)
    {
        if (currentStock >= amount)
        {
            currentStock -= amount;

            // Clamp the stock count to stay within the allowed range
            currentStock = Mathf.Clamp(currentStock, 0, maxStock);
            return true; // Return true if stock removal is successful
        }

        return false; // Return false if there is insufficient stock
    }
}
