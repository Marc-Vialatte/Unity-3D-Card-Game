using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public TurnManager turnManager;
    public CardsManager cardsManager;
    public ViewManager viewManager;

    public Player PlayerOne;
    public Player PlayerTwo;

    // Start is called before the first frame update
    void Start()
    {
        int x = 0;
        List<Card> deck = new List<Card>();

        for (int i = 0; i < 40; i++)
        {
            x = Random.Range(0, 3);
            deck.Add(cardsManager.playableCards[x]);
        }

        PlayerOne.deck = deck;
        PlayerTwo.deck = deck;
        
        PlayerOne.DeckShuffle();
        PlayerTwo.DeckShuffle();
        
        viewManager.SetValues(PlayerOne.name, PlayerTwo.name, PlayerOne.health, PlayerTwo.health, PlayerOne.mana, PlayerTwo.mana);

        for (int i = 0; i < 6; i++)
        {
            PlayerOne.DrawCard();
            PlayerTwo.DrawCard();
        }

        foreach(Card card in PlayerOne.hand)
        {
            viewManager.AddCardInHand(card.cardImage);
        }
        turnManager.ChangePlayerTurn();

        foreach (Card card in PlayerTwo.hand)
        {
            viewManager.AddCardInHand(card.cardImage);
        }
        turnManager.ChangePlayerTurn();
        NextPhase();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextPhase()
    {
        turnManager.NextPhase();
        switch (turnManager.turnPhase)
        {
            case TurnManager.TurnPhase.begin:
                viewManager.SetPhaseName("Begin");
                if (turnManager.playerTurn == TurnManager.PlayerTurn.playerOne)
                {
                    PlayerOne.ManaRegen();
                }
                else
                {
                    PlayerTwo.ManaRegen();
                }

                viewManager.SetPlayerHealth(PlayerOne.name, PlayerOne.health);
                viewManager.SetPlayerHealth(PlayerTwo.name, PlayerTwo.health);

                viewManager.SetPlayerMana(PlayerOne.name, PlayerOne.mana);
                viewManager.SetPlayerMana(PlayerTwo.name, PlayerTwo.mana);

                viewManager.SetRound();
                viewManager.SetPlayerHand();
                NextPhase();
                break;
            case TurnManager.TurnPhase.draw:
                viewManager.SetPhaseName("Draw");
                DrawCard(turnManager.playerTurn == TurnManager.PlayerTurn.playerOne ? PlayerOne : PlayerTwo);
                NextPhase();
                break;
            case TurnManager.TurnPhase.main:
                viewManager.SetPhaseName("Main");
                break;
            case TurnManager.TurnPhase.movement:
                viewManager.SetPhaseName("Movement");
                break;
            case TurnManager.TurnPhase.combat:
                viewManager.SetPhaseName("Combat");
                break;
            case TurnManager.TurnPhase.end:
                viewManager.SetPhaseName("End");
                break;
            default:
                break;
        }
    }

    public void UseCard(string cardName, GameObject uiCard)
    {
        Card card = cardsManager.playableCards.Find(x => x.name == cardName);

        switch (turnManager.playerTurn)
        {
            case TurnManager.PlayerTurn.playerOne when PlayerOne.mana >= card.manaCost:
                PlayerOne.mana -= card.manaCost;
                viewManager.SetPlayerMana(PlayerOne.name, PlayerOne.mana);
                CardEffect(PlayerOne, PlayerTwo, card);
                PlayerOne.UseCard(card);
                viewManager.RemoveCardFromHand(card.cardImage);
                Destroy(uiCard);
                break;
            case TurnManager.PlayerTurn.playerTwo when PlayerTwo.mana >= card.manaCost:
                PlayerTwo.mana -= card.manaCost;
                viewManager.SetPlayerMana(PlayerTwo.name, PlayerTwo.mana);
                CardEffect(PlayerTwo, PlayerOne, card);
                PlayerTwo.UseCard(card);
                viewManager.RemoveCardFromHand(card.cardImage);
                Destroy(uiCard);
                break;
            default:
                break;
        }
    }

    public void CardEffect(Player playerInPlay, Player playerInWait, Card card)
    {
        switch (card.typeOfCard)
        {
            case Card.TypesOfCard.Attaque:
                playerInWait.health += card.damageOrHeal;
                viewManager.SetPlayerHealth(playerInWait.name, playerInWait.health);
                break;
            case Card.TypesOfCard.Soins:
                playerInPlay.health += card.damageOrHeal;
                viewManager.SetPlayerHealth(playerInPlay.name, playerInPlay.health);
                break;
            case Card.TypesOfCard.Déplacement:
                break;
            default:
                break;
        }
    }

    public void DrawCard(Player player)
    {
        Image card = player.DrawCard();
        viewManager.AddCardInHand(card);
    }
}