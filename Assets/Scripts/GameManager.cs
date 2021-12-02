using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalDeCartas = 20;
    public GameObject cartaPrefab;
    public GameObject posicaoCartasJogador, posicaoCartasAdversario;
    
    void Start()
    {
        GeraBaralho();
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
            cartaSO = (CartaSO) (Resources.Load<CartaSO>(nomeDaCarta));
            carta.GetComponent<CartaScript>().identificadorDaCarta = cartaSO;
            float novoX = carta.transform.position.x + (-0.2f*i);
            carta.transform.position = new Vector3 (novoX, carta.transform.position.y , carta.transform.position.z);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
