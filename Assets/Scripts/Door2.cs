using UnityEngine;
using System.Collections;

public class Door2 : MonoBehaviour {

	public GameObject door;
	Animation anim;
	public Troll_Ctrl tc;

	public string openAnimName;
	public string closeAnimName;

	void Start(){
		anim = door.GetComponent<Animation>();
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag=="Player"){

			if(tc.carryingKey){
				Debug.Log("key");
				StartCoroutine(OpenTheDoor());
			}else{
				Debug.Log("no key");
			}

		}
	}

	IEnumerator OpenTheDoor(){
		anim.Play(openAnimName);
		
		yield return new WaitForSeconds(3f);
		anim.Play(closeAnimName);
	}

}
