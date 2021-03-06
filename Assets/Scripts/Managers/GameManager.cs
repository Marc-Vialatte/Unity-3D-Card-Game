﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public TurnManager turnManager;
    public CardsManager cardsManager;
    public ViewManager viewManager;
    public Animator animatorChampion1;
    public Animator animatorChampion2;

    public Player PlayerOne;
    public Player PlayerTwo;

    public GameObject championOneCard;
    public GameObject championTwoCard;

    public LineRenderer distanceChampionOneRenderer;
    public LineRenderer distanceChampionTwoRenderer;

    public GameObject range;

    private bool championOnCards = false;

    void Start()
    {
        championOneCard.GetComponent<BoxCollider>().enabled = false;
        championTwoCard.GetComponent<BoxCollider>().enabled = false;

        distanceChampionOneRenderer.gameObject.SetActive(false);
        distanceChampionTwoRenderer.gameObject.SetActive(false);

        range.SetActive(false);

        int x = 0;
        List<Card> deck = new List<Card>();

        for (int i = 0; i < 40; i++)
        {
            x = Random.Range(0, cardsManager.playableCards.Count);
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

    private void Update()
    {
        switch (turnManager.playerTurn)
        {
            case TurnManager.PlayerTurn.playerOne:
                float distancePlayerOne = Vector3.Distance(PlayerOne.championCard.transform.position, PlayerOne.champion.transform.position);
                if (distancePlayerOne <= PlayerOne.move)
                {
                    distanceChampionOneRenderer.SetPosition(0, PlayerOne.champion.transform.position);
                    distanceChampionOneRenderer.SetPosition(1, championOneCard.transform.position);
                    distanceChampionOneRenderer.startColor = Color.green;
                    distanceChampionOneRenderer.endColor = Color.green;
                }
                else if (distancePlayerOne > PlayerOne.move)
                {
                    distanceChampionOneRenderer.startColor = Color.red;
                    distanceChampionOneRenderer.endColor = Color.red;
                }

                range.transform.position = new Vector3(championOneCard.transform.position.x, range.transform.position.y, championOneCard.transform.position.z);

                if (animatorChampion1.GetBool("IsMoving"))
                    PlayerOne.champion.transform.position = Vector3.MoveTowards(PlayerOne.champion.transform.position, championOneCard.transform.position, 1f * Time.deltaTime);

                if (Vector3.Distance(PlayerOne.champion.transform.position, championOneCard.transform.position) < 0.05f)
                {
                    championOnCards = true;
                }
                else
                    championOnCards = false;
                break;

            case TurnManager.PlayerTurn.playerTwo:
                float distancePlayerTwo = Vector3.Distance(PlayerTwo.championCard.transform.position, PlayerTwo.champion.transform.position);
                if (distancePlayerTwo <= PlayerTwo.move)
                {
                    distanceChampionTwoRenderer.SetPosition(0, PlayerTwo.champion.transform.position);
                    distanceChampionTwoRenderer.SetPosition(1, championTwoCard.transform.position);
                    distanceChampionTwoRenderer.startColor = Color.green;
                    distanceChampionTwoRenderer.endColor = Color.green;
                }
                else if (distancePlayerTwo > PlayerTwo.move)
                {
                    distanceChampionTwoRenderer.startColor = Color.red;
                    distanceChampionTwoRenderer.endColor = Color.red;
                }

                range.transform.position = new Vector3(championTwoCard.transform.position.x, range.transform.position.y, championTwoCard.transform.position.z);

                if (animatorChampion2.GetBool("IsMoving"))
                    PlayerTwo.champion.transform.position = Vector3.MoveTowards(PlayerTwo.champion.transform.position, championTwoCard.transform.position, 1f * Time.deltaTime);

                if (Vector3.Distance(PlayerTwo.champion.transform.position, championTwoCard.transform.position) < 0.05f)
                {
                    championOnCards = true;
                }
                else
                    championOnCards = false;
                break;
        }
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
                    PlayerOne.MoveReset();
                }
                else
                {
                    PlayerTwo.ManaRegen();
                    PlayerTwo.MoveReset();
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
                    distanceChampionOneRenderer.gameObject.SetActive(true);
                }
                else
                {
                    championTwoCard.GetComponent<BoxCollider>().enabled = true;
                    distanceChampionTwoRenderer.gameObject.SetActive(true);
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
                distanceChampionOneRenderer.gameObject.SetActive(false);
                distanceChampionTwoRenderer.gameObject.SetActive(false);
                break;
        }
    }

    public void UseCard(string cardName, GameObject uiCard)
    {
        Card card = cardsManager.playableCards.Find(x => x.name == cardName);
        float range = Vector3.Distance(PlayerOne.champion.transform.position, PlayerTwo.champion.transform.position);

        switch (turnManager.playerTurn)
        {
            case TurnManager.PlayerTurn.playerOne when PlayerOne.mana >= card.manaCost && range <= card.maxRange:
                PlayerOne.mana -= card.manaCost;
                viewManager.SetPlayerMana(PlayerOne.playerName, PlayerOne.mana);
                StartCoroutine(CardEffect(PlayerOne, PlayerTwo, card));
                PlayerOne.UseCard(card);
                viewManager.RemoveCardFromHand(card.cardImage);
                Destroy(uiCard);
                break;
            case TurnManager.PlayerTurn.playerTwo when PlayerTwo.mana >= card.manaCost && range <= card.maxRange:
                PlayerTwo.mana -= card.manaCost;
                viewManager.SetPlayerMana(PlayerTwo.playerName, PlayerTwo.mana);
                StartCoroutine(CardEffect(PlayerTwo, PlayerOne, card));
                PlayerTwo.UseCard(card);
                viewManager.RemoveCardFromHand(card.cardImage);
                Destroy(uiCard);
                break;
        }
    }

    private IEnumerator CardEffect(Player playerInPlay, Player playerInWait, Card card)
    {
        switch (card.typeOfCard)
        {
            case Card.TypesOfCard.AttaqueCaC:
                print("ui");
                if(playerInPlay == PlayerOne)
                {
                    animatorChampion1.SetBool("IsAttack", true);
                }
                else
                {
                    animatorChampion2.SetBool("IsAttack", true);
                }
                playerInWait.LifeLooseOrRegen(card.damageOrHeal);
                viewManager.SetPlayerHealth(playerInWait.playerName, playerInWait.health);
                break;
            case Card.TypesOfCard.AttaqueDist:
                if (playerInPlay == PlayerOne)
                {
                    animatorChampion1.SetBool("IsAttackRange", true);
                }
                else
                {
                    animatorChampion2.SetBool("IsAttackRange", true);
                }
                playerInWait.LifeLooseOrRegen(card.damageOrHeal);
                viewManager.SetPlayerHealth(playerInWait.playerName, playerInWait.health);
                break;
            case Card.TypesOfCard.Soins:
                playerInPlay.LifeLooseOrRegen(card.damageOrHeal);
                viewManager.SetPlayerHealth(playerInPlay.playerName, playerInPlay.health);
                break;
            case Card.TypesOfCard.Déplacement:
                playerInPlay.MoveUseOrRegen(card.movePoint);
                break;
        }
        yield return new WaitForSeconds(1.0f);
        animatorChampion1.SetBool("IsAttack", false);
        animatorChampion2.SetBool("IsAttack", false);
        animatorChampion1.SetBool("IsAttackRange", false);
        animatorChampion2.SetBool("IsAttackR", false);
    }

    public void DrawCard(Player player)
    {
        Image card = player.DrawCard();
        viewManager.AddCardInHand(card);
    }

    public void ShowRange(bool isShowed)
    {
        range.SetActive(isShowed);
    }

    public void MoveChampion(Vector3 savedPosition)
    {
        Vector3 targetCardPosition;
        Vector3 targetChampionPosition;

        float distancePlayerOne = Vector3.Distance(PlayerOne.championCard.transform.position, PlayerOne.champion.transform.position);
        float distancePlayerTwo = Vector3.Distance(PlayerTwo.championCard.transform.position, PlayerTwo.champion.transform.position);

        switch (turnManager.playerTurn)
        {
            case TurnManager.PlayerTurn.playerOne when distancePlayerOne <= PlayerOne.move:
                targetCardPosition = new Vector3(PlayerOne.championCard.transform.position.x, 0, PlayerOne.championCard.transform.position.z);
                targetChampionPosition = new Vector3(PlayerTwo.champion.transform.position.x, 0, PlayerTwo.champion.transform.position.z);

                StartCoroutine(MoveChampionToCardPosition(targetCardPosition, targetChampionPosition, PlayerOne.champion.transform));
                PlayerOne.MoveUseOrRegen(-distancePlayerOne);
                break;
            case TurnManager.PlayerTurn.playerTwo when distancePlayerTwo <= PlayerTwo.move:
                targetCardPosition = new Vector3(PlayerTwo.championCard.transform.position.x, 0, PlayerTwo.championCard.transform.position.z);
                targetChampionPosition = new Vector3(PlayerOne.champion.transform.position.x, 0, PlayerOne.champion.transform.position.z);

                StartCoroutine(MoveChampionToCardPosition(targetCardPosition, targetChampionPosition, PlayerTwo.champion.transform));
                PlayerTwo.MoveUseOrRegen(-distancePlayerTwo);
                break;
            case TurnManager.PlayerTurn.playerOne when distancePlayerOne > PlayerOne.move:
                championOneCard.transform.position = savedPosition;
                break;
            case TurnManager.PlayerTurn.playerTwo when distancePlayerTwo > PlayerTwo.move:
                championOneCard.transform.position = savedPosition;
                break;
            default:
                break;
        }
    }

    private IEnumerator MoveChampionToCardPosition(Vector3 targetCardPosition, Vector3 targetChampionPosition, Transform champion)
    {
        yield return new WaitForSeconds(1.0f);

        champion.rotation = Quaternion.LookRotation(targetCardPosition - champion.position);

        yield return new WaitForSeconds(0.5f);

        switch (turnManager.playerTurn)
        {
            case TurnManager.PlayerTurn.playerOne:
                animatorChampion1.SetBool("IsMoving", true);
                break;
            case TurnManager.PlayerTurn.playerTwo:
                animatorChampion2.SetBool("IsMoving", true);
                break;
            default:
                break;
        }

        yield return new WaitUntil(() => championOnCards == true);

        animatorChampion1.SetBool("IsMoving", false);
        animatorChampion2.SetBool("IsMoving", false);

        yield return new WaitForSeconds(0.5f);

        champion.rotation = Quaternion.LookRotation(targetChampionPosition - champion.position);
        
        yield return null;
    }
}