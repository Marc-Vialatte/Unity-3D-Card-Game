using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ThisCard : MonoBehaviour
{
    public List<Card> thisCard = new List<Card>();
    public int thisId;

    public int id;
    public string cardName;
    public int cost;
    public int damages;
    public int movement;
    public int rangeMin;
    public int rangeMax;
    public string cardDescription;

    public Image card;

    // Start is called before the first frame update
    void Start()
    {
        thisCard[0] = CardDataBase.cardList[thisId];
    }

    // Update is called once per frame
    void Update()
    {
        id = thisCard[0].id;
        cardName = thisCard[0].cardName;
        cost = thisCard[0].cost;
        damages = thisCard[0].damages;
        movement = thisCard[0].movement;
        rangeMin = thisCard[0].rangeMin;
        rangeMax = thisCard[0].rangeMax;
        cardDescription = thisCard[0].cardDescription;
        card.sprite = thisCard[0].cardImage;
    }
}
