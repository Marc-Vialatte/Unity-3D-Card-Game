using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int playerId;
    public string playerName;
    public List<Card> hand;
    public List<Card> deck;
    public List<Card> graveyard;
    public int health = 100;
    public int mana = 0;
    public float move = 3;
    private int manaRegen = 5;
    private int moveMax = 5;

    public GameObject championCard;
    public GameObject champion;

    private bool isMoving = false;

    public void LifeLooseOrRegen(int lifeAlteration)
    {
        health += lifeAlteration;
    }

    public void MoveUseOrRegen(float moveAlteration)
    {
        move += moveAlteration;
    }

    public void ManaUseOrRegen(int manaAlteration)
    {
        mana += manaAlteration;
    }

    public void DeckShuffle()
    {
        int deckSize = deck.Count;
        Card container = new Card();

        for (int i = 0; i < deckSize; i++)
        {
            container = deck[i];
            int randomIndex = Random.Range(i, deckSize);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = container;
        }
    }

    public Image DrawCard()
    {
        Image card = deck[0].cardImage;
        hand.Add(deck[0]);
        deck.Remove(deck[0]);

        return card;
    }

    public void UseCard(Card card)
    {
        bool isRemoved = false;
        int removeCardIndex = -1;

        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i] != card || isRemoved != false) continue;
            removeCardIndex = i;
            graveyard.Add(hand[i]);
            isRemoved = true;
        }

        if (removeCardIndex >= 0)
        {
            hand.RemoveAt(removeCardIndex);
        }
    }

    public void ManaRegen()
    {
        mana += manaRegen;
    }
}
