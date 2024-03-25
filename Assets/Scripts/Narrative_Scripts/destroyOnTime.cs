using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyOnTime : MonoBehaviour
{
    public float timeToDestroy;
    float timer = 0.0f;

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;

        if(timer >= timeToDestroy)
        {
            Destroy(this);
        }

    }
}
