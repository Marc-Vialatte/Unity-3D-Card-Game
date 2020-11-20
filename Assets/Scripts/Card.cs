using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class Card
{
    public int id;
    public string cardName;
    public int cost;
    public int damages;
    public int movement;
    public int rangeMin;
    public int rangeMax;
    public string cardDescription;

    public Sprite cardImage;
    
    public Card() 
    {
        
    }

    public Card(Sprite CardImage)
    {
        cardImage = CardImage;
    }

    public Card(int Id, string CardName, int Cost, int Damages, int Movement, int RangeMin, int RangeMax, string CardDescription, Sprite CardImage)
    {
        id = Id;
        cardName = CardName;
        cost = Cost;
        damages = Damages;
        movement = Movement;
        rangeMin = RangeMin;
        rangeMax = RangeMax;
        cardDescription = CardDescription;

        cardImage = CardImage;
    }
}
