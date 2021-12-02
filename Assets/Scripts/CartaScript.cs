using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartaScript : MonoBehaviour
{
    public CartaSO identificadorDaCarta;
    public bool estaVirada;
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
        estaVirada = false;
    }

}
