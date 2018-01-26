using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneScript : MonoBehaviour {
  //Script que controla o FadePanel
  public FadeScript fade;
  //Função para iniciar o 
  //carregamento da cena
  public void LoadScene(string scene){
    StartCoroutine(FadeAndLoad(scene));
  }
  //Espera o fade para carregar a nova cena
  IEnumerator FadeAndLoad(string scene){
    yield return StartCoroutine
    (fade.Fade(Color.clear, Color.black));

    SceneManager.LoadScene(scene);
  }
}
