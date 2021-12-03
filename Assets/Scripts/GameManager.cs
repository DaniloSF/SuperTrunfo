using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalDeCartas = 20;
    public GameObject cartaPrefab;
    public GameObject posicaoCartasJogador, posicaoCartasAdversario;
    public List<GameObject> Baralho = new List<GameObject>();
    public List<GameObject> CartasDoPlayer = new List<GameObject>();
    public List<GameObject> CartasDoAdversario = new List<GameObject>();

    
    
    void Start()
    {
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
            nomeDaCarta = "Carta " + (i+1);
            GameObject carta = Instantiate(cartaPrefab, posicaoCartasJogador.transform.position, Quaternion.identity);
            cartaSO = (Resources.Load<CartaSO>("CartasSO/" + nomeDaCarta));
            carta.name = nomeDaCarta;
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

    public void AdicionaNoMonte(GameObject carta, GameObject monteASerAdd)
    {
        carta.transform.parent = monteASerAdd.transform;
        carta.transform.position = monteASerAdd.transform.position;
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

    
    
    /*
     * float novoX = carta.transform.position.x + (-0.2f*i);
            carta.transform.position = new Vector3(novoX, carta.transform.position.y, carta.transform.position.z);
     */

    // Update is called once per frame
    void Update()
    {
        
    }
}
