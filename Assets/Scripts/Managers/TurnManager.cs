using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    public enum PlayerTurn { playerOne, playerTwo };
    public PlayerTurn playerTurn;

    public enum TurnPhase { begin, draw, main, movement, combat, end };
    public TurnPhase turnPhase;

    // Start is called before the first frame update
    void Start()
    {
        playerTurn = PlayerTurn.playerOne;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextPhase ()
    {
        switch (turnPhase)
        {
            case TurnPhase.begin: turnPhase = TurnPhase.draw;
                break;
            case TurnPhase.draw: turnPhase = TurnPhase.main;
                break;
            case TurnPhase.main: turnPhase = TurnPhase.end;
                break;
            case TurnPhase.movement: turnPhase = TurnPhase.combat;
                break;
            case TurnPhase.combat: turnPhase = TurnPhase.end;
                break;
            case TurnPhase.end: ChangePlayerTurn();
                break;
            default:
                break;
        }
    }
    public void ChangePlayerTurn()
    {
        if (playerTurn == PlayerTurn.playerOne)
        {
            playerTurn = PlayerTurn.playerTwo;
            turnPhase = TurnPhase.begin;
        }
        else
        {
            playerTurn = PlayerTurn.playerOne;
            turnPhase = TurnPhase.begin;
        }
    }
}
