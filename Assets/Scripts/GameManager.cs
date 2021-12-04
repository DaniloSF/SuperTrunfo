using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


/// <summary>
/// Esse é o Script que coordena todo cerebro do jogo, mantendo estados e requisitando mudança de turnos
/// </summary>
public class GameManager : TurnManager
{

    public int totalDeCartas = 20;              // Variavel que controla a quantidade de cartas do jogo

    public GameObject cartaPrefab;              // Prefab de cada carta
    public GameObject posicaoCartasJogador, posicaoCartasAdversario, 
        posicaoCartasMonte;                                                 // GameObject que já estará na cena e mostra onde se
                                                                            // disposta cada monte de carta
    public List<GameObject> Baralho = new List<GameObject>();               // Lista usada para controlar TODAS as cartas na mesa 
    public List<GameObject> CartasDoPlayer = new List<GameObject>();        // Lista usada para controlar as cartas do jogador
    public List<GameObject> CartasDoAdversario = new List<GameObject>();    // Lista usada para controlar as cartas do adversário
    public List<GameObject> CartasDoMonte = new List<GameObject>();         // Lista usada para controlar as cartas acumuladas no monte
    
    public BoxCollider2D PlayerCartasColider;
    //public TurnManager turnManager;
    public GameObject Escolhas;
    public GameObject EscolhasAdversario;
    public GameObject Numero;
    public GameObject NumeroAdversario;
    public GameObject Resultado;
    public int statsEscolhido = -1;

    public GameObject IndicadorEscolhido;

    public float delay = 0f;

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
        EscolhasAdversario.SetActive(!isPlayerTurn);
    }

    /*
     * Ativa os gameObjects das escolhas individualmente
     */
    private void MostrarEscolhaSelecionada()
    {
        switch(statsEscolhido)
        {
        case -1:
            Escolhas.transform.GetChild(0).gameObject.SetActive(true);
            Escolhas.transform.GetChild(1).gameObject.SetActive(true);
            Escolhas.transform.GetChild(2).gameObject.SetActive(true);

            EscolhasAdversario.transform.GetChild(0).gameObject.SetActive(true);
            EscolhasAdversario.transform.GetChild(1).gameObject.SetActive(true);
            EscolhasAdversario.transform.GetChild(2).gameObject.SetActive(true);
            break;
        case 0:
            Escolhas.transform.GetChild(1).gameObject.SetActive(false);
            Escolhas.transform.GetChild(2).gameObject.SetActive(false);

            EscolhasAdversario.transform.GetChild(1).gameObject.SetActive(false);
            EscolhasAdversario.transform.GetChild(2).gameObject.SetActive(false);
            break;
        case 1:
            Escolhas.transform.GetChild(0).gameObject.SetActive(false);
            Escolhas.transform.GetChild(2).gameObject.SetActive(false);

            EscolhasAdversario.transform.GetChild(0).gameObject.SetActive(false);
            EscolhasAdversario.transform.GetChild(2).gameObject.SetActive(false);
            break;
        case 2:
            Escolhas.transform.GetChild(0).gameObject.SetActive(false);
            Escolhas.transform.GetChild(1).gameObject.SetActive(false);

            EscolhasAdversario.transform.GetChild(0).gameObject.SetActive(false);
            EscolhasAdversario.transform.GetChild(1).gameObject.SetActive(false);
            break;
        }
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
            switch ((TurnState)turnState)
            {
                case TurnState.Comeco:
                    statsEscolhido = -1;
                    turnState++;

                    Numero.GetComponent<TextMeshPro>().text = "x " + CartasDoPlayer.Count;
                    NumeroAdversario.GetComponent<TextMeshPro>().text = "x " + CartasDoAdversario.Count;
                    Resultado.GetComponent<TextMeshPro>().text = "";
                    break;

                case TurnState.Escolher:
                    MostrarPrimeiraCarta(CartasDoPlayer);
                    MostrarEscolhaSelecionada();
                    MostrarEscolhas();
                    if(!isPlayerTurn)
                    {
                        EscolhaInimigo();
                        SetTurnState(TurnState.Comparar);
                        delay = 1.5f;
                    }
                    else delay = 0f;
                    break;
                case TurnState.Comparar:
                    if (delay > float.Epsilon) { 
                        delay -= Time.deltaTime;
                    }
                    else
                    {
                        MostrarPrimeiraCarta(CartasDoAdversario);
                        MostrarEscolhaSelecionada();
                        CompararValores();
                        delay = 2f;
                    }
                    break;
                case TurnState.Derrota:
                    if (delay > float.Epsilon) { 
                        delay -= Time.deltaTime;
                    }
                    else
                    {
                        EsconderEscolhido();

                        if (CartasDoMonte.Count > 0)
                        {
                            AdicionaNoMonte(CartasDoMonte, totalDeCartas, CartasDoAdversario, 1, posicaoCartasAdversario, TurnState.Fim);
                            AdicionaNoMonte(CartasDoPlayer, 1, CartasDoAdversario, 0, posicaoCartasAdversario, TurnState.Fim);
                        }
                        else
                        {
                            AdicionaNoMonte(CartasDoPlayer, 1, CartasDoAdversario, 1, posicaoCartasAdversario, TurnState.Fim);
                        }
                        delay = 1.2f;
                    }
                    break;
                    
                case TurnState.Vitoria:
                    if (delay > float.Epsilon)
                    {
                        delay -= Time.deltaTime;
                    }
                    else
                    {
                        EsconderEscolhido();

                        if (CartasDoMonte.Count > 0)
                        {
                            AdicionaNoMonte(CartasDoMonte, totalDeCartas, CartasDoPlayer, 1, posicaoCartasJogador, TurnState.Fim);
                            AdicionaNoMonte(CartasDoAdversario, 1, CartasDoPlayer, 0, posicaoCartasJogador, TurnState.Fim);
                        }
                        else
                        {
                            AdicionaNoMonte(CartasDoAdversario, 1, CartasDoPlayer, 1, posicaoCartasJogador, TurnState.Fim);
                        }
                        delay = 1f;
                    }

                    break;
                case TurnState.Empate:
                    if (delay > float.Epsilon)
                        delay -= Time.deltaTime;
                    else
                    {
                        AdicionaNoMonte(CartasDoAdversario, 1, CartasDoMonte, 1, posicaoCartasMonte, TurnState.Comeco);
                        AdicionaNoMonte(CartasDoPlayer, 1, CartasDoMonte, 1, posicaoCartasMonte, TurnState.Comeco);
                        if (CartasDoAdversario.Count == 0 || CartasDoPlayer.Count == 0)
                        {
                            SetTurnState(TurnState.Fim);
                        }
                        delay = 1f;
                    }
                    break;

                case TurnState.Fim:
                    if(delay > float.Epsilon)
                    {
                        delay -= Time.deltaTime;
                    }
                    else
                    {
                        if (CartasDoAdversario.Count == 0)
                        {
                            VencedorGlobal.isWinner = true;
                            SceneManager.LoadScene("EndScreen");
                        }
                        else if (CartasDoPlayer.Count == 0)
                        {
                            VencedorGlobal.isWinner = false;
                            SceneManager.LoadScene("EndScreen");
                        }
                        else
                        {
                            PassTurn();
                            SetTurnState(TurnState.Comeco);
                        }
                    }
                    break;
            }
            
        }
        
    }

    private void MostrarEscolhido()
    {
        for (int i = 0; i < 3; i++)
        {
            if(i != statsEscolhido) IndicadorEscolhido.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        var display = IndicadorEscolhido.transform.GetChild(statsEscolhido).gameObject;
        display.SetActive(true);

        var textDisplay = IndicadorEscolhido.transform.GetChild(3).gameObject;
        textDisplay.SetActive(true);
        
    }

    private void EsconderEscolhido()
    {
        var display = IndicadorEscolhido.transform.GetChild(statsEscolhido).gameObject;
        display.SetActive(false);

        var textDisplay = IndicadorEscolhido.transform.GetChild(3).gameObject;
        textDisplay.SetActive(false);
    }
    /*
     * Chama VirarCarta da primeira carta na lista do Player e poe em relevo no axis Z para sorting order
     */
    private void MostrarPrimeiraCarta(List<GameObject> list)
    {
        var carta = GetPrimeiraCarta(list);
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
    private CartaScript GetPrimeiraCarta(List<GameObject> list)
    {
        return list.ElementAt(0).GetComponent<CartaScript>();
    }

    
    /*
     * Chamado assim que entrar no TurnState de Comparar, faz a comparação entre os valores escolhidos 
     * e coloca o state no resultado correspondente
     */
    private void CompararValores()
    {
        var cartaP = GetPrimeiraCarta(CartasDoPlayer);
        var cartaI = GetPrimeiraCarta(CartasDoAdversario);
        var cartaP_ID = cartaP.identificadorDaCarta;
        var cartaI_ID = cartaI.identificadorDaCarta;
        var cartaP_Stat = cartaP_ID.popularidade;
        var cartaI_Stat = cartaI_ID.popularidade;

        if (statsEscolhido != -1)
        {
            print("Stat: " + statsEscolhido);
            switch (statsEscolhido)
            {
                case 0:
                    cartaP_Stat = cartaP_ID.popularidade;
                    cartaI_Stat = cartaI_ID.popularidade;
                    break;
                case 1:
                    cartaP_Stat = cartaP_ID.preco;
                    cartaI_Stat = cartaI_ID.preco;
                    break;
                case 2:
                    cartaP_Stat = cartaP_ID.saciedade;
                    cartaI_Stat = cartaI_ID.saciedade;
                    break;
            }

            var textDisplay = IndicadorEscolhido.transform.GetChild(3).gameObject;
            textDisplay.GetComponent<TextMesh>().text = cartaP_Stat + " vs " + cartaI_Stat;

            switch (cartaP_Stat.CompareTo(cartaI_Stat))
            {
                case -1:
                    //print("Derrota");
                    Resultado.GetComponent<TextMeshPro>().text = "Derrota!";
                    SetTurnState(TurnState.Derrota);
                    break;
                case 0:
                    //print("Empate");
                    Resultado.GetComponent<TextMeshPro>().text = "Empate!";
                    SetTurnState(TurnState.Empate);
                    break;
                case 1:
                    //print("Vitoria");
                    Resultado.GetComponent<TextMeshPro>().text = "Vitória!";
                    SetTurnState(TurnState.Vitoria);
                    break;
            }

            
        }
    }

    /*
     *  Função chamada no final da comaparação para adicionar as cartas ao player
     */
    private void AdicionaNoMonte(List<GameObject> from, int fromCount, List<GameObject> to, int toCount, GameObject posicao,  TurnState nextTurnState)
    {
        print("A lista é igual o monte: " + from.Equals(CartasDoMonte));
        print("Quantas cartas tem: " + from.Count);
        for(var i = 0; from.Count > 0 && i < fromCount; i++)
        {
            print(fromCount);
            CartaScript cartaF = PopLista(from);

            if (cartaF != null)
            {
                cartaF.transform.parent = posicao.transform;
                to.Add(cartaF.gameObject);
                if(cartaF.faceParaCima) cartaF.VirarCarta();
                StartCoroutine(AdicionaNoMonte(cartaF.gameObject, posicao));
            }
            
        }
        
        for(var i = 0; to.Count > 0 && i < toCount; i++)
        {
            CartaScript cartaT = PopLista(to);

            if (cartaT != null)
            {
                cartaT.transform.parent = posicao.transform;
                to.Add(cartaT.gameObject);
                if(cartaT.faceParaCima) cartaT.VirarCarta();
                StartCoroutine(AdicionaNoMonte(cartaT.gameObject, posicao));
            }
        }
        
        
        SetTurnState(nextTurnState);
    }

    private CartaScript PopLista(List<GameObject> list)
    {
        if (list.Count == 0) return null;
        var cartaP = GetPrimeiraCarta(list);
        list.RemoveAt(0);
        return cartaP;
    }

    /*
     *  Função para o adversário escolher um atributo
     */

    private void EscolhaInimigo()
    {
        var carta = GetPrimeiraCarta(CartasDoAdversario);
        var maior = carta.identificadorDaCarta.popularidade;
        statsEscolhido = 0;

        if(carta.identificadorDaCarta.preco > maior) 
        {
            maior = carta.identificadorDaCarta.preco;
            statsEscolhido = 1;
        }
        if(carta.identificadorDaCarta.saciedade > maior) 
        {
            maior = carta.identificadorDaCarta.saciedade;
            statsEscolhido = 2;
        }      
        print("Stat: " + statsEscolhido);   
    }
}
