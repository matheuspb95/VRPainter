using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LoadCircle : MonoBehaviour {
  //Eventos da barra circular
  //quando o usuário olha para um objeto
  //quando o usuário deixa de olhar para um objeto
  //quando termina de carregar a barra circular
  public UnityEvent OnExitRaycast;
  public UnityEvent OnStartLoad;
  public UnityEvent OnFillEnd;

  //Barra circular
  public Image RadialBar;

  public bool UseTimer;

  //Porcentagem de carregamento da barra
  private float loadFillAmount;
  //Tempo de carregamento total da barra
  public float LoadFillTime;
  //Co-rotina de carregamento da barra
  Coroutine FillCoroutine;
  IEnumerator Filling(RaycastTarget target){
    //Carregar a barra circular no tempo determinado
    loadFillAmount = 0;
    while(loadFillAmount < 1){
      loadFillAmount += Time.deltaTime / LoadFillTime;
      RadialBar.fillAmount = loadFillAmount;
      yield return new WaitForEndOfFrame();
    }
    loadFillAmount = 1;
    //Chama os eventos quando a barra terminar de carregar
    Select(target);
  }
  public void Select(RaycastTarget target){
    OnFillEnd.Invoke();
    if(target != null)
      target.OnFillEnd.Invoke();
  }

  public void StartFill(RaycastTarget target){
    //Chama os eventos quando a barra começa a carregar
    if(target != null)
      target.OnStartLoad.Invoke();
      OnStartLoad.Invoke();
      //Chama a co-rotina para iniciar o carregamento da barra
      if(UseTimer)
        FillCoroutine = StartCoroutine(Filling(target));
  }

  public void ResetFill(RaycastTarget target){
    //Chama os eventos quando deixa de carregar a barra
    if(target != null)
      target.OnExitRaycast.Invoke();
      OnExitRaycast.Invoke();
      //Reinicia a barra de carregamento
      //Para a co-rotina de carregamento da barra
      RadialBar.fillAmount = 0;
      if(FillCoroutine != null)
        StopCoroutine(FillCoroutine);
  }
}