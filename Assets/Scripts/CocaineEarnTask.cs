using UnityEngine;
using TMPro;
using System.Collections;

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
    public TextMeshProUGUI errorText; // Reference to the TextMeshProUGUI component for displaying error message
    public CocaineStockManager stockManager; // Reference to the CocaineStockManager component

    public AudioClip copBribeSound; // Sound effect for CopBribe
    public TextMeshProUGUI timerText; // Reference to the TextMeshProUGUI component for the countdown timer

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
                    if (Random.Range(1, 11) == 1)
                    {
                        AudioManager.instance.PlaySoundEffect(copBribeSound);
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
            textDisplay.text = "Countdown: " + countdownTimer.ToString("F1");

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
            }
        }
    }

    private void StartTask()
    {
        textDisplay.enabled = true;
        textDisplay.text = "Countdown: " + countdownTimer.ToString("F1");
    }

    private void ShowTimerText()
    {
        timerText.gameObject.SetActive(true);
        timerText.text = "30";
        StartCoroutine(CountdownTimer());
    }

    private IEnumerator CountdownTimer()
    {
        int timerValue = 30;
        timerText.text = timerValue.ToString();

        while (timerValue > 0)
        {
            yield return new WaitForSeconds(1f);
            timerValue--;
            timerText.text = timerValue.ToString();
        }

        // Reset the trigger and deduct money and stock
        inTrigger = false;
        MoneyManager.instance.LoseAllMoney();
        stockManager.currentStock = 0;

        timerText.gameObject.SetActive(false);
        textDisplay.enabled = false;
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
