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
    // Start is called before the first frame update
    void Start()
    {
        if (VencedorGlobal.isWinner)
        {
            AudioSource VictorySong = GameObject.Find("VictorySong").GetComponent<AudioSource>();
            VictorySong.Play();
            GetComponent<Text>().text = "Parabéns! Você venceu!";
        }
        else
        {
            AudioSource DefeatSong = GameObject.Find("DefeatSong").GetComponent<AudioSource>();
            DefeatSong.Play();
            GetComponent<Text>().text = "A CPU te derrotou, que pena!";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
