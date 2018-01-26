using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RaycastTarget : MonoBehaviour {
  //Eventos quando o usuario olha para esse objeto, 
  //quando ele deixa de olhar para um objeto e 
  //quando termina de carregar a barra circular
  public UnityEvent OnStartLoad;
  public UnityEvent OnExitRaycast;
  public UnityEvent OnFillEnd;
    
  //Simula os eventos do Untiy com o evento da mira
  void Start () {
    OnStartLoad.AddListener(() => SimulateOnEnter());
    OnFillEnd.AddListener(() => SimulateClick());
    OnExitRaycast.AddListener(() => SimulateOnExit());
  }

  void SimulateOnEnter(){
    ExecuteEvents.Execute(gameObject, 
    new PointerEventData(EventSystem.current), 
    ExecuteEvents.pointerEnterHandler);
  }

  void SimulateClick(){
    ExecuteEvents.Execute(gameObject, 
    new PointerEventData(EventSystem.current),
    ExecuteEvents.pointerDownHandler);
    ExecuteEvents.Execute(gameObject, 
    new PointerEventData(EventSystem.current),
    ExecuteEvents.pointerClickHandler);
  }

  void SimulateOnExit(){
    ExecuteEvents.Execute(gameObject, 
    new PointerEventData(EventSystem.current),
    ExecuteEvents.pointerUpHandler);
    ExecuteEvents.Execute(gameObject, 
    new PointerEventData(EventSystem.current),
    ExecuteEvents.pointerExitHandler);
  }
}
