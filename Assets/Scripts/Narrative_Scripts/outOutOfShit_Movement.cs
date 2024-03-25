using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outOutOfShit_Movement : MonoBehaviour
{

    float speed = 15f;
    float rotationSpeed = 180f;

    void Update()
    {
        // Get the movement inputs
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        // Calculate movement based on inputs
        Vector3 movement = new Vector3(horizontalMovement, 0f, verticalMovement) * speed * Time.deltaTime;

        // Apply movement to the character
        transform.Translate(movement);

        // Get rotation on the Y axis based on turn inputs
        float rotationY = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        // Apply rotation to the character
        transform.Rotate(Vector3.up, rotationY);
    }
}
