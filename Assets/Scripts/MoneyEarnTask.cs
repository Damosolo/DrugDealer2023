using UnityEngine;
using TMPro;

public class MoneyEarnTask : MonoBehaviour
{
    public float countdownDuration = 5f; // Duration of the countdown in seconds
    public int playerLevelRequired = 1; // Minimum player level required to trigger the task

    private bool inTrigger = false; // Flag to track if the player is inside the trigger
    private bool countdownStarted = false; // Flag to track if the countdown has started
    private float countdownTimer = 0f; // Timer for the countdown
    private int moneyReward; // Amount of money rewarded for completing the task

    public TextMeshProUGUI textDisplay; // Reference to the TextMeshProUGUI component for displaying information
    private PlayerLevelManager playerLevelManager; // Reference to the PlayerLevelManager component

    private void Start()
    {
        textDisplay.enabled = false; // Hide the text initially
        playerLevelManager = FindObjectOfType<PlayerLevelManager>(); // Find the PlayerLevelManager component in the scene
    }

    private void Update()
    {
        if (inTrigger && !countdownStarted && Input.GetKeyDown(KeyCode.E))
        {
            // Check if player meets the required level to trigger the task
            int playerLevel = playerLevelManager.playerLevel; // Retrieve the player's level from the PlayerLevelManager
            if (playerLevel >= playerLevelRequired)
            {
                countdownStarted = true;
                countdownTimer = countdownDuration;

                moneyReward = Mathf.Max(playerLevelManager.GetMoneyPerTask(playerLevel), 50); // Retrieve the money reward based on the player's level, with a minimum of 50

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
}
