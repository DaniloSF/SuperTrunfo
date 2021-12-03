using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class GameManager : TurnManager
{

    public int totalDeCartas = 20;

    public GameObject cartaPrefab;
    public GameObject posicaoCartasJogador, posicaoCartasAdversario;
    public List<GameObject> Baralho = new List<GameObject>();
    public List<GameObject> CartasDoPlayer = new List<GameObject>();
    public List<GameObject> CartasDoAdversario = new List<GameObject>();
    
    public BoxCollider2D PlayerCartasColider;
    //public TurnManager turnManager;
    public GameObject Escolhas;
    public int statsEscolhido = -1;

    void Start()
    {
        
        PlayerCartasColider = posicaoCartasJogador.GetComponent<BoxCollider2D>();
        //turnManager = GetComponent<TurnManager>();
        GeraBaralho();
        foreach (var carta in Baralho)
        {   
            carta.GetComponent<CartaScript>().VirarCarta();
        }
        
    }

    public void GeraBaralho()
    {
        string nomeDaCarta;
        CartaSO cartaSO;

        for (int i = 0; i < totalDeCartas; i++)
        {
            Vector3 posicao = new Vector3(0, 0, 0);
            nomeDaCarta = "Carta " + (i + 1);
            GameObject carta = Instantiate(cartaPrefab, transform.position + new Vector3(0, 0, i * 0.1f), Quaternion.identity);
            cartaSO = Resources.Load<CartaSO>("CartasSO/" + nomeDaCarta);
            carta.name = nomeDaCarta;
            CartaScript cartaScript = carta.GetComponent<CartaScript>();
            cartaScript.identificadorDaCarta = cartaSO;
            //cartaScript.spriteDaCarta.sortingOrder = i;
            Baralho.Add(carta);
        }
        Shuffle(Baralho);
        SeparaBaralho();
        StartCoroutine(DistribuiCarta());

    }

    private IEnumerator DistribuiCarta()
    {
        for(int i = 0; i < CartasDoPlayer.Count; i++)
        {
            var cartaP = CartasDoPlayer.ElementAt(i);
            StartCoroutine(AdicionaNoMonte(cartaP, posicaoCartasJogador));
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(AdicionaNoMonte(CartasDoAdversario.ElementAt(i), posicaoCartasAdversario));
            yield return new WaitForSeconds(0.2f);
        }
        //yield return new WaitForSeconds(1f);
        SetGameReady(true);
    }

    

    public IEnumerator AdicionaNoMonte(GameObject carta, GameObject monteASerAdd)
    {
        var posicaoFinal = monteASerAdd.transform.position;
        float distanciaFaltante = (carta.transform.position - posicaoFinal).sqrMagnitude;
        while (distanciaFaltante > float.Epsilon)
        {
            Vector3 novaPosicao = Vector3.MoveTowards(carta.transform.position, posicaoFinal, 10f * Time.deltaTime);

            distanciaFaltante = (carta.transform.position - posicaoFinal).sqrMagnitude;
            carta.transform.parent = monteASerAdd.transform;
            carta.transform.position = novaPosicao;
            
            yield return new WaitForEndOfFrame();
        }
    }
    

    private void SeparaBaralho()
    {
        int cartasParaCadaJogador = totalDeCartas / 2;
        for (int i = cartasParaCadaJogador-1; i >= 0; i--)
        {
            Baralho[i].transform.parent = posicaoCartasJogador.transform;
            CartasDoPlayer.Add(Baralho[i]);
        }

        for (int j = totalDeCartas-1; j >= cartasParaCadaJogador; j--)
        {
            Baralho[j].transform.parent = posicaoCartasAdversario.transform;
            CartasDoAdversario.Add(Baralho[j]);
        }
    }


    /*
     * Esta é a função responsavel por embaralhar o baralho, após ele ter sido gereado
     */
    
    public static void Shuffle(List<GameObject> list) {
        var count = list.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = UnityEngine.Random.Range(i, count);
            GameObject tmp = list[i];
            var posTmpT = list[i].transform.position;
            var posTmpR = list[r].transform.position;
            list[i] = list[r];
            list[i].transform.position = posTmpT;
            list[r] = tmp;
            list[r].transform.position = posTmpR;

        }
    }

    private void OnMouseDown()
    {
        
    }

    internal void SetApertou(int stat)
    {
        statsEscolhido = stat;
        if(turnState == (int) TurnState.Escolher)
        {
            SetTurnState(TurnState.Comparar);
        }
    }

    

    private void MostrarEscolhas()
    {
        Escolhas.SetActive(isPlayerTurn);
    }

    /*
     * float novoX = carta.transform.position.x + (-0.2f*i);
            carta.transform.position = new Vector3(novoX, carta.transform.position.y, carta.transform.position.z);
     */

    // Update is called once per frame
    void Update()
    {
        if (gameReady)
        {
            if (gameStart)
            {
                DecidirPrimeiro();
                gameStart = false;
            }
            switch ((TurnState) turnState)
            {
                case TurnState.Comeco:
                    statsEscolhido = -1;
                    turnState++;
                    break;

                case TurnState.Escolher:
                    MostrarEscolhas();
                    break;
                case TurnState.Comparar:
                    MostrarPrimeiraCartaInimigo();
                    CompararValores();

                    break;
            }
            MostrarPrimeiraCarta();
        }
        
    }

    private void MostrarPrimeiraCartaInimigo()
    {
        var carta = GetPrimeiraCartaInimigo();
        if (!carta.faceParaCima)
        {
            carta.VirarCarta();
            Vector3 pos = carta.transform.position;
            pos.z = (carta.transform.parent.position.z - 0.1f);
            carta.transform.position = pos;
        }
    }

    private CartaScript GetPrimeiraCartaInimigo()
    {
        return CartasDoAdversario.ElementAt(0).GetComponent<CartaScript>();
    }

    private void CompararValores()
    {
        var cartaP = GetPrimeiraCarta();
        var cartaI = GetPrimeiraCartaInimigo();
        var cartaP_ID = cartaP.identificadorDaCarta;
        var cartaI_ID = cartaI.identificadorDaCarta;

        if (statsEscolhido != -1)
        {
            switch (cartaP_ID.popularidade.CompareTo(cartaI_ID.popularidade))
            {
                case -1:
                    SetTurnState(TurnState.Derrota);
                    break;
                case 0:
                    SetTurnState(TurnState.Empate);
                    break;
                case 1:
                    SetTurnState(TurnState.Vitoria);
                    break;
            }
            

        }
    }

    private void MostrarPrimeiraCarta()
    {
        var carta = GetPrimeiraCarta();
        if (!carta.faceParaCima)
        {
            carta.VirarCarta();
            Vector3 pos = carta.transform.position;
            pos.z = (carta.transform.parent.position.z - 0.1f);
            carta.transform.position = pos;
        }
    }

    private CartaScript GetPrimeiraCarta()
    {
        return CartasDoPlayer.ElementAt(0).GetComponent<CartaScript>();
    }
}
