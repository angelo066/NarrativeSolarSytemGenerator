using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipCameraController : MonoBehaviour
{
    public float rotationSpeed = 2.0f;
    public float thrustSpeed = 5.0f;
    public float strafeSpeed = 3.0f;
    public float mouseSensitivity = 2.0f;

    void Update()
    {
        // Rotaci�n horizontal con el movimiento del rat�n
        float horizontalMouseMovement = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, horizontalMouseMovement * rotationSpeed, 0);

        // Rotaci�n vertical con el movimiento del rat�n
        float verticalMouseMovement = -Input.GetAxis("Mouse Y") * mouseSensitivity;
        transform.Rotate(verticalMouseMovement * rotationSpeed, 0, 0);

        // Movimiento hacia adelante con W
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, 0, thrustSpeed * Time.deltaTime);
        }

        // Movimiento hacia atr�s con S
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, 0, -thrustSpeed * Time.deltaTime);
        }

        // Movimiento lateral con A y D
        float strafeMovement = Input.GetAxis("Horizontal") * strafeSpeed;
        transform.Translate(strafeMovement * Time.deltaTime, 0, 0);
    }
}

