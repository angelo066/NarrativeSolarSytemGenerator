using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui_Manager : MonoBehaviour
{

    private Vector3 startignPos = new Vector3(5, -10, 0);
    private List<GameObject> objectsToDestroy = new List<GameObject>();

    [SerializeField]
    Image fondo;

    [SerializeField]
    Sprite redPlanet;

    [SerializeField]
    Sprite bluePlanet;

    [SerializeField]
    Sprite gasPlanet;

    [SerializeField]
    Sprite icePlanet;

    //The gameObject that will contain the sprite we sill show
    [SerializeField]
    GameObject emptySprite;


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool uiOpen = !fondo.IsActive();
            fondo.gameObject.SetActive(uiOpen);
            SolarSystem_Physics.instance.uiOpen(uiOpen);

            if(!uiOpen) flush();
            else showPlanets();
        }
    }

    void showPlanets()
    {
        SolarSystem solarSystem = SolarSystem_Physics.instance.GetSystem();


        for (int i = 0; i < solarSystem.Number_of_Planets; i++)
        {
            GameObject newSprite = Instantiate(emptySprite, startignPos, Quaternion.identity);
            newSprite.transform.SetParent(fondo.transform, false);
            objectsToDestroy.Add(newSprite);

            Planet info = solarSystem.Planets[i];

            setSprite(newSprite, info);

            startignPos.x += 15;
            if(startignPos.x > GetComponent<RectTransform>().rect.width)
            {
                startignPos.x = 5;
                startignPos.y -= 15;
            }
        }
    }

    private void setSprite(GameObject newSprite, Planet info)
    {
        if (info.Type == 0)
            newSprite.GetComponent<Image>().sprite = redPlanet;
        else if (info.Type == 1)
            newSprite.GetComponent<Image>().sprite = bluePlanet;

        else if (info.Type == 2)
            newSprite.GetComponent<Image>().sprite = gasPlanet;
        else
            newSprite.GetComponent<Image>().sprite = icePlanet;

    }

    void flush()
    {
        for(int i = 0; i < objectsToDestroy.Count; i++)
        {
            Destroy(objectsToDestroy[i]);
        }
        objectsToDestroy.Clear();
    }

}

