using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
