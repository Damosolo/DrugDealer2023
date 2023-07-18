using UnityEngine;
using TMPro;

public class CocaineLevelUpBox : MonoBehaviour
{
    public TextMeshProUGUI purchaseText;
    public TextMeshProUGUI purchasePrompt;
    public CocainePlayerLevelManager playerLevelManager;
    public TextMeshProUGUI errorText;
    public float disableDelay = 2f;

    private bool isInRange;
    private int nextLevelCost;
    private int newMoneyPerTask;
    private int currentLevel;
    private int currentMoney;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            UpdatePurchaseText();
            purchasePrompt.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            ClearPurchaseText();
            purchasePrompt.enabled = false;
        }
    }

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            AttemptUpgradePurchase();
        }
    }

    private void UpdatePurchaseText()
    {
        currentLevel = playerLevelManager.cocainePlayerLevel;
        currentMoney = playerLevelManager.moneyManager.money;
        nextLevelCost = playerLevelManager.GetLevelCost(currentLevel + 1);
        newMoneyPerTask = playerLevelManager.GetMoneyPerTask(currentLevel + 1);

        if (nextLevelCost == 0)
        {
            purchaseText.text = "Max Level";
            purchasePrompt.text = "";
        }
        else
        {
            purchaseText.text = $"Purchase: Upgrade\nNew Rate: ${newMoneyPerTask} per Task";
            purchasePrompt.text = $"Cost: ${nextLevelCost}\nPress E to purchase";
        }
    }

    private void ClearPurchaseText()
    {
        purchaseText.text = "";
        purchasePrompt.text = "";
    }

    private void AttemptUpgradePurchase()
    {
        if (nextLevelCost == 0)
        {
            ShowError("Max Level reached!");
        }
        else if (currentMoney >= nextLevelCost)
        {
            playerLevelManager.moneyManager.SpendMoney(nextLevelCost);
            playerLevelManager.IncreaseLevel(currentLevel + 1);
            UpdatePurchaseText();
            ShowError("Upgrade purchased!");
        }
        else
        {
            ShowError("Cannot purchase upgrade. Insufficient funds.");
        }
    }

    private void ShowError(string message)
    {
        errorText.text = message;
        Invoke(nameof(DisableErrorText), disableDelay);
    }

    private void DisableErrorText()
    {
        errorText.text = "";
    }
}
