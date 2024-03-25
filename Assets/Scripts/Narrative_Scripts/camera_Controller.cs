using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_Controller : MonoBehaviour
{
    [SerializeField]
    private Camera camera;

    [SerializeField]
    private float cameraOffset;

    [SerializeField]
    private float scrollVelocity;

    [SerializeField]
    private float rotVelocity;

    private int currentCelestial = 0;

    private float acumulatedDistance = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentCelestial++;
            if(currentCelestial >= SolarSystem_Physics.instance.getNumberCelestials())
            {
                currentCelestial = 0;
            }

            setCameraToBody();

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentCelestial--;
            if(currentCelestial < 0)
            {
                currentCelestial = SolarSystem_Physics.instance.getNumberCelestials() - 1;
            }
            setCameraToBody();
        }

        //So we can come closer or further from the planet we are seeing
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollWheelInput != 0)
        {
            Vector3 newPos = camera.GetComponent<Transform>().position;

            if (scrollWheelInput < 0)
                newPos.y += scrollVelocity;
            else newPos.y -= scrollVelocity;

            acumulatedDistance = newPos.y;

            camera.GetComponent<Transform>().position = newPos;
        }

        //Camera rotation
        if (Input.GetMouseButton(2)) 
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");


            RotateCamera(mouseX, mouseY);
        }
    }

    void RotateCamera(float mouseX, float mouseY)
    {
        // Calcular la rotación basada en el movimiento del ratón
        Vector3 rotation = new Vector3(-mouseY, mouseX, 0f) * rotVelocity;

        // Aplicar la rotación a la cámara
        camera.transform.Rotate(rotation);
    }

    private void setCameraToBody()
    {
        GameObject celestial = SolarSystem_Physics.instance.getCelestial(currentCelestial);

        Vector3 celestialPos = celestial.transform.position;

        celestialPos.y += cameraOffset + celestial.transform.localScale.y + acumulatedDistance;

        camera.GetComponent<Transform>().position = celestialPos;

        camera.GetComponent<Transform>().parent = celestial.transform;
    }
}
