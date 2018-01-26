using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearControllerScript : MonoBehaviour {
	public Transform TrackingSpace;
	public bool ControllerIsConnected {
			get {
				OVRInput.Controller controller =OVRInput.GetConnectedControllers () & (OVRInput.Controller.LTrackedRemote | OVRInput.Controller.RTrackedRemote);
				return controller == OVRInput.Controller.LTrackedRemote || controller == OVRInput.Controller.RTrackedRemote;
			}
		}
	public OVRInput.Controller Controller {
			get {
				OVRInput.Controller controller = OVRInput.GetConnectedControllers ();
				if ((controller & OVRInput.Controller.LTrackedRemote) == OVRInput.Controller.LTrackedRemote) {
					return OVRInput.Controller.LTrackedRemote;
				} else if ((controller & OVRInput.Controller.RTrackedRemote) == OVRInput.Controller.RTrackedRemote) {
					return OVRInput.Controller.RTrackedRemote;
				}
				return OVRInput.GetActiveController ();
			}
		}

	public Vector3 ControllerWorldPosition {
		get {
			Matrix4x4 localToWorld = TrackingSpace.localToWorldMatrix;
	
			Vector3 localStartPoint = OVRInput.GetLocalControllerPosition (Controller);
    	
			return localToWorld.MultiplyPoint(localStartPoint);
		}
	}

	public Vector3 WorldEndPoint {
		get {
			Matrix4x4 localToWorld = TrackingSpace.localToWorldMatrix;
			Quaternion orientation = OVRInput.GetLocalControllerRotation (Controller);
			Vector3 localStartPoint = OVRInput.GetLocalControllerPosition (Controller);
			Vector3 localEndPoint = localStartPoint + ((orientation * Vector3.forward) * 500.0f);
			return localToWorld.MultiplyPoint(localEndPoint);
		}
	}

	public Quaternion ControllerWorldRotation {
		get {
			return OVRInput.GetLocalControllerRotation (Controller);
		}
	}

	public Ray ControllerRay {
		get {
			Matrix4x4 localToWorld = TrackingSpace.localToWorldMatrix;
			Quaternion orientation = OVRInput.GetLocalControllerRotation (Controller);
	
			Vector3 localStartPoint = OVRInput.GetLocalControllerPosition (Controller);
			Vector3 localEndPoint = localStartPoint + ((orientation * Vector3.forward) * 500.0f);
    	
			Vector3 worldStartPoint = localToWorld.MultiplyPoint(localStartPoint);
			Vector3 worldEndPoint = localToWorld.MultiplyPoint(localEndPoint);
					
			// Create new ray
			return new Ray(worldStartPoint, worldEndPoint - worldStartPoint);
			 
		}
	}

	public LineRenderer line;
	public bool ShowLine;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
