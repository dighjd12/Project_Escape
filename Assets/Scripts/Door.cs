using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public Animation anim;
	
	public string openAnimName;
	public string closeAnimName;

	void Start(){
		anim = GetComponent<Animation>();
	}
	

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag=="Player"){

			StartCoroutine(OpenTheDoor());
		}
	}


	IEnumerator OpenTheDoor(){
		anim.Play(openAnimName);

		yield return new WaitForSeconds(3f);
		anim.Play(closeAnimName);
	}

}
