using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ViewManager : MonoBehaviour
{
    public TMP_Text _playerOneName;
    public TMP_Text _playerTwoName;

    public GameObject _playerOneHealthBar;
    public GameObject _playerTwoHealthBar;
    public TMP_Text _playerOneHealth;
    public TMP_Text _playerTwoHealth;

    public TMP_Text _playerOneMana;
    public TMP_Text _playerTwoMana;

    public GameObject _playerOneRoundIndicator;
    public GameObject _playerTwoRoundIndicator;

    public TMP_Text _timer;

    public TMP_Text _Phase;

    public GameObject _playerOneHand;
    private List<Image> _playerOneHandCards = new List<Image>();
    public GameObject _playerTwoHand;
    private List<Image> _playerTwoHandCards = new List<Image>();




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValues(string playerOneName, string playerTwoName, int playerOneHealth, int playerTwoHealth, int playerOneMana, int playerTwoMana)
    {
        _playerOneName.text = playerOneName;
        _playerTwoName.text = playerTwoName;

        SetPlayerHealth(playerOneName, playerOneHealth);
        SetPlayerHealth(playerTwoName, playerTwoHealth);

        SetPlayerMana(playerOneName, playerOneMana);
        SetPlayerMana(playerTwoName, playerTwoMana);

        SetRound();
        SetPlayerHand();
    }

    public void SetPlayerHealth(string playerName, int playerHealth)
    {
        if (playerName == _playerOneName.text)
        {
            _playerOneHealth.text = "" + playerHealth;
            int lifeModulo100 = playerHealth % 100;
            if (playerHealth == 100)
            {
                lifeModulo100 = 100;
            }
            _playerOneHealthBar.transform.localScale = new Vector3(lifeModulo100 / 100f, 1, 1);
        }
        else
        {
            _playerTwoHealth.text = "" + playerHealth;
            int lifeModulo100 = playerHealth % 100;
            if (playerHealth == 100)
            {
                lifeModulo100 = 100;
            }
            _playerTwoHealthBar.transform.localScale = new Vector3(- lifeModulo100 / 100f, 1, 1);
        }
    }

    public void SetPlayerMana(string playerName, int playerMana)
    {
        if (playerName == _playerOneName.text)
            _playerOneMana.text = "" + playerMana;

        else
            _playerTwoMana.text = "" + playerMana;
    }

    public void SetRound()
    {
        if (TurnManager.Instance.playerTurn == TurnManager.PlayerTurn.playerOne)
        {
            _playerOneRoundIndicator.SetActive(true);
            _playerTwoRoundIndicator.SetActive(false);
        }
        else
        {
            _playerOneRoundIndicator.SetActive(false);
            _playerTwoRoundIndicator.SetActive(true);
        }
    }

    public void SetPhaseName(string phaseName)
    {
        _Phase.text = phaseName;
    }

    public void SetPlayerHand()
    {
        if (TurnManager.Instance.playerTurn == TurnManager.PlayerTurn.playerOne)
        {
            _playerOneHand.SetActive(true);
            _playerTwoHand.SetActive(false);
        }
        else
        {
            _playerOneHand.SetActive(false);
            _playerTwoHand.SetActive(true);
        }
    }

    public void AddCardInHand(Image cardToInstantiate)
    {
        Image card = Instantiate(cardToInstantiate, Vector3.zero, Quaternion.identity);
        card.name = cardToInstantiate.name;

        if (TurnManager.Instance.playerTurn == TurnManager.PlayerTurn.playerOne)
        {
            _playerOneHandCards.Add(card);
            card.transform.SetParent(_playerOneHand.transform);
            card.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
        else
        {
            _playerTwoHandCards.Add(card);
            card.transform.SetParent(_playerTwoHand.transform);
            card.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            card.gameObject.SetActive(true);
        }
    }
    public void RemoveCardFromHand( Image cardToRemove)
    {
        bool isRemoved = false;
        int cardToRemoveIndex = -1;

        if (TurnManager.Instance.playerTurn == TurnManager.PlayerTurn.playerOne)
        {
            for (int i = 0; i < _playerOneHandCards.Count; i++)
            {
                if (_playerOneHandCards[i] == cardToRemove && isRemoved == false)
                {
                    cardToRemoveIndex = i;
                    isRemoved = true;
                }
            }

            if (cardToRemoveIndex >= 0)
            {
                _playerOneHandCards.RemoveAt(cardToRemoveIndex);
            }
        }
        else
        {
            for (int i = 0; i < _playerTwoHandCards.Count; i++)
            {
                if (_playerTwoHandCards[i] == cardToRemove && isRemoved == false)
                {
                    cardToRemoveIndex = i;
                    isRemoved = true;
                }
            }

            if (cardToRemoveIndex >= 0)
            {
                _playerTwoHandCards.RemoveAt(cardToRemoveIndex);
            }
        }
    }
}
