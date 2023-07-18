using UnityEngine;
using TMPro;

public class WeedStockCollect : MonoBehaviour
{
    public int stockAmount = 5; // Amount of weed stock to give
    public AudioClip collectSound; // Sound effect to play when stock is collected
    public TextMeshProUGUI interactionText; // Reference to the TextMeshProUGUI component

    private bool canInteract = false; // Flag to track if the player can interact

    private AudioSource audioSource; // AudioSource component to play sound effects

    private void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // If there isn't an AudioSource component attached to the game object, add one
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Hide the interaction text initially
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            GiveWeedStock();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            if (interactionText != null)
            {
                interactionText.text = "Press 'E' to Harvest";
                interactionText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }

    private void GiveWeedStock()
    {
        WeedStockManager.instance.AddWeedStock(stockAmount);
        Debug.Log("Weed Stock Given: " + stockAmount);

        // Play the collect sound effect
        if (collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
        }

        // Hide the interaction text after stock is collected
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }
}
