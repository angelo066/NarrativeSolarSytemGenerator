using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class planetTrigger : MonoBehaviour
{
    bool inrange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<shipCameraController>())
        {
            Planet_Information parent = gameObject.GetComponentInParent<Planet_Information>();
            UiController.instance.changePlanetInfo(parent);
            inrange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<shipCameraController>())
            inrange = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && inrange)
        {
            Planet_Information parent = gameObject.GetComponentInParent<Planet_Information>();
 
            switch (parent.Type)
            {
                case 0:
                    SceneManager.LoadScene(1);
                    break;
                case 1:
                    SceneManager.LoadScene(2);
                    break;
                case 3:
                    SceneManager.LoadScene(3);
                    break;

                default:
                    break;
            }

            SolarSystem_Physics.instance.land();

        }
    }
}
