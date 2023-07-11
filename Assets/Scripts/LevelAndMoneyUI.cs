using UnityEngine;
using TMPro;

public class LevelAndMoneyUI : MonoBehaviour
{
    public TextMeshProUGUI Level;
    public TextMeshProUGUI Money;
    public TextMeshProUGUI WeedStock;
    public TextMeshProUGUI CocaineStock;
    public MoneyManager MoneyValue;
    public PlayerLevelManager PlayerLevelManager;
    public WeedStockManager WeedStockManager;
    public CocaineStockManager CocaineStockManager;

    private void Start()
    {
        Level.enabled = true;
        Money.enabled = true;
        WeedStock.enabled = true;
        CocaineStock.enabled = true;
    }

    private void Update()
    {
        Money.text = "$: " + MoneyValue.money.ToString();
        Level.text = "XP: " + PlayerLevelManager.playerLevel.ToString();
        WeedStock.text = "Weed Stock: " + WeedStockManager.currentStock.ToString();
        CocaineStock.text = "Cocaine Stock: " + CocaineStockManager.currentStock.ToString();
    }
}
