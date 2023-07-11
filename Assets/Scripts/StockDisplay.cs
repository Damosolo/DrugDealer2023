using UnityEngine;
using TMPro;

public class StockDisplay : MonoBehaviour
{
    public TextMeshProUGUI weedStockText; // Reference to the TextMeshProUGUI component for displaying weed stock
    public TextMeshProUGUI cocaineStockText; // Reference to the TextMeshProUGUI component for displaying cocaine stock

    private int weedStock = 0; // Current weed stock
    private int cocaineStock = 0; // Current cocaine stock

    private void Start()
    {
        UpdateStockText();
    }

    private void UpdateStockText()
    {
        weedStockText.text = "Weed Stock: " + weedStock;
        cocaineStockText.text = "Cocaine Stock: " + cocaineStock;
    }

    public void AddWeedStock(int amount)
    {
        weedStock += amount;
        UpdateStockText();
    }

    public void AddCocaineStock(int amount)
    {
        cocaineStock += amount;
        UpdateStockText();
    }

    public void UseWeedStock(int amount)
    {
        weedStock -= amount;
        UpdateStockText();
    }

    public void UseCocaineStock(int amount)
    {
        cocaineStock -= amount;
        UpdateStockText();
    }
}
