using UnityEngine;
using TMPro;

public class CocaineEarnTask : MonoBehaviour
{
    public float countdownDuration = 10f; // Duration of the countdown in seconds
    public int playerLevelRequired = 1; // Minimum player level required to trigger the task

    private bool inTrigger = false; // Flag to track if the player is inside the trigger
    private bool countdownStarted = false; // Flag to track if the countdown has started
    private float countdownTimer = 0f; // Timer for the countdown
    private int moneyReward;

    public TextMeshProUGUI textDisplay; // Reference to the TextMeshProUGUI component for displaying information
    public CocainePlayerLevelManager playerLevelManager; // Reference to the CocainePlayerLevelManager component
    public CocaineStockManager cocaineStockManager; // Reference to the CocaineStockManager component
    public MoneyManager moneyManager; // Reference to the MoneyManager component

    public TextMeshProUGUI errorText; // Reference to the TextMeshProUGUI component for displaying error message

    private void Start()
    {
        textDisplay.enabled = false; // Hide the text initially
    }

    private void Update()
    {
        if (inTrigger && !countdownStarted && Input.GetKeyDown(KeyCode.E))
        {
            // Check if player meets the required level and has at least 1 cocaine stock to trigger the task
            int playerLevel = playerLevelManager.cocainePlayerLevel; // Retrieve the player's level from the CocainePlayerLevelManager
            if (playerLevel >= playerLevelRequired && cocaineStockManager.currentStock >= 1)
            {
                countdownStarted = true;
                countdownTimer = countdownDuration;

                moneyReward = playerLevelManager.GetMoneyPerTask(playerLevel); // Retrieve the money reward based on the player's level

                textDisplay.enabled = true;
                textDisplay.text = "Countdown: " + countdownTimer.ToString("F1");
            }
            else
            {
                // Show error message if conditions are not met
                ShowErrorText("No Cocaine Stock Available");
            }
        }

        if (countdownStarted)
        {
            countdownTimer -= Time.deltaTime;
            textDisplay.text = "Countdown: " + countdownTimer.ToString("F1");

            if (countdownTimer <= 0f)
            {
                countdownStarted = false;
                moneyManager.money += moneyReward; // Earn money using the MoneyManager reference
                textDisplay.text = "Task Completed!\nMoney Earned: " + moneyReward;

                // Deduct one cocaine stock from the stock manager
                cocaineStockManager.RemoveCocaineStock(1);

                // Reset the trigger so that the task can be triggered again
                inTrigger = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = true;
            textDisplay.enabled = true;
            textDisplay.text = "Press 'E' to sell Cocaine.";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = false;
            textDisplay.enabled = false;
        }
    }

    private void ShowErrorText(string errorMessage)
    {
        errorText.text = errorMessage;
        Invoke(nameof(ClearErrorText), 2f); // Clear the error message after 2 seconds
    }

    private void ClearErrorText()
    {
        errorText.text = "";
    }
}
