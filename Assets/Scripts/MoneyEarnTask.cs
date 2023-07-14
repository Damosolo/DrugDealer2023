using UnityEngine;
using TMPro;
using System.Collections;

public class MoneyEarnTask : MonoBehaviour
{
    public float countdownDuration = 30f; // Duration of the countdown in seconds
    public int playerLevelRequired = 1; // Minimum player level required to trigger the task
    public float copBribeChance = 0.1f; // Chance of a cop bribe happening (0.0f to 1.0f)

    private bool inTrigger = false; // Flag to track if the player is inside the trigger
    private bool countdownStarted = false; // Flag to track if the countdown has started
    private float countdownTimer = 0f; // Timer for the countdown
    private int moneyReward; // Amount of money rewarded for completing the task
    private int bribeAmount; // Amount of money required for the bribe

    public TextMeshProUGUI textDisplay; // Reference to the TextMeshProUGUI component for displaying information
    private PlayerLevelManager playerLevelManager; // Reference to the PlayerLevelManager component
    public TextMeshProUGUI errorText; // Reference to the TextMeshProUGUI component for displaying error message

    public AudioSource audioSource; // Reference to the AudioSource component for playing sounds
    public AudioClip copBribeSound; // Sound effect for CopBribe
    public TextMeshProUGUI timerText; // Reference to the TextMeshProUGUI component for the countdown timer
    public TextMeshProUGUI bribeAmountText; // Reference to the TextMeshProUGUI component for displaying the cop bribe amount

    private Coroutine disableBribeTextCoroutine; // Coroutine reference for disabling the bribe text
    private Coroutine disableCountdownTextCoroutine; // Coroutine reference for disabling the countdown text

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
                PlayCopBribeSound();

                // Calculate the cop bribe amount using the formula: playerStock + playerMoney + 100
                int playerMoney = MoneyManager.instance.money;
                int playerStock = WeedStockManager.instance.currentStock;
                bribeAmount = playerStock + playerMoney + 100;

                // Display the cop bribe amount to the player
                bribeAmountText.text = "Bribe Amount: $" + bribeAmount;
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
            timerText.text = "Countdown: " + countdownTimer.ToString("F1");

            if (countdownTimer <= 0f)
            {
                countdownStarted = false;

                // Check if the player has enough money for the bribe
                int playerMoney = MoneyManager.instance.money;

                if (playerMoney >= bribeAmount)
                {
                    MoneyManager.instance.SpendMoney(bribeAmount);
                    textDisplay.text = "Task Completed!\nMoney Earned: " + moneyReward + "\nBribe Passed!";
                }
                else
                {
                    MoneyManager.instance.LoseAllMoney();
                    WeedStockManager.instance.currentStock = 0;
                    textDisplay.text = "Task Completed!\nMoney Earned: " + moneyReward + "\nBribe Failed!";
                }

                // Reset the trigger so that the task can be triggered again
                inTrigger = false;

                // Deduct one weed stock
                WeedStockManager.instance.RemoveWeedStock(1);

                // Disable the bribe text after 3 seconds
                disableBribeTextCoroutine = StartCoroutine(DisableBribeText(3f));
            }
        }
    }

    private void StartTask()
    {
        textDisplay.enabled = true;
        textDisplay.text = "Countdown: " + countdownTimer.ToString("F1");
    }

    private void PlayCopBribeSound()
    {
        if (audioSource != null && copBribeSound != null)
        {
            audioSource.PlayOneShot(copBribeSound);
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

    private void ShowErrorText(string errorMessage)
    {
        errorText.text = errorMessage;
        Invoke(nameof(ClearErrorText), 2f); // Clear the error message after 2 seconds
    }

    private void ClearErrorText()
    {
        errorText.text = "";
    }

    private IEnumerator DisableBribeText(float delay)
    {
        yield return new WaitForSeconds(delay);
        bribeAmountText.gameObject.SetActive(false);
        disableBribeTextCoroutine = null;

        // Disable the countdown text after 3 seconds
        disableCountdownTextCoroutine = StartCoroutine(DisableCountdownText(3f));
    }

    private IEnumerator DisableCountdownText(float delay)
    {
        yield return new WaitForSeconds(delay);
        timerText.gameObject.SetActive(false);
        disableCountdownTextCoroutine = null;
    }

    private void OnDestroy()
    {
        // Stop the coroutines if the script is destroyed
        if (disableBribeTextCoroutine != null)
        {
            StopCoroutine(disableBribeTextCoroutine);
        }
        if (disableCountdownTextCoroutine != null)
        {
            StopCoroutine(disableCountdownTextCoroutine);
        }
    }
}
