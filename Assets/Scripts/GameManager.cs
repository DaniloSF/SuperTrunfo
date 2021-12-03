using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;


/// <summary>
/// Esse é o Script que coordena todo cerebro do jogo, mantendo estados e requisitando mudança de turnos
/// </summary>
public class GameManager : TurnManager
{

    public int totalDeCartas = 20;              // Variavel que controla a quantidade de cartas do jogo

    public GameObject cartaPrefab;              // Prefab de cada carta
    public GameObject posicaoCartasJogador, posicaoCartasAdversario;        // GameObject que já estará na cena e mostra onde se
                                                                            // disposta cada monte de carta
    public List<GameObject> Baralho = new List<GameObject>();               // Lista usada para controlar TODAS as cartas na mesa 
    public List<GameObject> CartasDoPlayer = new List<GameObject>();        // Lista usada para controlar as cartas do jogador
    public List<GameObject> CartasDoAdversario = new List<GameObject>();    // Lista usada para controlar as cartas do adversário
    
    public BoxCollider2D PlayerCartasColider;
    //public TurnManager turnManager;
    public GameObject Escolhas;
    public int statsEscolhido = -1;


    /*
     * No método start, a principio temo o chamdo do método para gerar o baralho e em seguida
     * viramos todas as cartas para baixo.
     */
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

    
    /*
     * Para o método GeraBaralho temos uma iteração inicial para instanciar todas as 20 cartas do baralho, adicionar
     * seu respectivo ScriptableObject de cada uma das cartas e depois adiciona-las na lista Baralho.
     * Em seguida chamamos a função Shuffle que faz o embaralhamento das cartas (na propria lista Baralho)
     * Depois chamamos a função SeparaBaralho que distribui as cartas entre player e adversário.
     * E por ultimo colocamos cada monte de carta em seu local correto
     */
    public void GeraBaralho()
    {
        string nomeDaCarta;
        CartaSO cartaSO;

        for (int i = 0; i < totalDeCartas; i++)
        {
            Vector3 posicao = new Vector3(0, 0, 0);
            nomeDaCarta = "Carta " + (i + 1);
            GameObject carta = Instantiate(cartaPrefab, transform.position + new Vector3(0, 0, i * 0.1f), Quaternion.identity);
            cartaSO = Resources.Load<CartaSO>("CartasSO/" + nomeDaCarta);   // Buscando o ScriptableObject de cada carta 
            carta.name = nomeDaCarta;       // Alterando o nome do GameObject na hierarquia
            CartaScript cartaScript = carta.GetComponent<CartaScript>();
            cartaScript.identificadorDaCarta = cartaSO;
            //cartaScript.spriteDaCarta.sortingOrder = i;
            Baralho.Add(carta);
        }
        Shuffle(Baralho);
        SeparaBaralho();
        StartCoroutine(DistribuiCarta());

    }

    /*
     * Metodo responsavel por colcocar cada carta na mao correta de cada player
     * Feito IEnumerator para ser assincrono e poder animar individualmente e alternadamente
     */
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

    
    /*
     * Essa função recebe 2 parametros que são a carta e o monte do jogador para qual a carta deve ir. Em seguida ela
     * faz o monte ser um pai da carta na hierarquia e em seguida move a carta até o local de seu pai na tela.
     */
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
    

    /*
     * Esta função distribui as cartas (que estão na lista Baralho) para cada um dos jogadores na sequencia que elas estão
     * dispostas. Uma vez que, antes dessa função, chamamos a função para embaralhar a lista Baralho.
     */
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
     * Esta é a função responsavel por embaralhar o baralho, após ele ter sido gereado. Durante
     * a iteração, trocamos o elemento atual da iteração com o elemento de indice r que é gerado aleatoriamente
     * entre o indice atual da iteração e o tamanho total da lista. Manipulamos a posição para que não há
     * luta de quem está na frente pelo Sorting Layer
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

    /*
     * Funcao que os botoes das escolhas referenciam ao serem clicadas, muda o estado do turno para a comparação
     */
    internal void SetApertou(int stat)
    {
        statsEscolhido = stat;
        if(turnState == (int) TurnState.Escolher)
        {
            SetTurnState(TurnState.Comparar);
        }
    }

    /*
     * Ativa o gameObject parente das Escolhas
     */
    private void MostrarEscolhas()
    {
        Escolhas.SetActive(isPlayerTurn);
    }

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

    /*
     * Chama VirarCarta da primeira carta na lista do Inimigo e poe em relevo no axis Z para sorting order
     */
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

    /*
     * Retorna a referencia da primeira carta do Inimigo
     */
    private CartaScript GetPrimeiraCartaInimigo()
    {
        return CartasDoAdversario.ElementAt(0).GetComponent<CartaScript>();
    }

    /*
     * Chama VirarCarta da primeira carta na lista do Player e poe em relevo no axis Z para sorting order
     */
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

    /*
     * Retorna a referencia da primeira carta do Player
     */
    private CartaScript GetPrimeiraCarta()
    {
        return CartasDoPlayer.ElementAt(0).GetComponent<CartaScript>();
    }

    
    /*
     * Chamado assim que entrar no TurnState de Comparar, faz a comparação entre os valores escolhidos 
     * e coloca o state no resultado correspondente
     */
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

    
}
