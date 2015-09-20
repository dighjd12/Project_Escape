using UnityEngine;
using System.Collections;

public class Door2 : MonoBehaviour {

	public GameObject door;
	Animation anim;
	AudioSource aud;
	public Troll_Ctrl tc;

	public GameMananger gm;

	public string openAnimName;
	public string closeAnimName;

	void Start(){
		anim = door.GetComponent<Animation>();
		aud = door.GetComponent<AudioSource>();
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag=="Player"){

			if(tc.carryingKey){
				Debug.Log("key");
				StartCoroutine(OpenTheDoor());
			}else{
				Debug.Log("no key");
				gm.GiveInstruction(3f, "you need a key to open this door");
			}

		}
	}

	IEnumerator OpenTheDoor(){
		anim.Play(openAnimName);
		aud.Play();

		yield return new WaitForSeconds(3f);
		anim.Play(closeAnimName);
	}

}
