using UnityEngine;
using System.Collections;

public class TreasureBox : MonoBehaviour {

	public GameMananger gm;
	public float fadeTime;

	public void Discover(){

		StartCoroutine("FadeOut");
		gm.WhitenImage(gm.Treasure1);
		gm.foundTreasure=true;
		//play sound?
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
