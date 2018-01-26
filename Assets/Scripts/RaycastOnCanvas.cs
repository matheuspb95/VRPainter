using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RaycastOnCanvas : MonoBehaviour {
	public Transform Reticle;
	public Vector3 screenPos;
	public GraphicRaycaster raycaster;
	public Canvas canvas; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		screenPos = Camera.main.WorldToScreenPoint(Reticle.position);
		PointerEventData pointer = new PointerEventData(EventSystem.current);
		pointer.position = new Vector2(Screen.width/2, Screen.height/2);
		//pointer.worldNormal = Vector3.forward;
		List<RaycastResult> results = new List<RaycastResult>();
		raycaster.Raycast(pointer, results);
		foreach(RaycastResult obj in results){
			if(Input.GetButtonDown("Fire1")){
				ExecuteEvents.ExecuteHierarchy(obj.gameObject, pointer, ExecuteEvents.selectHandler);
				ExecuteEvents.ExecuteHierarchy(obj.gameObject, pointer, ExecuteEvents.pointerEnterHandler);
				ExecuteEvents.ExecuteHierarchy(obj.gameObject, pointer, ExecuteEvents.pointerDownHandler);
				ExecuteEvents.ExecuteHierarchy(obj.gameObject, pointer, ExecuteEvents.pointerClickHandler);
				ExecuteEvents.ExecuteHierarchy(obj.gameObject, pointer, ExecuteEvents.submitHandler);
				print(obj.gameObject);
			}
			if(Input.GetButtonUp("Fire1")){
				ExecuteEvents.ExecuteHierarchy(obj.gameObject, pointer, ExecuteEvents.pointerUpHandler);
				ExecuteEvents.ExecuteHierarchy(obj.gameObject, pointer, ExecuteEvents.deselectHandler);
			}
		}
	}
}
