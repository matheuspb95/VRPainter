using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {
	
	public float velocity = 1;
	
	// Update is called once per frame
	void Update () {
		float movex = Input.GetAxis("Mouse X");
		float movey = Input.GetAxis("Mouse Y");

		transform.Rotate(new Vector3(-movey * velocity, movex * velocity, 0));
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
	}
}
