using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public float rotationSpeed = 90f; // Speed of rotation in degrees per second
    public float rotationDuration = 2f; // Duration of rotation in seconds

    private float currentRotation = 0f;
    private float rotationTimer = 0f;

    void Update()
    {
        // Update the rotation timer
        rotationTimer += Time.deltaTime;

        if (rotationTimer <= rotationDuration)
        {
            // Calculate the rotation amount for this frame
            float rotationAmount = (rotationSpeed * Time.deltaTime) % 360f;

            // Update the current rotation
            currentRotation += rotationAmount;

            // Apply the rotation to the object
            transform.rotation = Quaternion.Euler(0f, currentRotation, 0f);
        }
    }
}
