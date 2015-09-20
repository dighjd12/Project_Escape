using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public Animation anim;
	public AudioSource aud;
	public string openAnimName;
	public string closeAnimName;

	void Start(){
		anim = GetComponent<Animation>();
		aud = GetComponent<AudioSource>();
	}
	

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag=="Player"){

			StartCoroutine(OpenTheDoor());
		}
	}


	IEnumerator OpenTheDoor(){
		anim.Play(openAnimName);
		aud.Play();

		yield return new WaitForSeconds(3f);
		anim.Play(closeAnimName);
	}

}
