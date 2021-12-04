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
            GetComponent<Text>().text = "Parabéns! Você venceu!";
        }
        else
        {
            GetComponent<Text>().text = "A CPU te derrotou, que pena!";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
