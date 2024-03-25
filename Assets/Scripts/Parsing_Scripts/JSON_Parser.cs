using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class JSON_Parser : MonoBehaviour
{
    private SolarSystem solarSytemParsed;
    private Physic_Ranges ranges; //Class used to generate every physic aspect the user doesnt specify
    // Start is called before the first frame update

    int planetPositioningIndex = 0;

    /*
     * Creo que para hacer la busqueda lo mas eficiente posible
     * lo suyo seria añadirle al mensaje que el usuario manda al chatgpt
     * un añadido de "Estructuramelo como este ejemplo
     */


    public void LoadJson(string file)
    {
        if (File.Exists(file))
        {
            try
            {
                StreamReader sr = new StreamReader(file);

                string json = sr.ReadToEnd();

                solarSytemParsed = JsonUtility.FromJson<SolarSystem>(json);

                SolarSystem_Physics.instance.JsonRead();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error al deserializar el JSON: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("Path doesn´t exist");
        }


    }

    public void LoadJsonFromGpt(string json)
    {

        bool valid = false;
        Json_Narrative_Checker checker = new Json_Narrative_Checker();

        do
        {
            try
            {
                solarSytemParsed = new SolarSystem();
                solarSytemParsed = JsonConvert.DeserializeObject<SolarSystem>(json);

                Json_Narrative_Checker.Reasons reason = checker.checkNarrative(solarSytemParsed);
                //while (reason != Json_Narrative_Checker.Reasons.Interesting_Story){

                    //Add whatever it needs to the AI petition
                    if(reason == Json_Narrative_Checker.Reasons.Boring_Relations)
                    {

                    }
                    else if(reason == Json_Narrative_Checker.Reasons.Not_Enough_Characters)
                    {

                    }
                    else if(reason == Json_Narrative_Checker.Reasons.Not_Enough_Enemies)
                    {

                    }
                    else if(reason == Json_Narrative_Checker.Reasons.Not_Enough_Factions)
                    {

                    }
                    else if(reason == Json_Narrative_Checker.Reasons.Not_Enough_Habited)
                    {

                    }
                    else if(reason == Json_Narrative_Checker.Reasons.Not_Enough_Planets)
                    {

                    }

                //}

                valid = true;
            }
            catch (JsonException ex)
            {
                //Here we would ask the AI why is it wrong and make it right.
                Debug.Log($"Error al deserializar el JSON: {ex.Message}");
            }
        }
        while (!valid);

        SolarSystem_Physics.instance.JsonRead();
    }

    //Se give the generator the numbers we took from JSON file
    public void setGenerationData(System_Generator generator)
    {
        SolarSystem_Physics.instance.setInformation(solarSytemParsed);

        ranges = new Physic_Ranges();
        int n_Planets = solarSytemParsed.Number_of_Planets;
        int starSize = ranges.generate_Star_Rad(solarSytemParsed.Star_Size);
        int starMass = ranges.generate_Star_Mass(starSize);

        //Star generated
        generator.setN_Planets(n_Planets);
        generator.set_Star_Size(starSize);
        generator.set_Star_Mass(starMass);

        //Generate planets
        int totalSatellites = 0;
        List<int> satellites = new List<int>();
        Planet[] planets = solarSytemParsed.Planets;
        float[] rads = new float[n_Planets];
        float[] mass = new float[n_Planets];
        Vector3[] dist = new Vector3[n_Planets];

        for (int i = 0; i < n_Planets; i++)
        {
            Planet currentPlanet = planets[i];

            Vector3 newDist = new Vector3(0, 0, 0);
            newDist.x = ranges.generate_Planet_Distance(starSize, currentPlanet.Type);

            if (planetPositioningIndex <= 3) newDist = cardinalPlacement(planetPositioningIndex, newDist);
            else if (planetPositioningIndex <= 7) newDist = corners(planetPositioningIndex, newDist);
            else planetPositioningIndex = 0;

            rads[i] = ranges.generate_Planet_Rad(currentPlanet.Giant, starSize);
            mass[i] = ranges.generate_Planet_Mass(rads[i]);
            dist[i] = newDist;

            satellites.Add(currentPlanet.Satellites);
            totalSatellites += currentPlanet.Satellites;

            planetPositioningIndex++;
        }

        generator.set_Planets(rads, mass, dist, planets);
        generator.set_Satellites_Numbers(satellites);

        //Generate Satellites
        int currentSatellite = 0;   //Iterate Satellites in the general array
        float[] satellite_Rads = new float[totalSatellites];
        float[] satellite_Mass = new float[totalSatellites];
        Vector3[] satellite_Dist = new Vector3[totalSatellites];

        //Iterate Planets
        for (int i = 0; i < planets.Length; i++)
        {
            //Iterate satellites
            for (int j = 0; j < satellites[i]; j++)
            {
                float planet_Rad = rads[i];
                float planet_Pos = dist[i].x;

                //Position generated from planet_Radious and planet Position
                float distance = ranges.generate_Satellite_Distance(planet_Rad, planet_Pos);
                Vector3 newDist = new Vector3(distance, 0, 0);

                if (Mathf.Abs(dist[i].z) > Mathf.Abs(dist[i].x))
                {
                    planet_Pos = dist[i].z;
                    newDist.z = dist[i].z;
                }
                float satellite_Rad = ranges.generate_Satellites_Rads(planet_Rad);
                satellite_Rads[currentSatellite] = satellite_Rad;
                satellite_Mass[currentSatellite] = ranges.generate_Satellites_Masses(satellite_Rad);



                if (j <= 3) newDist = cardinalPlacement(j, newDist, new Vector2(dist[i].x, dist[i].z));
                else
                {
                    newDist = corners(j, newDist, planet_Pos);
                }

                satellite_Dist[currentSatellite] = newDist;

                currentSatellite++;
            }

        }

        generator.set_Satellites(satellite_Rads, satellite_Mass, satellite_Dist);

    }
    /// <summary>
    /// Sets the remaining planets across the circunference
    /// </summary>
    /// <param name="currentPlanet"> the planet that we changing</param>
    /// <param name="newDist">the distance we are setting it to</param>
    /// /// <param name="center"> so we can use it with the star and the planets</param>
    /// <returns></returns>
    private Vector3 corners(int n, Vector3 newDist, float center = 0)
    {
        float aux = newDist.x;
        if (n == 4)   //Top right corner
        {
            newDist.x = newDist.x * Mathf.Cos(Mathf.PI / 4);
            newDist.z = aux * Mathf.Sin(Mathf.PI / 4);
        }
        else if (n == 5)  //Top left corner
        {
            newDist.x = -newDist.x * Mathf.Cos(Mathf.PI / 4);
            newDist.z = aux * Mathf.Sin(Mathf.PI / 4);
        }
        else if (n == 6)  //Bottom right corner
        {
            newDist.x = newDist.x * Mathf.Cos(Mathf.PI / 4);
            newDist.z = -aux * Mathf.Sin(Mathf.PI / 4);
        }
        else if (n == 7)
        {
            newDist.x = -newDist.x * Mathf.Cos(Mathf.PI / 4);
            newDist.z = -aux * Mathf.Sin(Mathf.PI / 4);

        }

        return newDist;
    }



    /// <summary>
    /// This method is used to distribute various planets of the same type across the system
    /// </summary>
    /// <param name="n"> the planet we are changing</param>
    /// <param name="newDist"> the distance of said planet</param>
    /// <param name="center"> so we can use it with the star and the planets</param>
    /// <returns></returns>
    private Vector3 cardinalPlacement(int n, Vector3 newDist, Vector2 center = new Vector2())
    {
        //We place them across the cardinal points
        if (n == 1)
        {
            float diff = newDist.x - center.x;
            newDist.x = center.x - diff;
        }
        else if (n == 2)
        {
            float diff = newDist.x - center.x;
            newDist.z = center.y - diff;
            newDist.x = 0;
        }
        else if (n == 3)
        {
            float diff = newDist.x - center.x;
            newDist.z = center.y + diff;
            newDist.x = 0;
        }

        return newDist;
    }


    /// <summary>
    /// this function determines if a determined satellite is in its logical range. 
    /// Logical beeing between its own planet and the next. And not inside other satellite of said planet
    /// </summary>
    /// <param name="nextPlanetPos"> Position of next planet. Default value for last planet </param>
    /// <param name="nextPlanetRad"> Radious of next planet. Default value for last planet </param>
    /// <param name="satellites">position of same planet satellites</param>
    /// <param name="satellitesRads">Rads of same planet satellites</param>
    /// <param name="s">Index of the current satellite</param>
    /// <returns></returns>
    private bool inRange(float[] satellites, float[] satellitesRads, int s,
        float nextPlanetPos = float.MaxValue, float nextPlanetRad = float.MaxValue / 2)
    {
        bool inRange = true;

        //Dimensions and position of current satellite
        float position = satellites[s];
        float rad = satellitesRads[s];

        //It appeared further than the next planet or inside it
        if (position > nextPlanetPos || position + rad >= nextPlanetPos - nextPlanetRad) return false;

        //We check every satellite until the one we are checking right now
        for (int i = 0; i < s; i++)
        {

            //We always check with the previous satellite
            if (position - rad <= satellites[i] + satellitesRads[i]) return false;
        }


        return inRange;
    }

}
