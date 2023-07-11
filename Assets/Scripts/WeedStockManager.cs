using UnityEngine;

public class WeedStockManager : MonoBehaviour
{
    public static WeedStockManager instance;

    public int currentStock = 0; // Current weed stock count
    public int maxStock = 10; // Maximum weed stock allowed

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddWeedStock(int amount)
    {
        currentStock += amount;

        // Clamp the stock count to stay within the allowed range
        currentStock = Mathf.Clamp(currentStock, 0, maxStock);
    }

    public void RemoveWeedStock(int amount)
    {
        currentStock -= amount;

        // Clamp the stock count to stay within the allowed range
        currentStock = Mathf.Clamp(currentStock, 0, maxStock);
    }
}
