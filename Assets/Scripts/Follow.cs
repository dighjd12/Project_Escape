using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	public GameObject Target; //the gameobject for the camera to follow
	public float Distance; //difference in z-axis
	public float Height; //difference in y-axis
	public float Speed;
	public float rotSpeed;

	Vector3 Pos;

	void MoveUpDown(){

	}

	void Update () {
	
		Pos = new Vector3(Target.transform.position.x, Height, Target.transform.position.z - Distance);

		transform.Rotate(0f, Input.GetAxisRaw ("Horizontal") * rotSpeed * Time.deltaTime, 0f);
		/*
		this.gameObject.transform.position
			= Vector3.MoveTowards(this.gameObject.transform.position, Pos, Speed * Time.deltaTime);
		*/

		this.gameObject.transform.position
			= Vector3.Lerp(this.gameObject.transform.position, Pos, Speed * Time.deltaTime);

		MoveUpDown();

	}
}
