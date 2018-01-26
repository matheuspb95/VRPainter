using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour {
  Image panel;
  //Inicializa o panel como escuro
  //e chama a co-rotina FadeOut
  void Start () {
    panel = GetComponent<Image>();
    panel.color = Color.black;
    StartCoroutine(Fade(Color.black, Color.clear));
  }

  //Função para iniciar o FadeIn
  public void StartFade(){
    StartCoroutine(Fade(Color.clear, Color.black));
  }
  //Co-rotina de Fade
  public IEnumerator Fade(Color start, Color end){
    float value = 0;
    while(value < 1){
      yield return new WaitForEndOfFrame();
      value += Time.deltaTime;
      panel.color = Color.Lerp(start, end, value);
    }
    panel.color = end;
  }
}