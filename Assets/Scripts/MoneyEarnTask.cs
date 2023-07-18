using UnityEngine;
using TMPro;
using System.Collections;

public class MoneyEarnTask : MonoBehaviour
{
    public float countdownDuration = 30f;
    public int playerLevelRequired = 1;
    private bool inTrigger = false;
    private bool countdownStarted = false;
    private float countdownTimer = 0f;
    private int moneyReward;
    public TextMeshProUGUI textDisplay;
    private PlayerLevelManager playerLevelManager;
    public TextMeshProUGUI errorText;
    private MoneyManager moneyManager;
    private WeedStockManager weedStockManager;

    private void Start()
    {
        textDisplay.enabled = false;
        playerLevelManager = FindObjectOfType<PlayerLevelManager>();
        moneyManager = MoneyManager.instance;
        weedStockManager = WeedStockManager.instance;
    }

    private void Update()
    {
        if (inTrigger && !countdownStarted && Input.GetKeyDown(KeyCode.E))
        {
            int playerLevel = playerLevelManager.playerLevel;
            if (playerLevel >= playerLevelRequired && weedStockManager.currentStock > 0)
            {
                countdownStarted = true;
                countdownTimer = countdownDuration;
                moneyReward = Mathf.Max(playerLevelManager.GetMoneyPerTask(playerLevel), 50);
                StartTask();
            }
            else if (playerLevel < playerLevelRequired)
            {
                ShowErrorText("Insufficient Level");
            }
            else
            {
                ShowErrorText("Insufficient Stock");
            }
        }

        if (countdownStarted)
        {
            countdownTimer -= Time.deltaTime;
            textDisplay.text = string.Format("Countdown: {0:F1}", countdownTimer);

            if (countdownTimer <= 0f)
            {
                countdownStarted = false;
                moneyManager.EarnMoney(moneyReward);
                weedStockManager.RemoveWeedStock(1);
                StartCoroutine(ShowEarnedMoneyThenHide(2f));
            }
        }
    }

    private void StartTask()
    {
        textDisplay.enabled = true;
        textDisplay.text = string.Format("Countdown: {0:F1}", countdownTimer);
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
        StartCoroutine(ClearErrorText(2f));
    }

    private IEnumerator ClearErrorText(float delay)
    {
        yield return new WaitForSeconds(delay);
        errorText.text = "";
    }

    private IEnumerator ShowEarnedMoneyThenHide(float delay)
    {
        textDisplay.text = "Money earned: " + moneyReward;
        yield return new WaitForSeconds(delay);
        if (inTrigger)
        {
            textDisplay.text = "Press 'E' to sell Weed.";
        }
        else
        {
            textDisplay.enabled = false;
        }
    }
}
