using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndLevel : MonoBehaviour {

	public GameMananger gm;

	void OnTriggerEnter(){
		gm.endGame();
	}

}
