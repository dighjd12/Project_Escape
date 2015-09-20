using UnityEngine;
using System.Collections;

public class RotateSelf : MonoBehaviour {

	public float Speed =5f;

	void Update () {
		transform.Rotate(new Vector3(0f, Speed, 0f));
	}
}
