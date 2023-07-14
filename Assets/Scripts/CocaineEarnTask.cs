using UnityEngine;
using TMPro;
using System.Collections;

public class CocaineEarnTask : MonoBehaviour
{
    public float countdownDuration = 10f; // Duration of the countdown in seconds
    public int playerLevelRequired = 1; // Minimum player level required to trigger the task
    public float copBribeChance = 0.1f; // Chance of a cop bribe happening (0.0f to 1.0f)

    private bool inTrigger = false; // Flag to track if the player is inside the trigger
    private bool countdownStarted = false; // Flag to track if the countdown has started
    private float countdownTimer = 0f; // Timer for the countdown
    private int moneyReward; // Amount of money rewarded for completing the task

    public TextMeshProUGUI textDisplay; // Reference to the TextMeshProUGUI component for displaying information
    public CocainePlayerLevelManager playerLevelManager; // Reference to the CocainePlayerLevelManager component
    public TextMeshProUGUI errorText; // Reference to the TextMeshProUGUI component for displaying error message
    public CocaineStockManager stockManager; // Reference to the CocaineStockManager component

    public AudioSource audioSource; // Reference to the AudioSource component for playing sounds
    public AudioClip copBribeSound; // Sound effect for CopBribe
    public TextMeshProUGUI timerText; // Reference to the TextMeshProUGUI component for the countdown timer
    public TextMeshProUGUI bribeAmountText; // Reference to the TextMeshProUGUI component for displaying the cop bribe amount

    private Coroutine disableBribeTextCoroutine; // Coroutine reference for disabling the bribe text
    private Coroutine disableCountdownTextCoroutine; // Coroutine reference for disabling the countdown text

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
                // Check if player has stock
                if (stockManager.currentStock > 0)
                {
                    countdownStarted = true;
                    countdownTimer = countdownDuration;

                    moneyReward = playerLevelManager.GetMoneyPerTask(playerLevel); // Retrieve the money reward based on the player's level

                    // Check for CopBribe
                    if (Random.value <= copBribeChance)
                    {
                        PlayCopBribeSound();
                        ShowTimerText();
                    }
                    else
                    {
                        StartTask();
                    }
                }
                else
                {
                    // Show error message if player has no stock
                    ShowErrorText("Insufficient Stock");
                }
            }
            else
            {
                // Show error message if conditions are not met
                ShowErrorText("Insufficient Level");
            }
        }

        if (countdownStarted)
        {
            countdownTimer -= Time.deltaTime;
            timerText.text = "Countdown: " + countdownTimer.ToString("F1");

            if (countdownTimer <= 0f)
            {
                countdownStarted = false;
                if (stockManager.currentStock > 0)
                {
                    MoneyManager.instance.EarnMoney(moneyReward);
                    textDisplay.text = "Task Completed!\nMoney Earned: " + moneyReward;

                    // Reset the trigger so that the task can be triggered again
                    inTrigger = false;

                    // Deduct cocaine stock
                    stockManager.RemoveCocaineStock(1);
                }
                else
                {
                    // Show error message if player has no stock
                    ShowErrorText("Insufficient Stock");
                }

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

    private void ShowTimerText()
    {
        timerText.gameObject.SetActive(true);
        timerText.text = "Countdown: " + countdownTimer.ToString("F1");
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

    private IEnumerator DisableBribeText(float delay)
    {
        yield return new WaitForSeconds(delay);
        bribeAmountText.gameObject.SetActive(false);
        disableBribeTextCoroutine = null;
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
