using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script defines Clases so we can parse Json with the JsonUtility
 * function "FromJson"
 */

/*
 * The following clases are used to store
 * the information directly from the Json
 */
//[System.Serializable]
//public class SolarSystem
//{
//    public int Number_of_Planets;

//    public int Star_Size;   //Dawrf Normal or Giant

//    public Planet[] Planets;

//    public string History;  //General History of the solar system
//}

//[System.Serializable]
//public class Planet
//{
//    public string Name;

//    public PlanetCharacteristics Characteristics;

//    public int Satellites;

//    public int Type;    //If it is a fire Word a Water world a Gas World or a Frozen World

//    public bool Giant;  //If we want it to be giant or not
//}

//[System.Serializable]
//public class PlanetCharacteristics
//{
//    public string Surface;

//    public string Temperature;
//}

[System.Serializable]
public class SolarSystem
{
    public int Number_of_Planets { get; set; }
    public int Star_Size { get; set; }
    public Planet[] Planets { get; set; }
    public string History { get; set; }

    public string[] Factions { get; set; }

    public string[] Races { get; set; }
    public Character[] Characters { get; set; }
}
[System.Serializable]
public class Planet
{
    public string Name { get; set; }
    public PlanetCharacteristics Characteristics { get; set; }
    public int Satellites { get; set; }

    //Parses into enum
    public int Type { get; set; }
    public bool Giant { get; set; }

    public bool Inhabited { get; set; }

    //Determines relations with each planet, including itself(always 0)
    public int[] Relations { get; set; }

    //Parses into enum
    public int Goverment { get; set; }
}
[System.Serializable]
public class PlanetCharacteristics
{
    public string Surface { get; set; }
    public string Temperature { get; set; }

    //Parses into enum
    public int[] Resources { get; set; }

}
[System.Serializable]
public class Character
{
    public string Name { get; set; }
    public string Faction { get; set; }

    public string Dialogue { get; set; }

    //Parses into enum
    public int Purpose { get; set; }

    //Other characters
    public int[] Enemies { get; set; }

    public string Race { get; set; }

    public int Planet { get; set; }

    public string Description { get; set; }

}

 
//Estaría guapo:
//lo del color(complicat pero viable).
//Ui para ver las facciones, personajes etc..(Important),
//Saber que se puede y que no se puede.
// Uno aleatorio para la gente que no quiere pensar fr fr
//Guerra civil
//Ajustar los diálogos a los personajes(Ponerles diferente vocabulario y/o idiomas)
//No dar por hecho que hay una sola especie por planeta(se puede añadir un array a la clase)
//Varios personajes por raza(y que no necesariamente sean humanoides)
// Cosas orgánicas como madera o piedras en los recursos

///MMETERLE LAS NORMAS DE NARRATIVA
///Especificar que meta las razas que ya ha nombrtado él mismo, que no se invente nuevas para los personajes

//Knowing that:
// -Star_Size = 0 equals Dawrf star, Star_Size = 1 equals Normal star and Star_Size = 2 equals giant star
// -Type = 0 equals FirePlanet, Type = 1 equals Water planet, Type = 2 equals Gas planet and Type = 3 equals frozen planet
// -Relations = 0 means its the same planet, Relations = 1 means peace, Relations = 2 means politic tension and Relations = 3 means war
// -Goverment = 0 means Monarchy, Goverment = 1 means Republic, Goverment = 2 means Democracy, Goverment = 3 means Autoritarian_Fascist, Goverment = 4 means Autoritarian_Comunist, Goverment = 5 means War_Clans, Goverment = 6 means Inhabited
// -Purpose = 0 means Assasination, Purpose = 1 means Collect an item, Purpose = 2 means Rescue somebody, Purpose = 3 means Obtain_information, Purpose = 4 means Exploration
// -Resources = 0 means Food, Resources = 1 means Minerals, Resources = 2 means Magic, Resources = 3 means Weapong, Resources = 4 means slaves
// -Charactes must have dialogue and name, planets must have names too 
// -Planets with Inhabited = false must have the Not_habited goverment, wich means Goverment must equal 6, and they cant have Weapons, nor Slaves as resources, they also cant be in war with anyone
// -You cant name the planets: Planet1, Planet2... etc. Same with characters and races, u must come up with actual names
// -Enemies must come determined by a number, meaning that if Character number 3 is enemy with character number 0, its array must contain number 0, if it is enemy with number 3 it must contain number 3
// -Characters cant be enemies with themselves
// -This is the class im going to serialize the Json into
//[System.Serializable]
//public class SolarSystem
//{
//    public int Number_of_Planets { get; set; }
//    public int Star_Size { get; set; }
//    public Planet[] Planets { get; set; }
//    public string History { get; set; }

//    public string[] Factions { get; set; }

//    public string[] Races { get; set; }
//    public Character[] Characters { get; set; }
//}
//[System.Serializable]
//public class Planet
//{
//    public string Name { get; set; }
//    public PlanetCharacteristics Characteristics { get; set; }
//    public int Satellites { get; set; }

//    //Parses into enum
//    public int Type { get; set; }
//    public bool Giant { get; set; }

//    public bool Inhabited { get; set; }

//    //Determines relations with each planet, including itself(always 0)
//    public int[] Relations { get; set; }

//    //Parses into enum
//    public int Goverment { get; set; }
//}
//[System.Serializable]
//public class PlanetCharacteristics
//{
//    public string Surface { get; set; }
//    public string Temperature { get; set; }

//    //Parses into enum
//    public int[] Resources { get; set; }

//}
//[System.Serializable]
//public class Character
//{
//    public string Name { get; set; }
//    public string Faction { get; set; }

//    public string Dialogue { get; set; }

//    //Parses into enum
//    public int Purpose { get; set; }

//    //Other characters
//    public Character[] Enemies { get; set; }

//    public string Race { get; set; }

//    public int Planet { get; set; }

//    public string Description { get; set; }

//}
//Give me a Json, and only the Json please, dont add anymore text, that follows the following description,And give it to me in a Json format that I can copy paste please
/// Hace mucho muchisimo tiempo estalló una guerra interplanetaria en un sistema de cuyo nombre no recuerdo ya. Ocurrieron sucesos devastadores, y fue una guerra
/// como cualquier otra donde no ganó ningún bando. Los planetas quedaron destrozados y sus habitantes tuvieron que aclimatarse a los cambios climáticos y a las
/// catástrofes medioambientales para sobrevivir. 
/// De los 7 planetas que existían en el planeta solar, solo quedaron 3 planetas habitables por los seres vivos, el resto sucumbieron a la catarsis y al colapso
/// absoluto. El primer planeta habitable, Wakanda estaba formado por diversas especies con diferentes desarrollos tecnológicos, algunas especies simplemente
/// sobrevivían gracias a la pequeña cadena trófica que existía, otras especies se habían acabado asentando en un estado tribal, y luego había una ciudad habitada
/// por la especie predominante, los wakandianos.
/// El segundo planeta habitable estaba formado por especies más desarrolladas con civilizaciones con diversas culturas, aunque la codicia y la avarícia fueron las
/// consecuencias de la actual guerra civil que desarrolla.
/// El tercer planeta está habitado por seres ancestrales conocidos por el resto de planetas como "Los Complutenses" cuya magnitud como seres vivos abarcaba
/// centenares de metros y eran seres colosales milenarios.
/// El resto de planetas inhabitables tienen diferentes condiciones, uno es un planeta de fuego donde se dice que vive el rey del fuego, y otro es un planeta helado
/// que se dice que guarda la más preciosas de las joyas en su núcleo.
/// 
/// Soy un explorador del espacio profundo y he descubierto este sistema solar, pero necesito más información para sobrevivir a este sistema solar y extraer las
/// riquezas que pueda albergar. Me tendré que preparar una nave gigante con todas las maquinas y viveres que me dejen albergarme en este sistema. Gracias.


//Give a solar sytem with four planets, one of each kind, 3 of them are
//at war with each other and give me at least 4 characters
