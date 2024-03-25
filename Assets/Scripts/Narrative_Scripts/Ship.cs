using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ship : MonoBehaviour
{
    bool inrange = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<outOutOfShit_Movement>())
            inrange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<outOutOfShit_Movement>())
            inrange = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && inrange)
        {
            SceneManager.LoadScene(0);
            SceneManager.sceneLoaded += takeOffCallback;
            //SolarSystem_Physics.instance.takeOff();
        }

    }

    private void takeOffCallback(Scene scene, LoadSceneMode mode)
    {
        SolarSystem_Physics.instance.takeOff();
    }
}
