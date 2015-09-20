using UnityEngine;
using System.Collections;

public class MonsterBox : MonoBehaviour {

	int currIndex;
	public GameObject[] sequence;
	int sequenceLength;
	public GameObject Key_door1;

	public GameMananger gm;
	public AudioSource aud;
	public GameObject[] possibleEnemies;

	//public GameObject enemySlime;
	public Vector3[] spawnPositions;
	public bool CanCheckSequence;

	public AudioSource keyAud;

	// Use this for initialization
	void Start () {
		currIndex = 0;
		sequenceLength = sequence.Length;
		print (sequenceLength);
		spawnPositions = new Vector3[4];
		spawnPositions[0] = new Vector3(282f, -11f, 352f);
		spawnPositions[1] = new Vector3(566f, -11f, 377f);
	//	spawnPositions[2] = new Vector3(409f, -11f, 427f);
	//	spawnPositions[3] = new Vector3(570f, -11f, 473f);
		CanCheckSequence = true;

	}


	public void CheckSequence(GameObject nextObject){
		if(nextObject.Equals(sequence[currIndex])){
			gm.GiveNotice(1f, "Correct!");
			currIndex++;
			//if end of sequence
			if(currIndex==sequenceLength){
				gm.GiveNotice(1f, "Found the Key!");
				currIndex=0;

				Instantiate(Key_door1);
				keyAud.Play();
				CanCheckSequence=false; 
			}
		}else{
			gm.GiveNotice(4f, "Incorrect! Enemies will spawn soon.");
			currIndex=0;

			int rand = Mathf.RoundToInt(Random.Range(0, possibleEnemies.Length-1));
			GameObject enemy = possibleEnemies[rand];
			StartCoroutine(SpawnMonsters(enemy));
		}
	}

	IEnumerator SpawnMonsters(GameObject enemy){
		CanCheckSequence=false;
		yield return new WaitForSeconds(4f);
		aud.Play();

		for(int n=0; n<spawnPositions.Length; n++){
			Instantiate (enemy, spawnPositions[n], Quaternion.identity);
		}
		CanCheckSequence=true;
	}
}
