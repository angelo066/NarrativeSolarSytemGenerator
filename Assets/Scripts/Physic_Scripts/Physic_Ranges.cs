using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This script contains a series of ranges
 that are viable for planets, stars and satelites

We use this so the user doesnt need to specify the radius of the star
or the mass of every satelite.

If it is not specified, it will be given a beliable number
 */
public class Physic_Ranges
{

    public enum StarSize { Dawrf, Normal, Giant};
    public enum PlanetType { Fire, Water, Gas, Frozen};

    private int min_Star_Rad = 200;  //If the user specifies Darf
    private int max_Star_Rad = 2000; //If the user specifies giant


    private int min_Planet_Rad = 10;
    private int max_Planet_Rad = 1000; //For large stars

    //Sun numbers so we can calculate zones with our star
    private const int Sun_max_hab_distance = 2000;
    private const int Sun_min_hab_distance = 1200;
    private const int sun_Radious = 696;

    private const int sunMass = 333000;

    //Earth radious so we can calculate masses according to it
    private const float earthRad = 6.3f;

    //Moon Rad and mass so we can calculate satellite masses according to it
    private const float moonRad = 1.7f;
    private const float moonMass = 0.01f;

    /// Everything is calculed using star size and mass,
    /// so every planet and satellite properties are relative
    /// to its mother star

    //Generate a random size for our star
    public int generate_Star_Rad(int star_Size)
    {
        StarSize star = (StarSize)star_Size;
        if(star == StarSize.Giant)
        {
            return Random.Range(max_Star_Rad - 300,
                max_Star_Rad);
        }

        if (star == StarSize.Dawrf)
        {
            return Random.Range(min_Star_Rad,
                min_Star_Rad + 500);
        }

        //If Normal

        return Random.Range(min_Star_Rad + 500,
                max_Star_Rad + 500);
    }

    //Generate mass according to star size
    public int generate_Star_Mass(int rad)
    {
        return (rad * sunMass) / sun_Radious;
    }

    //Place planets in distance dependig of their nature
    public int generate_Planet_Distance(int star_Rad, int planet_type)
    {
        //Habitalbe zones (Water planet zones)
        int minDist = (star_Rad * Sun_min_hab_distance) / sun_Radious;
        int maxDist = (star_Rad * Sun_max_hab_distance) / sun_Radious;

        PlanetType type = (PlanetType)planet_type;

        if(type == PlanetType.Fire)
        {
            int minFire = minDist - star_Rad/2;

            return Random.Range(minFire, minDist);
        }

        if(type == PlanetType.Water)
        {
            return Random.Range(minDist, maxDist);
        }

        if(type == PlanetType.Gas)
        {
            return Random.Range(maxDist, (int)maxDist*2);
        }

        //Frozen
        return Random.Range((int)maxDist * 2, (int)maxDist * 4);
    }

    //Generate Planet Rad depending if it is giant or not
    public int generate_Planet_Rad(bool giant, int star_Size)
    {
        if (giant) return Random.Range(max_Planet_Rad / 2, max_Planet_Rad);
        
        //Normal
        return Random.Range(min_Planet_Rad, max_Planet_Rad/2);
    }

    //Generate Planet Mass using earth mass
    public float generate_Planet_Mass(float planet_Rad)
    {
        return planet_Rad / earthRad;      //Because earth mass equals 1
    }

    //Generate Satellite distances according to their planet Radious
    public float generate_Satellite_Distance(float planet_Rad, float planet_Pos)
    {
        return Random.Range(planet_Pos + (planet_Rad * 2), planet_Pos + (planet_Rad * 4));
    }

    public float generate_Satellite_Distance(float planet_Rad, float planet_Pos, float nextPlanet, float nextPlanetRad)
    {
        return Random.Range(planet_Pos + planet_Rad , nextPlanet - nextPlanetRad);
    }

    //Generate Satellite rads according to their planet
    public float generate_Satellites_Rads(float planet_Rad)
    {
        return Random.Range(planet_Rad / 15, planet_Rad / 2);
    }

    //Generate Satellite masses according to moon mass
    public float generate_Satellites_Masses(float satellite_Rad)
    {
        return (satellite_Rad * moonMass) / moonRad;
    }
    
}
