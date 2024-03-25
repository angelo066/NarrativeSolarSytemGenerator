using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiController : MonoBehaviour
{
    public static UiController instance;

    [SerializeField]
    TextMeshProUGUI planetInfoText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void changePlanetInfo(Planet_Information planet)
    {
        //We show the planet information

        string finalMessage = "Surface:";

        finalMessage = finalMessage + planet.car.surface + "\n";

        finalMessage = finalMessage + "Temperature:" + planet.car.temperature + "\n"; 

        finalMessage = finalMessage + "Resources:" + "\n";

        for(int i = 0; i < planet.car.res.Length; i++)
        {
            finalMessage = finalMessage + planet.car.res[i] + "\n";
        }

        finalMessage = finalMessage + "Press L to land into this world";

        planetInfoText.text = finalMessage;
    }

    public void findText()
    {
        planetInfoText = FindObjectOfType<TextMeshProUGUI>();
    }

}
