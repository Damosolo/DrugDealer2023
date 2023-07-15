using UnityEngine;
using TMPro;
using System.Collections;

public class MoneyEarnTask : MonoBehaviour
{
    public float countdownDuration = 30f; // Duration of the countdown in seconds
    public int playerLevelRequired = 1; // Minimum player level required to trigger the task

    private bool inTrigger = false; // Flag to track if the player is inside the trigger
    private bool countdownStarted = false; // Flag to track if the countdown has started
    private float countdownTimer = 0f; // Timer for the countdown
    private int moneyReward; // Amount of money rewarded for completing the task

    public TextMeshProUGUI textDisplay; // Reference to the TextMeshProUGUI component for displaying information
    private PlayerLevelManager playerLevelManager; // Reference to the PlayerLevelManager component
    public TextMeshProUGUI errorText; // Reference to the TextMeshProUGUI component for displaying error message

    private void Start()
    {
        textDisplay.enabled = false; // Hide the text initially
        playerLevelManager = FindObjectOfType<PlayerLevelManager>(); // Find the PlayerLevelManager component in the scene
    }

    private void Update()
    {
        if (inTrigger && !countdownStarted && Input.GetKeyDown(KeyCode.E))
        {
            // Check if player meets the required level and has stock to trigger the task
            int playerLevel = playerLevelManager.playerLevel; // Retrieve the player's level from the PlayerLevelManager
            if (playerLevel >= playerLevelRequired && WeedStockManager.instance.currentStock > 0)
            {
                countdownStarted = true;
                countdownTimer = countdownDuration;

                // Calculate the money reward based on the player's level
                moneyReward = Mathf.Max(playerLevelManager.GetMoneyPerTask(playerLevel), 50); // Base rate of $50

                StartTask();
            }
            else if (playerLevel < playerLevelRequired)
            {
                // Show error message for insufficient level
                ShowErrorText("Insufficient Level");
            }
            else
            {
                // Show error message for insufficient stock
                ShowErrorText("Insufficient Stock");
            }
        }

        if (countdownStarted)
        {
            countdownTimer -= Time.deltaTime;
            textDisplay.text = "Countdown: " + countdownTimer.ToString("F1");

            if (countdownTimer <= 0f)
            {
                countdownStarted = false;

                MoneyManager.instance.EarnMoney(moneyReward);

                // Reset the trigger so that the task can be triggered again
                inTrigger = false;

                // Deduct one weed stock
                WeedStockManager.instance.RemoveWeedStock(1);

                // Disable the text display after 3 seconds
                StartCoroutine(DisableTextDisplay(3f));
            }
        }
    }

    private void StartTask()
    {
        textDisplay.enabled = true;
        textDisplay.text = "Countdown: " + countdownTimer.ToString("F1");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = true;
            textDisplay.enabled = true;
            textDisplay.text = "Press 'E' to sell Weed.";
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

    private IEnumerator DisableTextDisplay(float delay)
    {
        yield return new WaitForSeconds(delay);
        textDisplay.enabled = false;
    }
}
