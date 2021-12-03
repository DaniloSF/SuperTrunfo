using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Este é o Script que cuida das funções realizadas por cada carta individualmente.
/// 
/// </summary>

public class CartaScript : MonoBehaviour
{
    public CartaSO identificadorDaCarta;    // Variável do ScriptableObject dasta carta
    public bool faceParaCima;               // Variável que marca se a carta esta com a face virada para cima ou não (tem seu valor inicial no prefab do objeto)

    [HideInInspector]
    public SpriteRenderer spriteDaCarta;    // Variável que é usada para salvar o SpriteRender daquela carta.

    private void Start()
    {
        
    }

    
    /*
     * Este método vira a carta, ou seja, troca a sprite de face da carta para de dorso. Utilizando da propriedade "facePraCime" da
     * propria carta para identificar se a carta esta de face para cima ou não e trocando esse valor booleano.
     */
    public void VirarCarta()
    {
        faceParaCima = !faceParaCima;
        spriteDaCarta = GetComponent<SpriteRenderer>();
        Sprite s1;
        if (!faceParaCima)
        {
            s1 = identificadorDaCarta.verso;
            
        }
        else
        {
            s1 = identificadorDaCarta.frente;
            
        }
        spriteDaCarta.sprite = s1;
    }

    
}
