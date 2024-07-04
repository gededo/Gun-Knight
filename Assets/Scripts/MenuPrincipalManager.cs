using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    [SerializeField] private string Leveldojogo;
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelopcoes;
   public void Play() {
        PlayerPrefs.SetString("coins", "");
        PlayerPrefs.SetString("equippedpowerups", "");
        PlayerPrefs.SetInt("wallet", 0);
        SceneManager.LoadScene(Leveldojogo);
   }

   public void Optionsmenu() {
     painelMenuInicial.SetActive(false);
     painelopcoes.SetActive(true);
   }

   public void Closeoptionsmenu() {
     painelMenuInicial.SetActive(true);
     painelopcoes.SetActive(false);
   }

   public void Closethegame() {
    Debug.Log("Saiu do Jogo");
     Application.Quit();
   }
}
