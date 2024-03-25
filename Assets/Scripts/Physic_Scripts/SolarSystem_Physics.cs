using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Clase encargada de llevar a cabo la física entre los cuerpos de un sistema solar
/// </summary>
public class SolarSystem_Physics : MonoBehaviour
{
    public static SolarSystem_Physics instance;

    public enum Purposes { Assasination, Collect, Rescue, Obtain_Information, Exploration }


    //This is temporal while I try to introduce Chatgpt into the project
    private string FileName = "\\Universidad\\tfg\\LaNueva\\Tfg\\Assets\\testing_Json\\prueba.Json";

    //Physics
    const float G = 100f; //Gravitational constant

    //Array de cuerpos
    const int max_Bodies = 100;
    int N_celestials = 0;
    GameObject[] celestials = new GameObject[max_Bodies];

    System_Generator generator;
    JSON_Parser parser;

    //This variable will be removed as soon as I get my Json directrly from chatgpt
    public string jsonText;

    private bool landed = false;

    private SolarSystem system_Information;

    private bool onUi = false;

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

    void Start()
    {
        generator = GetComponent<System_Generator>(); 
        parser = GetComponent<JSON_Parser>();
        //We generate the system in order to star physics with it
        //generator.generate();

        //parser.LoadJson(FileName);
        parser.LoadJsonFromGpt(jsonText);

        //InitialVelocity();
    }


    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (!landed) Gravity();

    }

    //Métodos físicos

    //Atracción entre cuerpos
    private void Gravity()
    {
        for (int i = 0; i < N_celestials; i++)
        {
            for (int j = 0; j < N_celestials; j++)
            {
                GameObject a = celestials[i];
                GameObject b = celestials[j];
                if (a != b)
                {
                    float mass1 = a.GetComponent<Rigidbody>().mass;
                    float mass2 = b.GetComponent<Rigidbody>().mass;

                    float r = Vector3.Distance(a.transform.position, b.transform.position);

                    //Fórmula de la fuerza gravitatoria
                    Vector3 grav = (b.transform.position - a.transform.position).normalized * (G * (mass1 * mass2) / (r * r));
                    a.GetComponent<Rigidbody>().AddForce(grav);
                }
            }
        }
    }

    //Rotation between bodies
    void InitialVelocity()
    {
        for (int i = 0; i < N_celestials; i++)
        {
            for (int j = 0; j < N_celestials; j++)
            {
                GameObject a = celestials[i];
                GameObject b = celestials[j];
                if (a != b)
                {

                    float mass2 = b.GetComponent<Rigidbody>().mass;

                    float r = Vector3.Distance(a.transform.position, b.transform.position);
                    a.transform.LookAt(b.transform); //Esto es sospechoso, quizás haya que cambiarlo (Puede producir que no roten sobre si mismos los planetas)
                    a.GetComponent<Rigidbody>().velocity += a.transform.right * Mathf.Sqrt((G * mass2) / r);
                }
            }
        }
    }

    //Public methods

    //Celestial bodies report their existence
    public void reportCelestial(GameObject c)
    {
        celestials[N_celestials] = c;
        N_celestials++;
    }

    public void JsonRead()
    {
        //Generate the system using Json
        parser.setGenerationData(generator);
        generator.generate();
        InitialVelocity();
    }


    public GameObject getCelestial(int index)
    {
        return celestials[index];
    }

    public int getNumberCelestials() { return N_celestials; }

    public void land() { landed = true; }

    public void takeOff()
    {
        landed = false;
        N_celestials = 0;
        generator.generate();
        InitialVelocity();
        UiController.instance.findText();
    }

    public void setInformation(SolarSystem system)
    {
        system_Information = system;
    }

    public string [] getFactions() { return system_Information.Factions; }

    public string[] getRaces() { return system_Information.Races; }

    public Character[] GetCharacters() { return system_Information.Characters; }

    public void uiOpen(bool open) { onUi = open; }  

    public SolarSystem GetSystem() { return system_Information; }
}
