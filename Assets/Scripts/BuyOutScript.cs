using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BuyOutScript : MonoBehaviour
{
    public MoneyManager moneyManager; // Reference to your MoneyManager
    public TextMeshProUGUI promptText; // Reference to your TextMeshPro component

    private bool playerInRange = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Assuming the player's tag is "Player"
        {
            playerInRange = true;
            promptText.text = "Press E to Buy-Out 50k";
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Assuming the player's tag is "Player"
        {
            playerInRange = false;
            promptText.text = "";
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (moneyManager.money >= 50000) // Using moneyManager.money to get the player's balance
            {
                // Deduct the money and load main menu
                moneyManager.SpendMoney(50000); // Assuming you have a method to spend money
                SceneManager.LoadScene("MainMenu"); // Assuming your main menu scene is named "MainMenu"
            }
            else
            {
                promptText.text = "Insufficient funds";
            }
        }
    }
}
