using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class CartaScript : MonoBehaviour
{
    public CartaSO identificadorDaCarta;
    public bool faceParaCima;
    private SpriteRenderer spriteDaCarta;

    private void Start()
    {
        
    }

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
