using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalPosition : MonoBehaviour {
	public float radius, polar, azimuthal;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnValidate(){
		float x = radius * Mathf.Sin(polar * Mathf.Deg2Rad) * Mathf.Cos(azimuthal * Mathf.Deg2Rad);
		float y = radius * Mathf.Sin(polar * Mathf.Deg2Rad) * Mathf.Sin(azimuthal * Mathf.Deg2Rad);
		float z = radius * Mathf.Cos(polar * Mathf.Deg2Rad);

		transform.position = new Vector3(x,y,z);

		transform.LookAt(Vector3.zero);
	}
}
