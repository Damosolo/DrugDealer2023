using UnityEngine;
using TMPro;

public class CocaineStockCollect : MonoBehaviour
{
    public int stockAmount = 5;
    public AudioClip collectSound;
    public TextMeshProUGUI interactionText;
    public CocainePlayerLevelManager playerLevelManager; // Get reference to the player level manager
    public int unlockPrice = 20000; // Set unlock price

    private bool canInteract = false;
    private bool isUnlocked = false; // Flag to track if the functionality is unlocked

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        interactionText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            if (isUnlocked)
            {
                GiveCocaineStock();
            }
            else
            {
                if (playerLevelManager.moneyManager.money >= unlockPrice)
                {
                    playerLevelManager.moneyManager.SpendMoney(unlockPrice);
                    isUnlocked = true;
                    interactionText.text = "Unlocked! Press 'E' to harvest cocaine.";
                }
                else
                {
                    interactionText.text = "Not enough money to unlock cocaine harvesting!";
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            interactionText.gameObject.SetActive(true);
            interactionText.text = isUnlocked ? "Press 'E' to harvest cocaine." : "Pay $20000 to unlock. Press 'E' to pay.";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            interactionText.gameObject.SetActive(false);
        }
    }

    private void GiveCocaineStock()
    {
        CocaineStockManager.instance.AddCocaineStock(stockAmount);
        Debug.Log("Cocaine Stock Given: " + stockAmount);
        audioSource.PlayOneShot(collectSound);
        interactionText.gameObject.SetActive(false);
    }
}
