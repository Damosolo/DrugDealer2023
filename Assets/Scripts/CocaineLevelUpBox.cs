using UnityEngine;
using TMPro;

public class CocaineLevelUpBox : MonoBehaviour
{
    public int purchaseLevel;
    public TextMeshProUGUI purchaseText;
    public TextMeshProUGUI purchasePrompt;
    public CocainePlayerLevelManager playerLevelManager;
    public TextMeshProUGUI errorText;
    public float disableDelay = 2f;

    private bool isInRange;
    private int nextLevelCost;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            UpdatePurchaseText();
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
        int xpReward = playerLevelManager.GetXPReward(purchaseLevel);
        int moneyPerTask = playerLevelManager.GetMoneyPerTask(purchaseLevel);
        nextLevelCost = playerLevelManager.GetLevelCost(purchaseLevel + 1);

        if (nextLevelCost == 0)
        {
            // Reached max level, display "Max Level" text
            purchaseText.text = "Max Level";
            purchasePrompt.text = "";
        }
        else
        {
            purchaseText.text = "Purchase: Level " + purchaseLevel + "\nReward: " + xpReward + " XP, $" + moneyPerTask + " per Task";
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
        int currentLevel = playerLevelManager.cocainePlayerLevel;
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
            purchaseLevel++;
            UpdatePurchaseText();
            errorText.text = "Level purchased!";
            Invoke(nameof(DisableErrorText), disableDelay); // Disable error text after a delay
        }
        else
        {
            errorText.text = "Cannot purchase level. Insufficient funds.";
            Invoke(nameof(DisableErrorText), disableDelay); // Disable error text after a delay
        }
    }

    private void DisableErrorText()
    {
        errorText.text = "";
    }
}
