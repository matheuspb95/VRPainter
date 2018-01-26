using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldObject : MonoBehaviour {
	public Transform SelectedObject;
	public float Distance;

	public GearControllerScript controller;

	float t = 0;
	public float SelectTime = 0.5f;

	Vector3 StartPos;

	float StartScale;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(SelectedObject != null){
			if(t < 1){
				t += Time.deltaTime / SelectTime;
				SelectedObject.position = Vector3.Lerp(StartPos, controller.ControllerRay.GetPoint(Distance), t);
				return;
			}
			SelectedObject.position = controller.ControllerRay.GetPoint(Distance);
			float scale = SelectedObject.localScale.x;
			//scale += Input.GetAxis("Vertical") * 0.01f;
			scale += OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).x * 0.01f;
			print(OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad));
			SelectedObject.localScale = Vector3.one * scale;
		}
	}

	public void SelectObject(Transform target){
		if(SelectedObject == null){
			t = 0;
			StartPos = target.position;
			StartScale = target.localScale.x;
			SelectedObject = target;
		}
	}

	public void DeselectObj(){
		SelectedObject = null;
	}
}
