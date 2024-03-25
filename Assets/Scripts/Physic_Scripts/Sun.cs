using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.SetActive(false);
        //Debug.Log("An object has entered the sun");
    }
}
