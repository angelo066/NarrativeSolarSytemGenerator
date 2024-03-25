using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_Generator : MonoBehaviour
{
    //Prefabs of the celestial bodies
    [SerializeField]
    private GameObject celestialObject;
    [SerializeField]
    private GameObject star;

     float starRadious;
     float starMass;

    //Materiales to asign to planets depending of their proximity to the star/ size / composition/ etc
    [SerializeField]
    private Material hotPlanet;
    [SerializeField]
    private Material waterPlanet;
    [SerializeField]
    private Material gasPlanet; 
    [SerializeField]
    private Material frozenPlanet;


    [SerializeField]
    GameObject enemyShip;

    //Number of planets and stars that we want in our system
    private int N_Planets;

    //Stars is 1 unless the development goes really well
    private int N_stars = 1;

    //Distances and sizes of the planets.
    //Distances counting from point (0,0,0) the main star
    //Each of this arrays is in order. Meaning, first distances, first radious and first mass all correspond to the first planet
    
    private Vector3[] planet_Distances;
    
    private float[] planet_Radious;
    
    private float[] planet_Masses;

    private Planet[] planets;

    /// <summary>
    /// Information of satelites for each planet
    /// </summary>
    
    private int[] satellites;
    
    private Vector3[] satellites_Distances;
    
    private float[] satellites_Rads;
    
    private float[] satellites_Masses;


    //Mininum habitable distance with Sun  = 1200 (Unity units)
    //Maximum distance with Sun = 2000(Unity units)
    //Sun size 696(Unity units)
    private const float Sun_max_hab_distance = 2000;
    private const float Sun_min_hab_distance = 1200;
    private const float sun_Radious = 696;

    public void generate()
    {
        generateStars();

        generatePlanets();
    }

    private void generatePlanets()
    {
        int last_index = 0; //Tracks the satelite information
        //Generate planets
        for (int i = 0; i < N_Planets; i++)
        {
            //Instantiate and report the identety
            GameObject g = Instantiate(celestialObject, planet_Distances[i], Quaternion.identity);
            g.GetComponent<celestial>().report();

            //Change radioues according to user input
            float planetRad = planet_Radious[i];
            g.transform.localScale = new Vector3(planetRad, planetRad, planetRad);

            //Change mass according to user input
            g.GetComponent<Rigidbody>().mass = planet_Masses[i];

            Planet_Information info = g.GetComponent<Planet_Information>();

            info.setInfo(planets[i]);

            Character[] characters = SolarSystem_Physics.instance.GetCharacters();

            for (int character_index = 0; character_index < characters.Length; character_index++)
            {
                if (characters[character_index].Planet == i) info.addCharacter(characters[character_index]);
            }

            setMaterials(i, g);

            //Generation of satelites
            last_index = generateSatelites(last_index, i, g);

            //If it is Inhabited
            if (planets[i].Inhabited)
            {
                Vector3 shipPlacement = planet_Distances[i];

                shipPlacement.y += planetRad + 20;

                GameObject ship = Instantiate(enemyShip, shipPlacement, Quaternion.identity);

                ship.transform.parent = g.transform;

                //ship.GetComponent<enemyShips>().setPlayer(SolarSystem_Physics.instance.getPlayer());
                //ship.GetComponent<enemyShips>().setPlayer(GameObject.Find("spacePlayer"));
            }
        }
    }

    private int generateSatelites(int last_index, int i, GameObject g)
    {
        int n_satelites = satellites[i];

        Vector3[] positions = new Vector3[n_satelites];
        float[] masses = new float[n_satelites];
        float[] rads = new float[n_satelites];

        int p;
        for (p = 0; p < n_satelites; p++)
        {
            positions[p] = satellites_Distances[last_index + p];
            masses[p] = satellites_Masses[last_index + p];
            rads[p] = satellites_Rads[last_index + p];

        }
        last_index += p;

        Satelite_Generator sat_Gen = g.GetComponent<Satelite_Generator>();

        sat_Gen.satelites = n_satelites;
        sat_Gen.satelites_Dist = positions;
        sat_Gen.satelites_Masses = masses;
        sat_Gen.satelites_Rads = rads;

        sat_Gen.Generate();
        return last_index;
    }

    private void setMaterials(int i, GameObject g)
    {
        Planet_Information planet = g.GetComponent<Planet_Information>();

        //Is closer to star
        if (planet.Type == 0) g.GetComponent<Renderer>().material = hotPlanet;
        //Habitable planet
        else if (planet.Type == 1) g.GetComponent<Renderer>().material = waterPlanet;
        //Far, but not far enough to be frozen
        else if (planet.Type == 2) g.GetComponent<Renderer>().material = gasPlanet;
        //Frozen
        else if (planet.Type == 3) g.GetComponent<Renderer>().material = frozenPlanet;
    }

    private void generateStars()
    {
        //Generate stars
        for (int i = 0; i < N_stars; i++)
        {
            GameObject g = Instantiate(star, new Vector3(0, 0, 0), Quaternion.identity);
            g.transform.localScale = new Vector3(starRadious, starRadious, starRadious);
            g.GetComponent<Rigidbody>().mass = starMass;
            g.GetComponent<celestial>().report();
        }
    }

    private void checkArraySizes()
    {
        if (planet_Distances.Length != N_Planets)
        {
            Debug.LogError("Number of distances must be the same as number of planets");
            Debug.Break();
        }

        if (planet_Radious.Length != N_Planets)
        {
            Debug.LogError("Number of radious must be the same as number of planets");
            Debug.Break();
        }

        if (planet_Masses.Length != N_Planets)
        {
            Debug.LogError("Number of masses must be the same as number of planets");
            Debug.Break();
        }
    }


    ///// JSON loading Region
    ///

    public void setN_Planets(int n) { N_Planets = n; }
    public void set_Star_Size(int r) { starRadious = r; }
    public void set_Star_Mass(int m) { starMass = m; }

    public void set_Planets(float[] rads, float[] mass, Vector3[] dist, Planet[] planets_info)
    {
        planet_Radious = new float[rads.Length];
        planet_Masses = new float[mass.Length];
        planet_Distances = new Vector3[dist.Length];

        planets = planets_info;

        for(int i = 0; i < rads.Length; i++)
        {
            planet_Radious[i] = rads[i];
            planet_Masses[i] = mass[i];
            planet_Distances[i] = dist[i];
        }
    }

    public void set_Satellites_Numbers(List<int> satel)
    {
        satellites = new int[satel.Count];
        for(int i = 0; i < N_Planets; i++)
        {
            satellites[i] = satel[i];
        }
    }

    public void set_Satellites(float[] rads, float[]mass, Vector3[] dist)
    {
        satellites_Rads = new float[rads.Length];
        satellites_Masses = new float[mass.Length];
        satellites_Distances = new Vector3[dist.Length];

        //We use rads.Lengt because Satellites contains the exact number
        //each planet has (including 0)
        for(int i = 0; i < rads.Length; i++)
        {
            satellites_Rads[i] = rads[i];
            satellites_Masses[i] = mass[i];
            satellites_Distances[i] = dist[i];
        }
    }
}
