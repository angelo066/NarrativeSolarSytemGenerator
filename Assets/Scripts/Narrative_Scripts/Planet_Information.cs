using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script used to store the information about the planet and to show it
 * when neccesary
 */

public class Planet_Information : MonoBehaviour
{
    //Public variables because user is not going to going to have access to this class
    public enum Resources { Food, Minerals, Magic, Weapons, Slaves }
    public enum Goverment {Monarchy, Republic, Democracy, Autoritarian_Fascist, Autoritarian_Communist, War_Clans, Not_Habited}

    public enum Relations { Same_Territory, Peace, Tension, War }
    public struct Caracteristics
    {
        public string surface;
        public string temperature;

        public Resources[] res;
    }

    public string Name;

    public Caracteristics car;

    public int satelites;

    public int Type;

    public bool Inhabited;

    public Goverment gov;

    List<Character> characters;

    public Relations[] relations;

    internal void setInfo(Planet planet)
    {
        //Ass we only call setInfo once i can initialize its Character list here
        characters = new List<Character>();

        Name = planet.Name;

        car.surface = planet.Characteristics.Surface;
        car.temperature = planet.Characteristics.Temperature;

        car.res = new Resources[planet.Characteristics.Resources.Length];
        for(int i = 0; i< planet.Characteristics.Resources.Length; i++){
            int bobobo = planet.Characteristics.Resources[i];
            car.res[i] = (Resources)bobobo;
        }


        satelites = planet.Satellites;

        Type = planet.Type;

        Inhabited = planet.Inhabited;

        gov = (Goverment)planet.Goverment;


        relations = new Relations[planet.Relations.Length];

        for(int i = 0; i < relations.Length; i++)
        {
            relations[i] = (Relations)planet.Relations[i];
        }
    }

    public void addCharacter(Character c)
    {
        characters.Add(c);
    }
}
