using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satelite_Generator : MonoBehaviour
{
    public int satelites;

    public Vector3[] satelites_Dist;
    public float[] satelites_Masses;
    public float[] satelites_Rads;

    [SerializeField]
    GameObject moon;

    public void Generate()
    {
        for (int i = 0; i < satelites; i++)
        {
            //Instantiate and report the identety
            GameObject g = Instantiate(moon, satelites_Dist[i], Quaternion.identity);
            g.GetComponent<celestial>().report();

            //Change radioues according to user input
            float planetRad = satelites_Rads[i];
            g.transform.localScale = new Vector3(planetRad, planetRad, planetRad);

            //Change mass according to user input
            g.GetComponent<Rigidbody>().mass = satelites_Masses[i];
        }
    }
}
