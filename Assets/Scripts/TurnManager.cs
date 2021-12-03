using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
    public enum TurnState
    {
        Comeco,
        Escolher,
        Comparar,
        Empate,
        Vitoria,
        Derrota
    }
    public bool gameStart = true;
    public bool gameReady = false;
    public bool isPlayerTurn = true;
    public int turnState = 0;

    GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<GameManager>();
    }

    public void DecidirPrimeiro()
    {
        //isPlayerTurn = Mathf.Round(Random.value) > 0;
        isPlayerTurn = true;
    }

    private void PassTurn()
    {
        isPlayerTurn = !isPlayerTurn;

    }

    void Comparar()
    {

    }

    public void SetTurnState(TurnState state)
    {
        turnState = (int)state;
        print(turnState);
    }

    public void SetGameReady(bool v)
    {
        gameReady = v;
        print(gameReady);
    }
}
