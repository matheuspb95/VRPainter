using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {
	public Transform ObjectToRotate;
	public GearControllerScript controller;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(ObjectToRotate != null){
			try{
				ObjectToRotate.rotation = OVRInput.GetLocalControllerRotation (controller.Controller);
					
			} catch {}
		}
	}

	public void SelectObject(Transform obj){
		ObjectToRotate = obj;
	}
}
