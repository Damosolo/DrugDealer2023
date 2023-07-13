using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DrugTrigger : MonoBehaviour
{
    public List<GameObject> objectsToActivate;
    public List<GameObject> objectsToDeactivate;
    public TextMeshProUGUI promptText;
    public float activationDuration = 60f; // Duration in seconds

    private bool isInRange = false;
    private bool isActivated = false;
    private float activationTimer = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            promptText.text = "Press 'E' to do Drugs";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            promptText.text = "";
        }
    }

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E) && !isActivated)
        {
            ActivateObjects();
        }

        if (isActivated)
        {
            activationTimer += Time.deltaTime;

            if (activationTimer >= activationDuration)
            {
                DeactivateObjects();
            }
        }
    }

    private void ActivateObjects()
    {
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(true);
        }

        foreach (GameObject obj in objectsToDeactivate)
        {
            obj.SetActive(false);
        }

        isActivated = true;
        promptText.text = "";
    }

    private void DeactivateObjects()
    {
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objectsToDeactivate)
        {
            obj.SetActive(true);
        }

        isActivated = false;
        activationTimer = 0f;
    }
}
