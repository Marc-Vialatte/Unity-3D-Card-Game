﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card", menuName ="Card")]
public class Card : ScriptableObject
{
    public int cardId;
    //public Sprite cardImage;
    public Image cardImage;
    public string cardName;
    public string cardDescription;
    public int manaCost;
    public float maxRange;
    public int damageOrHeal;
    public int movePoint;

    public enum TypesOfCard { AttaqueCaC, AttaqueDist, Soins, Déplacement };
    public TypesOfCard typeOfCard;


    public Card ()
    {

    }
    public Card (int Id, Image image, string name, string description, int manaAlteration, int max, int lifeAlteration, int moveAlteration)
    {
        cardId = Id;
        cardImage = image;
        cardName = name;
        cardDescription = description;
        manaCost = manaAlteration;
        maxRange = max;
        damageOrHeal = lifeAlteration;
        movePoint = moveAlteration;
    }
}
