using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Este Script muda a tela de fim de jogo dependendo do vencedor.
/// 
/// </summary>

public class EndManager : MonoBehaviour
{
    // Mensagens aleatórias na tela de vitória ou derrota
    string[] LoseMessages = new string[]
    {
        "\"CPU: Hoje é o seu dia de comer peixe!\"",
        "\"CPU: Fui eu que coloquei aquela larva no seu almoço!\"",
        "\"CPU: Você devia ter ido comprar um salgado!\"",
        "\"CPU: Furei a sua fila, amigo!\""
    };

    string[] WinMessages = new string[]
    {
        "\"PLAYER: É isso que chamam de 'inteligência' artificial?\"",
        "\"PLAYER: Esqueceram de descomentar seu código!\"",
        "\"PLAYER: Agora eu posso comer em paz o meu almoço!\"",
        "\"PLAYER: Ops! Apertei Alt + F4!\""
    };

    public GameObject Tela;
    // Start is called before the first frame update
    void Start()
    {
        
        Tela = GameObject.Find("Main Camera");
        if (VencedorGlobal.isWinner)
        {
            Tela.GetComponent<Camera>().backgroundColor = new Color(0f, 0f, 0.7f);
            AudioSource VictorySong = GameObject.Find("VictorySong").GetComponent<AudioSource>();
            VictorySong.Play();
            GetComponent<Text>().text = "Parabéns! Você venceu!";
            GameObject.Find("Mensagem").GetComponent<Text>().text = WinMessages[(int)Random.Range(0, WinMessages.Length)];
        }
        else
        {
            Tela.GetComponent<Camera>().backgroundColor = new Color(0.7f, 0f, 0f);
            AudioSource DefeatSong = GameObject.Find("DefeatSong").GetComponent<AudioSource>();
            DefeatSong.Play();
            GetComponent<Text>().text = "A CPU te derrotou, que pena!\n";
            GameObject.Find("Mensagem").GetComponent<Text>().text = LoseMessages[(int)Random.Range(0, WinMessages.Length)];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
