using UnityEngine;
using TMPro;

public class CocaineEarnTask : MonoBehaviour
{
    public float countdownDuration = 10f; // Duration of the countdown in seconds
    public int playerLevelRequired = 1; // Minimum player level required to trigger the task

    private bool inTrigger = false; // Flag to track if the player is inside the trigger
    private bool countdownStarted = false; // Flag to track if the countdown has started
    private float countdownTimer = 0f; // Timer for the countdown
    private int moneyReward; // Amount of money rewarded for completing the task

    public TextMeshProUGUI textDisplay; // Reference to the TextMeshProUGUI component for displaying information
    public CocainePlayerLevelManager playerLevelManager; // Reference to the CocainePlayerLevelManager component

    private void Start()
    {
        textDisplay.enabled = false; // Hide the text initially
    }

    private void Update()
    {
        if (inTrigger && !countdownStarted && Input.GetKeyDown(KeyCode.E))
        {
            // Check if player meets the required level to trigger the task
            int playerLevel = playerLevelManager.cocainePlayerLevel; // Retrieve the player's level from the CocainePlayerLevelManager
            if (playerLevel >= playerLevelRequired)
            {
                countdownStarted = true;
                countdownTimer = countdownDuration;

                moneyReward = playerLevelManager.GetMoneyPerTask(playerLevel); // Retrieve the money reward based on the player's level

                textDisplay.enabled = true;
                textDisplay.text = "Countdown: " + countdownTimer.ToString("F1");
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
                textDisplay.text = "Task Completed!\nMoney Earned: " + moneyReward;

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
}
