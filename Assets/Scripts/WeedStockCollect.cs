using UnityEngine;
using TMPro;

public class WeedStockCollect : MonoBehaviour
{
    public float countdownDuration = 10f; // Duration of the countdown in seconds
    public int stockAmount = 5; // Amount of weed stock to give

    public TextMeshProUGUI timerText; // Reference to the TextMeshProUGUI component for displaying the countdown timer

    private bool inTrigger = false; // Flag to track if the player is inside the trigger
    private bool countdownStarted = false; // Flag to track if the countdown has started
    private float countdownTimer = 0f; // Timer for the countdown

    private void Start()
    {
        timerText.gameObject.SetActive(false); // Hide the timer text initially
    }

    private void Update()
    {
        if (inTrigger && !countdownStarted && Input.GetKeyDown(KeyCode.E))
        {
            StartCountdown();
        }

        if (countdownStarted)
        {
            countdownTimer -= Time.deltaTime;
            UpdateTimerText();

            if (countdownTimer <= 0f)
            {
                countdownStarted = false;
                GiveWeedStock();
                ResetTrigger();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = true;
            timerText.text = "Press 'E' to Harvest";
            timerText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ResetTrigger();
        }
    }

    private void StartCountdown()
    {
        countdownStarted = true;
        countdownTimer = countdownDuration;
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        int seconds = Mathf.CeilToInt(countdownTimer);
        timerText.text = "Countdown: " + seconds.ToString();

        // Update the color of the timer text based on the remaining time
        if (countdownTimer <= 3f)
        {
            timerText.color = Color.red;
        }
        else if (countdownTimer <= 6f)
        {
            timerText.color = Color.yellow;
        }
        else
        {
            timerText.color = Color.green;
        }
    }

    private void GiveWeedStock()
    {
        WeedStockManager.instance.AddWeedStock(stockAmount);
        Debug.Log("Weed Stock Given: " + stockAmount);
    }

    private void ResetTrigger()
    {
        inTrigger = false;
        countdownStarted = false;
        countdownTimer = 0f;
        timerText.gameObject.SetActive(false);
    }
}
