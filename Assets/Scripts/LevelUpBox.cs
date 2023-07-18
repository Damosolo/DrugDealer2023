using UnityEngine;
using TMPro;

public class LevelUpBox : MonoBehaviour
{
    public TextMeshProUGUI purchaseText;
    public TextMeshProUGUI purchasePrompt;
    public PlayerLevelManager playerLevelManager;
    public TextMeshProUGUI errorText;
    public float disableDelay = 2f;

    private bool isInRange;
    private int nextLevelCost;
    private int newMoneyPerTask;

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
            AttemptPurchase();
        }
    }

    private void UpdatePurchaseText()
    {
        int currentLevel = playerLevelManager.playerLevel;
        nextLevelCost = playerLevelManager.GetLevelCost(currentLevel + 1);
        newMoneyPerTask = playerLevelManager.GetMoneyPerTask(currentLevel + 1);

        if (nextLevelCost == 0)
        {
            // Reached max level, display "Max Level" text
            purchaseText.text = "Max Level";
            purchasePrompt.text = "";
        }
        else
        {
            purchaseText.text = "Purchase: Upgrade\nNew Rate: $" + newMoneyPerTask + " per Task";
            purchasePrompt.text = "Cost: $" + nextLevelCost + "\nPress E to purchase";
        }
    }

    private void ClearPurchaseText()
    {
        purchaseText.text = "";
        purchasePrompt.text = "";
    }

    private void AttemptPurchase()
    {
        int currentLevel = playerLevelManager.playerLevel;
        int nextLevelCost = playerLevelManager.GetLevelCost(currentLevel + 1);

        if (nextLevelCost == 0)
        {
            // Reached max level, show "Max Level" message
            errorText.text = "Max Level reached!";
            Invoke(nameof(DisableErrorText), disableDelay); // Disable error text after a delay
        }
        else if (playerLevelManager.moneyManager.money >= nextLevelCost)
        {
            playerLevelManager.moneyManager.SpendMoney(nextLevelCost);
            playerLevelManager.IncreaseLevel(currentLevel + 1);
            UpdatePurchaseText();
            errorText.text = "Upgrade purchased!";
            Invoke(nameof(DisableErrorText), disableDelay); // Disable error text after a delay
        }
        else
        {
            errorText.text = "Cannot purchase upgrade. Insufficient funds.";
            Invoke(nameof(DisableErrorText), disableDelay); // Disable error text after a delay
        }
    }

    private void DisableErrorText()
    {
        errorText.text = "";
    }
}
