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

    public GameObject championOneCard;
    public GameObject championTwoCard;

    // Start is called before the first frame update
    void Start()
    {
        championOneCard.GetComponent<BoxCollider>().enabled = false;
        championTwoCard.GetComponent<BoxCollider>().enabled = false;

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
        
        viewManager.SetValues(PlayerOne.playerName, PlayerTwo.playerName, PlayerOne.health, PlayerTwo.health, PlayerOne.mana, PlayerTwo.mana);

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

                viewManager.SetPlayerHealth(PlayerOne.playerName, PlayerOne.health);
                viewManager.SetPlayerHealth(PlayerTwo.playerName, PlayerTwo.health);

                viewManager.SetPlayerMana(PlayerOne.playerName, PlayerOne.mana);
                viewManager.SetPlayerMana(PlayerTwo.playerName, PlayerTwo.mana);

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
                if (turnManager.playerTurn == TurnManager.PlayerTurn.playerOne)
                {
                    championOneCard.GetComponent<BoxCollider>().enabled = true;
                }
                else
                {
                    championTwoCard.GetComponent<BoxCollider>().enabled = true;
                }

                break;
            case TurnManager.TurnPhase.movement:
                viewManager.SetPhaseName("Movement");
                break;
            case TurnManager.TurnPhase.combat:
                viewManager.SetPhaseName("Combat");
                break;
            case TurnManager.TurnPhase.end:
                if (PlayerOne.health <= 0)
                    viewManager.ShowWinnerPanel(PlayerTwo.playerName);
                else if (PlayerTwo.health <= 0)
                    viewManager.ShowWinnerPanel(PlayerOne.playerName);
                viewManager.SetPhaseName("End");

                championOneCard.GetComponent<BoxCollider>().enabled = false;
                championTwoCard.GetComponent<BoxCollider>().enabled = false;
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
                viewManager.SetPlayerMana(PlayerOne.playerName, PlayerOne.mana);
                CardEffect(PlayerOne, PlayerTwo, card);
                PlayerOne.UseCard(card);
                viewManager.RemoveCardFromHand(card.cardImage);
                Destroy(uiCard);
                break;
            case TurnManager.PlayerTurn.playerTwo when PlayerTwo.mana >= card.manaCost:
                PlayerTwo.mana -= card.manaCost;
                viewManager.SetPlayerMana(PlayerTwo.playerName, PlayerTwo.mana);
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
                viewManager.SetPlayerHealth(playerInWait.playerName, playerInWait.health);
                break;
            case Card.TypesOfCard.Soins:
                playerInPlay.health += card.damageOrHeal;
                viewManager.SetPlayerHealth(playerInPlay.playerName, playerInPlay.health);
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

    public void MoveChampion()
    {
        Vector3 targetPosition;
        Quaternion targetRotation;

        switch (turnManager.playerTurn)
        {
            case TurnManager.PlayerTurn.playerOne:
                targetPosition = new Vector3(PlayerOne.championCard.transform.position.x, 0, PlayerOne.championCard.transform.position.z); ;
                //targetRotation = new Quaternion(0, PlayerOne.championCard.transform.rotation.y, 0, 1);
                targetRotation = PlayerOne.championCard.transform.rotation;

                StartCoroutine(MoveChampionToCardPosition(targetPosition, targetRotation, PlayerOne.champion.transform));
                break;
            case TurnManager.PlayerTurn.playerTwo:
                targetPosition = new Vector3(PlayerTwo.championCard.transform.position.x, 0, PlayerTwo.championCard.transform.position.z); ;
                targetRotation = new Quaternion(0, PlayerTwo.championCard.transform.rotation.y, 0, 1);

                StartCoroutine(MoveChampionToCardPosition(targetPosition, targetRotation, PlayerTwo.champion.transform));
                break;
            default:
                break;
        }
    }

    private IEnumerator MoveChampionToCardPosition(Vector3 targetPosition, Quaternion targetRotation, Transform champion)
    {
        yield return new WaitForSeconds(3);

        champion.rotation = Quaternion.LookRotation(targetPosition - champion.position);

        yield return new WaitForSeconds(2);

        float step = 1.0f * Time.deltaTime;
        while (champion.position != targetPosition)
        {
            champion.position = Vector3.MoveTowards(champion.position, targetPosition, step);
        }
        yield return new WaitForSeconds(2);

        champion.rotation = targetRotation;

        yield return null;
    }

}