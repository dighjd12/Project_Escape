using UnityEngine;
using System.Collections;

public class Camera_Ctrl : MonoBehaviour {

	float minFov = 15f;
	float maxFov = 90f;
	float sensitivity = 10f;
	
	void Update () {
		float fov = Camera.main.fieldOfView;
		fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
		fov = Mathf.Clamp(fov, minFov, maxFov);
		Camera.main.fieldOfView = fov;

	}

}
