using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Este Script é responsável pelos botões na tela de fim de jogo.
/// 
/// </summary>

public class Botoes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * Inicia o jogo
     */
    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    /*
     * Vai para a tela de créditos
     */
    public void Credits()
    {
        SceneManager.LoadScene("Creditos1");
    }

    /*
     * método para ir para a próxima tela de créditos quando apertado o botão
     */
    public void NextCredits()
    {
        SceneManager.LoadScene("Creditos2");
    }

    /*
     * Sair do jogo
     */
    public void Sair()
    {
        print("Exiting...");
        Application.Quit();
    } 

    /*
     * Abre a tela de instruções
     */
    public void Instructions()
    {
        SceneManager.LoadScene("Instructions");
    } 

    /*
     * Volta para o menu principal
     */
    public void Menu()
    {
        SceneManager.LoadScene("Inicial");
    }  
}
