using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerateObject : MonoBehaviour {
	public float ForceMultiplier = 100;
	public Rigidbody selectedObj;
	public GearControllerScript controller;
	public LineRenderer line;

	public int MaxQueue = 8;
	public Queue<Vector3> Accels = new Queue<Vector3>();
	public Vector3 AccelSoma;
	int accelCount = 0;
	public int LaunchCount = 10;
	public float ForceLimit = 5f;

	public HoldObject holdScript;
	// Use this for initialization
	void Start () {
		holdScript = GetComponent<HoldObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if(selectedObj != null){
			try{
				Vector3 accel = OVRInput.GetLocalControllerAcceleration(controller.Controller) * ForceMultiplier;
				Accels.Enqueue(accel);
				if(Accels.Count >= MaxQueue){
					Accels.Dequeue();
				}
				Vector3 accelResult = Vector3.zero;
				float mult = 1;
				Vector3[] list = new Vector3[MaxQueue];
				Accels.CopyTo(list, 0);
				//print(list.Length);
				for(int i = list.Length - 1; i >= 0; i--){
					mult = mult * 0.75f;
					//print(list[i]);
					accelResult += list[i] * mult;
				}
				line.SetPosition(0, selectedObj.transform.position);
				line.SetPosition(1, (selectedObj.transform.position) + accelResult);
				if(accelResult.magnitude > ForceLimit){
					print(accelResult.magnitude);
					//selectedObj.AddForce(accelResult);
					AccelSoma += accelResult;
					accelCount++;
					if(accelCount >= LaunchCount){
						holdScript.DeselectObj();
						selectedObj.AddForce(AccelSoma / accelCount);
						accelCount = 0;
						AccelSoma = Vector3.zero;
					}
				} 
			} catch {}
		}		
	}

	public void SetSelected(Rigidbody obj){
		selectedObj = obj;
	}
}
