using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Json_Narrative_Checker : MonoBehaviour
{
    public enum Reasons { Interesting_Story, Not_Enough_Planets, Not_Enough_Factions, Not_Enough_Habited, Not_Enough_Characters,
                          Boring_Relations, Not_Enough_Enemies}

    public Reasons checkNarrative(SolarSystem system)
    {
        //Number of planets
        if (system.Number_of_Planets < 1) return Reasons.Not_Enough_Planets; //Either theres no planet or the Ai has failed

        //Number of factions
        if (system.Factions.Length < 2) return Reasons.Not_Enough_Factions;

        //Number of inhabited Planets
        int habited = 0;

        for(int i = 0; i < system.Number_of_Planets; i++)
        {
            if (system.Planets[i].Inhabited) habited++;
        }

        if (habited < 2) return Reasons.Not_Enough_Habited;

        //Number of characters
        if (system.Characters.Length < 2) return Reasons.Not_Enough_Characters;

        //Number of interesting relations between planets
        bool at_least_tension = false;
        int index = 0;
        while(index < system.Planets.Length && !at_least_tension)
        {
            int relations_index = 0;

            while(relations_index < system.Planets[index].Relations.Length && !at_least_tension)
            {
                if(system.Planets[index].Relations[relations_index] == 2 ||
                    system.Planets[index].Relations[relations_index] == 3)
                {
                    at_least_tension = true;
                }
                relations_index++;
            }

            index++;
        }

        if (!at_least_tension) return Reasons.Boring_Relations;

        int character_Index = 0;
        int enemies = 0;
        while(character_Index < system.Characters.Length)
        {
            enemies += system.Characters[character_Index].Enemies.Length;
            character_Index++;
        }

        if (enemies < 1) return Reasons.Not_Enough_Enemies;

        return Reasons.Interesting_Story;
    }
}
