using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Mouse_Ctrl : MonoBehaviour {

//	public Transform Target; //position of the target basically
	public GameObject Cursor; //cursor to show at the mouse
	public Character_Ctrl cc; //to send info to the player ctrl
	public MonsterBox mb;
	public TreasureBox tb;
//	public Transform Target;
//	Transform camTr;
	Vector3 prevCamPos;

	GameObject zoomCam;
	
//	float zoomSpeed = 50f;
	bool Zoomed = false;

	void Start () {
//		camTr = Camera.main.transform;
	}

	void Update () {
	
		RaycastHit hit; //raycasthit is basically the point where ray hits sth (that sth's position)
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
			Cursor.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z); 
			//move the cursor to that point, except move it a bit upwards so it doesn't hide the target object


			if(Input.GetMouseButtonDown(0) && cc.PS!=PlayerState.Dead){

				if(!Zoomed){


					//move using mouse
					if(hit.transform.name == "level02"){

						print(Cursor.transform.position);
						/*
						Vector3 followPos = Cursor.transform.position;
						//if hits floor
						if(hit.transform.position.y==-11.0f){
							//	cc.MoveTowardPos(Cursor.transform);
							
							//cc.FollowMouse =true;
							StartCoroutine(cc.MoveTowards(followPos));
						}else{
							//if hits wall
							followPos.y = -11.0f;
							StartCoroutine(cc.MoveTowards(followPos));
						}
						print (followPos);
						*/
					}

					/*
					//use mouse skill 
					if(hit.transform.tag == "Enemy"){
						Target = hit.transform;
						cc.UseSkill(Target);
					}
*/
					//monster box
					if(hit.transform.gameObject.tag == "Box"){
						if(mb.CanCheckSequence){
							mb.CheckSequence(hit.transform.gameObject);
						}
					}

					if(hit.transform.gameObject.tag == "Treasures"){
						tb.Discover();
					}


					//zoom if zoomable
					if(hit.transform.gameObject.tag == "Zoomable"){
						
						print ("zoomable");
						zoomCam = hit.transform.parent.FindChild("ZoomCamera").gameObject;

						RotateSelf rotating = hit.transform.GetComponent<RotateSelf>();

						if(rotating!=null){
							rotating.enabled = false;
							hit.transform.rotation = Quaternion.identity;
						}

				//		Camera zoomCam2 = hit.transform.GetComponentInChildren<Camera>();

						zoomCam.SetActive(true);
					//	Camera.main.gameObject.SetActive(false);

/*
						Vector3 camPos = camTr.position;
						prevCamPos = camPos;
						
						
						camPos.x = hit.transform.position.x;
						camPos.y = hit.transform.position.y;
						
						
						camPos.z += 10.0f; // Zoom
						
						Camera.main.transform.position = Vector3.Lerp(camTr.position, camPos, Time.deltaTime * zoomSpeed);
*/
						Zoomed = true;
						cc.enabled=false;//can't move or attack
					}
				}else {
					//zoom out back
				//	Camera.main.transform.position = Vector3.Lerp(camTr.position, prevCamPos, Time.deltaTime * zoomSpeed);

					print(zoomCam==null);
					if(zoomCam!=null){
						zoomCam.SetActive(false);

					}

					Zoomed = false;
					cc.enabled = true;
				}
			//	print (Zoomed);


			}



		}
	}
}
