using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameMananger : MonoBehaviour {

	public float time;

	public Text timeText;
	public Image CurrChar;
	public Image Life1;
	public Image Life2;
	public Image Life3;
	public Image Treasure1;
	public bool foundTreasure;

	public Text noticeText;
	public Text instructionText;
	
	public GameObject replayButton;
	bool gameEnd;
	public GameObject startButton;
	public Image sceneHideImg;

	public GameObject Character1; //Knight
	public GameObject Character2; //Troll
	public GameObject CurrentPlayer;
	public Character_Ctrl cc;

	public Transform hintPlane;
	public GameObject hint1;
	bool HintUp;
	public AudioSource hintAud;

	public Button OtherChar1;
	public Image OtherChar1Img;
	

	// Use this for initialization
	void Start () {
		Time.timeScale=0;// stop scene for seconds!!!!!!!!!
		DimImage(Treasure1);
		OtherChar1.onClick.AddListener(() => {SwitchChar(OtherChar1Img);});
		Character2.SetActive(false);
		CurrentPlayer = Character1;
		HintUp=false;
		noticeText.enabled = false;
		foundTreasure=false;
		gameEnd=false;

		replayButton.SetActive(false);
		replayButton.GetComponent<Button>().onClick.AddListener(() => {restartGame();});
		startButton.GetComponent<Button>().onClick.AddListener(() => {startGame();});

	}

	public void endGame(){
		gameEnd=true;
		cc.PS = PlayerState.Dead;
		//disable movement
		float endTime = time;
		float timeScore = 1000f - Mathf.Floor(endTime);
		float treasureScore = 0f;
		if(foundTreasure){
			treasureScore= 200f;
		}
		float totalScore = timeScore + treasureScore;
		GiveInstruction(20f, "Level Completed! Your score is " + totalScore + "!");
		
		replayButton.SetActive(true);
		//give out the restart button 
	}

	public void startGame (){
		startButton.SetActive(false);
		StartCoroutine(Fade());
		Time.timeScale=1;
	}

	IEnumerator Fade()
	{
		float duration = 4f;
		float currentTime = 0f;
		float currentAlpha = sceneHideImg.color.a;
		float targetAlpha = (currentAlpha+1) % 2;
		while(currentTime < duration)
		{
			//fade in
			float alpha = Mathf.Lerp(currentAlpha, targetAlpha, currentTime/duration);
			sceneHideImg.color = new Color(sceneHideImg.color.r,
			                               sceneHideImg.color.g, sceneHideImg.color.b, alpha);
			currentTime += Time.deltaTime;
			yield return null;
		}
		yield return new WaitForSeconds(2f);
		
	}

	public void restartGame (){
		gameEnd=false;
		replayButton.SetActive(false);
		Fade();
		Application.LoadLevel ("Scene1");
	}

	void Update(){
		if(!gameEnd){
			time += Time.deltaTime;
			timeText.text = "Time : " + Mathf.Floor(time).ToString();
		}

		if(cc.PS==PlayerState.Dead){
			gameEnd=true;
			replayButton.SetActive(true);
		}

		if(Input.GetKeyDown(KeyCode.S)){
			SwitchCharacters();
			//switch between two characters
		}

		if(!HintUp){
			Collider[] objectsOnHintPlane = Physics.OverlapSphere(hintPlane.position, 25f);

			for(int n=0; n<objectsOnHintPlane.Length; n++){
				
				if(objectsOnHintPlane[n].gameObject.tag=="Player"){
					StartCoroutine(ShowHintPlane());
				}
			}
		}

	}

	public void GiveInstruction(float time, string message){
		instructionText.text = message;
		StartCoroutine(FadeInAndOutInstruction(time));
	}
	
	IEnumerator FadeInAndOutInstruction(float time)
	{
		float duration = 1.5f;
		float currentTime = 0f;
		while(currentTime < duration)
		{
			//fade in
			float alpha = Mathf.Lerp(0f, 1f, currentTime/duration);
			instructionText.color = new Color(instructionText.color.r,
			                            instructionText.color.g, instructionText.color.b, alpha);
			currentTime += Time.deltaTime;
			yield return null;
		}

		yield return new WaitForSeconds(time);

		currentTime = 0f;
		while(currentTime < duration)
		{
			//fade out
			float alpha = Mathf.Lerp(1f, 0f, currentTime/duration);
			instructionText.color = new Color(instructionText.color.r,
			                                  instructionText.color.g, instructionText.color.b, alpha);
			currentTime += Time.deltaTime;
			yield return null;
		}

	}

	public void GiveNotice(float time, string message){
		noticeText.text = message;
		StartCoroutine(GiveNoticeForTime(time));
	}

	IEnumerator GiveNoticeForTime(float time){
		noticeText.enabled = true;
		float sTime = 0f;
		while(sTime<time){
			sTime += .5f;
			if(noticeText.fontSize==20){
				noticeText.fontSize = 25;
			}else{
				noticeText.fontSize = 20;
			}
			yield return new WaitForSeconds(.5f);
		}
		noticeText.enabled = false;

	}

	IEnumerator ShowHintPlane(){
		yield return new WaitForSeconds(3f);

		Collider[] objectsOnHintPlane = Physics.OverlapSphere(hintPlane.position, 25f);
		
		for(int n=0; n<objectsOnHintPlane.Length; n++){
			
			if(objectsOnHintPlane[n].gameObject.tag=="Player"){
				hint1.SetActive(true);
				HintUp=true;
				hintAud.Play();
			}
		}
	}

	void SwitchCharacters(){
		Vector3 currPos;

		if(CurrentPlayer==Character1){
			currPos = Character1.transform.position;
			Character2.transform.position = currPos - new Vector3(0f, 7f);
			Character2.GetComponent<Spec_Ctrl>().UpdateInfo();
		//	Character1.GetComponent<Spec_Ctrl>().SwitchOut();
			Character2.SetActive(true);
			Character1.SetActive(false);
			SwitchChar(OtherChar1Img);
			CurrentPlayer=Character2;
		}else{
			currPos = Character2.transform.position;
			Character1.transform.position = currPos + new Vector3(0f, 7f);
			Character1.GetComponent<Spec_Ctrl>().UpdateInfo();
		//	Character2.GetComponent<Spec_Ctrl>().SwitchOut();
			Character1.SetActive(true);
			Character2.SetActive(false);
			SwitchChar(OtherChar1Img);
			CurrentPlayer=Character1;
		}

	}

	public void DimImage(Image img){
		//img must be one of the images variables above
		img.color = Color.gray;
	}

	public void WhitenImage(Image img){
		img.color = Color.white;
	}

	public void SwitchChar(Image img){
		Sprite tmp = img.sprite;
		Sprite tmp2 = CurrChar.sprite;

		img.sprite = tmp2;
		CurrChar.sprite = tmp;
	}

}
