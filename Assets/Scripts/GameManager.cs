using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// Neste Script é feito todo o controle geral do jogo e administração das regras
/// </summary>

public class GameManager : MonoBehaviour
{
    public int totalDeCartas = 20;              // Variavel que controla a quantidade de cartas do jogo
    public GameObject cartaPrefab;              // Prefab de cada carta
    public GameObject posicaoCartasJogador, posicaoCartasAdversario;        // GameObject que já estará na cena e mostra onde se
                                                                                // disposta cada monte de carta
    public List<GameObject> Baralho = new List<GameObject>();                   // Lista usada para controlar TODAS as cartas na mesa 
    public List<GameObject> CartasDoPlayer = new List<GameObject>();            // Lista usada para controlar as cartas do jogador
    public List<GameObject> CartasDoAdversario = new List<GameObject>();        // Lista usada para controlar as cartas do adversário

    
    /*
     * No método start, a principio temo o chamdo do método para gerar o baralho e em seguida
     * viramos todas as cartas para baixo.
     */
    void Start()
    {
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
            nomeDaCarta = "Carta " + (i+1);
            GameObject carta = Instantiate(cartaPrefab, posicaoCartasJogador.transform.position, Quaternion.identity);
            cartaSO = (Resources.Load<CartaSO>("CartasSO/" + nomeDaCarta)); // Buscando o SciptableObject de cada carta 
            carta.name = nomeDaCarta;       // Alterando o nome do GameObject na hierarquia
            CartaScript cartaScript = carta.GetComponent<CartaScript>();
            cartaScript.identificadorDaCarta = cartaSO;
            Baralho.Add(carta);
        }
        Shuffle(Baralho);
        SeparaBaralho();
        foreach (var carta in CartasDoPlayer)
        {
            AdicionaNoMonte(carta, posicaoCartasJogador);
        }
        foreach (var carta in CartasDoAdversario)
        {
            AdicionaNoMonte(carta, posicaoCartasAdversario);
        }
        
    }

    /*
     * Essa função recebe 2 parametros que são a carta e o monte do jogador para qual a carta deve ir. Em seguida ela
     * faz o monte ser um pai da carta na hierarquia e em seguida move a carta até o local de seu pai na tela.
     */
    public void AdicionaNoMonte(GameObject carta, GameObject monteASerAdd)
    {
        carta.transform.parent = monteASerAdd.transform;
        carta.transform.position = monteASerAdd.transform.position;
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
     * entre o indice atual da iteração e o tamanho total da lista.
     */
    
    public static void Shuffle<GameObject>(List<GameObject> list) {
        var count = list.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }
    }

    
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
