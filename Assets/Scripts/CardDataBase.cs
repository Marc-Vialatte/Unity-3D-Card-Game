using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();
    public List<Sprite> cardsSprites;

    void Awake()
    {
        //cardList.Add(new Card(0, "None", 0, 0, "None"));
        cardList.Add(new Card(1, "Attaque Légère CàC", 1, -10, 0, 1, 1, "Attaque légère de corps à corps, inflige 10 dégats", cardsSprites[1]));
        cardList.Add(new Card(2, "Attaque Lourde CàC", 3, -20, 0, 1, 1, "Attaque lourde de corps à corps, inflige 20 dégats", cardsSprites[2]));
        cardList.Add(new Card(3, "Attaque Légère Distance", 2, -10, 0, 2, 5, "Attaque légère à distance, inflige 10 dégats", cardsSprites[3]));
        cardList.Add(new Card(4, "Attaque Lourde Distance", 5, -20, 0, 2, 5, "Attaque lourde à distance, inflige 10 dégats", cardsSprites[4]));
    }
}
