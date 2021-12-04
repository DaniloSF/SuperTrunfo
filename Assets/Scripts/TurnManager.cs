using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>
/// Este � o script que cont�m as fun��es referentes a turnos dentro do jogo, requisitados no GameManager
/// </summary>
public class TurnManager : MonoBehaviour
{
    public enum TurnState
    {
        Comeco,
        Escolher,
        Comparar,
        Empate,
        Vitoria,
        Derrota,
        Fim
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

    protected void PassTurn()
    {
        isPlayerTurn = !isPlayerTurn;

    }

    void Comparar()
    {

    }

    public void SetTurnState(TurnState state)
    {
        turnState = (int)state;
        //print("TurnState: " + turnState);
    }

    public void SetGameReady(bool v)
    {
        gameReady = v;
        print(gameReady);
    }
}
