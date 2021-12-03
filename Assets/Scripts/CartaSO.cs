using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Este é o Script de geração do ScriptableObject das cartas. Que salvam dados importantes como suas Sprites de frente
/// e verso, nome, número e seus atributos do jogo (popularidade, preco e saciedade).
/// </summary>

[CreateAssetMenu(menuName = "Carta")]
public class CartaSO : ScriptableObject
{
    public Sprite verso;
    public Sprite frente;
    
    public string nome;
    public int numero;
    public int popularidade;
    public int preco;
    public int saciedade;
    
}
