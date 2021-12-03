using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicar : MonoBehaviour
{
    public int stat;
    public GameManager manager;
    private void Start()
    { 
    }
    private void OnMouseUpAsButton()
    {
        manager.SetApertou(stat);
    }
}
