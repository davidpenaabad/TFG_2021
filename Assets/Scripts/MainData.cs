using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MainData : MonoBehaviour
{
    string fileCharacterPath, fileReadyDeckPath, fileBurnedDeckPath, fileExhaustedDeckPath; 
    string jsonCharacterString, jsonDecksString;
    static public List<CharactersAndCards> dataActiveCharacters;
   

    private void Awake()
    {

        CharacterList characters = new CharacterList(); // LISTA QUE GUARDA LA LISTA DE PERSONAJES QUE SALEN EN UNA EXPEDICIÓN. POR EL MOMENTO SÓLO ESTÁ UNO IMPLEMENTADO
        dataActiveCharacters = new List<CharactersAndCards>(); // LISTA QUE GUARDA OBJETOS DE LA CLASE QUE PERMITE ALMACENAR LOS DATOS DE CADA PERSONAJE Y SUS MAZOS

        fileCharacterPath = Application.dataPath + "/Resources/JSON/StatsCharacters.json"; // DIRECCION DEL ARCHIVO JSON QUE GUARDA LAS ESTADISTICAS DE LOS PERSONAJES
        jsonCharacterString = File.ReadAllText(fileCharacterPath); //LECTURA DEL FICHERO JSON DE PERSONAJES
        characters = JsonUtility.FromJson<CharacterList>(jsonCharacterString);

        //Debug.Log(characters.characters.Count);

        foreach (Character character in characters.characters) //LOOP QUE RECORRE LA LISTA DE PERSONAJES PARA ALMACENAR LOS DATOS DE LOS PERSONAJES Y SUS MAZOS
        {
            CharactersAndCards tempCharacterAndCards = new CharactersAndCards();
            CardList tempReadyDeck = new CardList();
            CardList tempBurnedDeck = new CardList();
            CardList tempExhaustedDeck = new CardList();

            //LECTURA DEL MAZO DE CARTAS PREPARADAS Y ACTIVAS PARA EL ENCUENTRO
            fileReadyDeckPath = ReturnFilePath(character, "ready");
            jsonDecksString = File.ReadAllText(fileReadyDeckPath);
            tempReadyDeck = JsonUtility.FromJson<CardList>(jsonDecksString);

            //LECTURA DEL MAZO DE CARTAS QUE SE ENCUENTRAN AGOTADAS PORQUE YA HAN SIDO USADAS EN OTRO ENCUENTRO Y QUE SE PUEDEN RECUPERAR DURANTE EL ENCUENTRO ACTUAL POR LA ACCIÓN DE OTRAS CARTAS
            fileBurnedDeckPath = ReturnFilePath(character, "burned"); 
            jsonDecksString = File.ReadAllText(fileBurnedDeckPath);
            tempBurnedDeck = JsonUtility.FromJson<CardList>(jsonDecksString);

            // LECTURA DEL MAZO DE CARTAS QUE SE ENCUENTRAN AGOTADAS Y QUE SOLO SE PUEDEN RECUPERAR ENTRE PANTALLAS DE BUSQUEDA
            fileExhaustedDeckPath = ReturnFilePath(character, "exhausted"); 
            jsonDecksString = File.ReadAllText(fileExhaustedDeckPath);
            tempExhaustedDeck = JsonUtility.FromJson<CardList>(jsonDecksString);

            tempCharacterAndCards.character = character;
            tempCharacterAndCards.readyDeck = tempReadyDeck;
            tempCharacterAndCards.burnedDeck = tempBurnedDeck;
            tempCharacterAndCards.exhaustedDeck = tempExhaustedDeck;

            dataActiveCharacters.Add(tempCharacterAndCards);
        }

        //Debug.Log(dataActiveCharacters[0].character.name);
        //Debug.Log(dataActiveCharacters[0].character.hand);
        //Debug.Log(dataActivePlayers[0].readyDeck.cards.Count);
        //Debug.Log(dataActivePlayers[0].burnedDeck.cards.Count);
        //Debug.Log(dataActivePlayers[0].exhaustedDeck.cards.Count);
    }

    private string ReturnFilePath(Character character, string deckType)
    {
        string filePath;

        switch (deckType)
        {
            case "ready":
                filePath = Application.dataPath + "/Resources/JSON/Decks/" + character.name + "/ReadyDeck.json";
                break;

            case "burned":
                filePath = Application.dataPath + "/Resources/JSON/Decks/" + character.name + "/BurnedDeck.json";
                break;

            case "exhausted":
                filePath = Application.dataPath + "/Resources/JSON/Decks/" + character.name + "/ExhaustedDeck.json";
                break;

            default:
                filePath = "";
                break;
        }

        return filePath;
    }
}


[System.Serializable]
public class Character
{
    public string name;
    public int energy;
    public int health;
    public int hand;
    public float aimBasePercentage;
    public int aimBaseRabbet;
    public float defenseBasePercentage;
    public int defenseBaseRabbet;
    public float harvestBasePercentage;
    public int harvestBaseRabbet;
    public int supportingBaseRabbet;
    public int consumptionFoodPerDay;
    public int consumptionWaterPerDay;
    public string specialHabilityName;
    public string specialHabilityDescription;
}

[System.Serializable]
public class CharacterList
{
    public List<Character> characters;
}

[System.Serializable]
public class Card{
    public string ID;
    public string cardName;
    public string cardDescription;
    public int level;
    public string levelMark;
    public string frameworkType;
    public string imageArt;
    public int copies;
}

[System.Serializable]
public class DeckList
{
    public List<CardList> decks;
}

[System.Serializable]
public class CardList
{
    public List<Card> cards;
}

[System.Serializable]
public class CharactersAndCards
{
    public Character character;
    public CardList readyDeck;
    public CardList burnedDeck;
    public CardList exhaustedDeck;
}