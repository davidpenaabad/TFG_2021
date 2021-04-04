using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using UnityEngine.AddressableAssets;

public class DrawCards : MonoBehaviour
{
    public Canvas mainCanvas;
    //public GameObject card;
    public GameObject playerZone;
    public GameObject enemyZone;
    public GameObject deckPlayerZone;
    private Sprite imageForArt;
    static public string triggerPlayer = "Harry";
    public CharactersAndCards activeCharacter;
    private List<GameObject> readyDeck;

    //private string relativePath = "Assets/Resources/Images/Cards/";
    //private string extension = ".png";

    void Start()
    {
        //Debug.Log(MainData.dataActiveCharacters[0].character.name);
        activeCharacter = new CharactersAndCards();
        readyDeck = new List<GameObject>();

        foreach (CharactersAndCards tempActiveCharacter in MainData.dataActiveCharacters)
        {
            if (tempActiveCharacter.character.name == triggerPlayer)
            {
                activeCharacter = tempActiveCharacter;
                break;
            }
        }

        //Debug.Log(activeCharacter.character.hand);
        //Debug.Log(activeCharacter.exhaustedDeck.cards.Count);


        activeCharacter.readyDeck.cards.Suffle(50); //FUNCIÓN PARA BARAJAR EL MAZO

        for (int i = 0; i < activeCharacter.readyDeck.cards.Count; i++) //CARGAR EL MAZO DE DE CARTAS DEL JUGADOR EN LA ZONA DE MAZO DE CARTAS ACTIVAS
        {
            GameObject card = Resources.Load("Prefabs/Card", typeof(GameObject)) as GameObject;
            card.transform.GetChild(0).GetComponent<Image>().sprite = ReturnArtworkCard(activeCharacter.readyDeck.cards[i]); //CARGAR LA IMAGEN DE LA CARTA
            //GetChild(1) CORRESPONDE A LA CAPA BACKGROUND QUE NO VARIA
            card.transform.GetChild(2).GetComponent<Image>().sprite = ReturnFrameworkType(activeCharacter.readyDeck.cards[i]); //CARGAR EL TIPO DE MARCO DE CARTA
            card.transform.GetChild(3).GetComponent<Image>().sprite = ReturnLevelMark(activeCharacter.readyDeck.cards[i]); //CARGAR EL INDICADOR DEL NIVEL DE LA CARTA
            card.transform.GetChild(4).GetComponent<Text>().text = activeCharacter.readyDeck.cards[i].cardName != null ? activeCharacter.readyDeck.cards[i].cardName : ""; // CARGAR EL NOMBRE DE LA CARTA
            card.transform.GetChild(5).GetComponent<Text>().text = activeCharacter.readyDeck.cards[i].cardDescription != null ? activeCharacter.readyDeck.cards[i].cardDescription : ""; // CARGAR LA DESCRIPCION DE LA CARTA
            card.transform.GetComponent<GameObjectCardData>().ID = activeCharacter.readyDeck.cards[i].ID;
            card.transform.GetComponent<GameObjectCardData>().cardName = activeCharacter.readyDeck.cards[i].cardName;
            card.transform.GetComponent<GameObjectCardData>().cardDescription = activeCharacter.readyDeck.cards[i].cardDescription;
            card.transform.GetComponent<GameObjectCardData>().level = activeCharacter.readyDeck.cards[i].level;
            card.transform.GetComponent<GameObjectCardData>().copies = activeCharacter.readyDeck.cards[i].copies;

            card = Instantiate(card, Vector3.zero, Quaternion.identity);
            //GameObject playerCard = Instantiate(card, Vector3.zero, Quaternion.identity);
            card.transform.SetParent(deckPlayerZone.transform, false);
            readyDeck.Add(card);
        }

        //Debug.Log(mainCanvas.transform.GetComponent<RectTransform>().localPosition.x);

        StartCoroutine(DealCard());

        //readyDeck[0].transform.GetComponent<Transform>().Rotate(new Vector3(0, 0, 45);
    }

    IEnumerator DealCard()
    {
        for (int i = 0; i < activeCharacter.character.hand; i++)
        {
            readyDeck[i].transform.SetParent(playerZone.transform, true);
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(FlipCard(readyDeck[i]));
        }
    }

  IEnumerator FlipCard(GameObject card) {
        for(int i = 0; i < 180; i++)
        {
            yield return new WaitForEndOfFrame();

            card.transform.GetComponent<Transform>().Rotate(new Vector3(0, 1, 0));

            if (i == 90 || i == -90)
            {
                card.transform.GetChild(6).gameObject.SetActive(false);
            }
        }
    }

    private Sprite ReturnArtworkCard(Card card) //FUNCION PARA RETORNAR LA IMAGEN DE LA CARTA DE ACUERDO A LOS DATOS DEL FICHERO JSON
    {
        Sprite art;


        if (card.imageArt != null && card.imageArt != "") //&& AssetDatabase.FindAssets(card.imageArt) != null)
        {
            //art = AssetDatabase.LoadAssetAtPath(relativePath + card.imageArt + extension, typeof(Sprite)) as Sprite;
            art = Resources.Load("Images/Cards/" + card.imageArt, typeof(Sprite)) as Sprite;
        }
        else
        {
            //art = AssetDatabase.LoadAssetAtPath(relativePath + "Default" + extension, typeof(Sprite)) as Sprite;
            art = Resources.Load("Images/Cards/" + "Default", typeof(Sprite)) as Sprite;
        }

        return art;
    }

    private Sprite ReturnFrameworkType(Card card) //FUNCION PARA RETORNAR LA IMAGEN DEL MARCO DE LA CARTA DE ACUERDO A LOS DATOS DEL FICHERO JSON
    {
        Sprite framework;

        if (card.frameworkType != null && card.frameworkType != "")// && AssetDatabase.FindAssets(card.frameworkType) != null)
        {
            //framework = AssetDatabase.LoadAssetAtPath(relativePath + card.frameworkType + extension, typeof(Sprite)) as Sprite;
            framework = Resources.Load("Images/Cards/" + card.frameworkType, typeof(Sprite)) as Sprite;
        }
        else
        {
            //framework = AssetDatabase.LoadAssetAtPath(relativePath + "DefaultFramework" + extension, typeof(Sprite)) as Sprite;
            framework = Resources.Load("Images/Cards/" + "DefaultFramework", typeof(Sprite)) as Sprite;
        }

        return framework;
    }

    private Sprite ReturnLevelMark(Card card) //FUNCION PARA RETORNAR LA IMAGEN QUE INDICA EL NIVEL DE LA CARTA DE ACUERDO A LOS DATOS DEL FICHERO JSON
    {
        Sprite levelMark;

        if (card.levelMark != null && card.levelMark != "")// && AssetDatabase.FindAssets(card.levelMark) != null)
        {
            //levelMark = AssetDatabase.LoadAssetAtPath(relativePath + card.levelMark + extension, typeof(Sprite)) as Sprite;
            levelMark = Resources.Load("Images/Cards/" + card.levelMark, typeof(Sprite)) as Sprite;
        }
        else
        {
            //levelMark = AssetDatabase.LoadAssetAtPath(relativePath + "Level1" + extension, typeof(Sprite)) as Sprite;
            levelMark = Resources.Load("Images/Cards/" + "Level1", typeof(Sprite)) as Sprite;
        }

        return levelMark;
    }
}


