using UnityEngine;
using System.Collections;

public class TreasureBox : MonoBehaviour {

	public GameMananger gm;
	public float fadeTime;
	public AudioSource aud;

	bool canDiscover = true;

	public void Discover(){

		if(canDiscover){
			aud.Play();
			StartCoroutine("FadeOut");
			gm.WhitenImage(gm.Treasure1);
			gm.foundTreasure=true;
			canDiscover=false;
			//play sound?
		}
	}
	
	IEnumerator FadeOut(){
		MeshRenderer[] meshRends = GetComponentsInChildren<MeshRenderer>();

		//safe since the script is specific to one prefab
		Material mat1 = meshRends[0].material;
		Material mat2 = meshRends[1].material;
		Material[] mats = new Material[]{mat1, mat2};
		foreach(Material mat in mats){
			while(mat.color.a > 0)
			{
				Color newColor = mat.color;
				newColor.a -= Time.deltaTime / fadeTime;
				mat.color = newColor;
				yield return null;
			}
			if(mat.color.a<0){
				Destroy(this.gameObject);
			}
		}

		
	}

}
