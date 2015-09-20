using UnityEngine;
using System.Collections;

public class Troll_Ctrl : MonoBehaviour, Spec_Ctrl {

	public Character_Ctrl cc;
	public Animation anim;
	string moveAnimation = "Run";
	bool Carrying = false;
	public bool carryingKey = false;
	GameObject carriedObject;
	GameObject lastCarriedObject;

	public AudioSource stepAud;
	public AudioSource skillAud;

	public float skillRange = 80f;
	public GameMananger gm;

	float ccHeight = 80f;
	float ccRad = 30f;
	float ccCenterY = 36f;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation>();
		Carrying = false;
		carriedObject = null;
		lastCarriedObject = null;
		//mainCamera = GameObject.FindWithTag("TrollCam");
		//cc = GetComponent<Character_Ctrl>();
	}
	
	public void AnimationUpdate(){
		if(cc.PS==PlayerState.Wait){
			anim.CrossFade("Idle_01", 0.2f);
			stepAud.Stop();
		}
		else if(cc.PS==PlayerState.Walk){
			anim.CrossFade(moveAnimation, 0.2f);
			if(!stepAud.isPlaying){
				stepAud.Play();
			}
		}else if(cc.PS==PlayerState.Attack){
			anim.CrossFade("Attack_02", 0.2f);
			skillAud.Play ();
		}
	}
	
	public IEnumerator Skill(){

		if(Carrying){
			//drop
			carriedObject.transform.localPosition = new Vector3(0.1f, 0f, 0.1f);
			/*
			if(carriedObject.GetComponent<Collider>()!=null){
				carriedObject.GetComponent<Collider>().enabled = true;
				carriedObject.GetComponent<Rigidbody>().useGravity = true;
				Physics.IgnoreCollision(this.GetComponentInParent<Collider>(), carriedObject.GetComponent<Collider>());
			}
*/
			lastCarriedObject = carriedObject;
			carriedObject.transform.parent = null;
			moveAnimation = "Run";
			Carrying = false;
			carriedObject = null;
			if(carryingKey){
				carryingKey=false;
			}
		}else if(!Carrying){

			//pickup
			GameObject target = FindClosestCarryable();
			if(target!=null){
				target.transform.parent = this.transform;
				if(target.name.Substring(0,3) == "key"){
					carryingKey=true;
				}
				
				if(target.GetComponent<Collider>()!=null){
					//move out of the object
					Mesh mesh = target.GetComponentInChildren<MeshFilter>().mesh;
					float width = mesh.bounds.size.z/2;
					float depth = mesh.bounds.size.x/2;
					
					Vector3 temp = cc.transform.localPosition;
					cc.transform.localPosition = temp - new Vector3(depth, 0f, width);
				}
/*
 * 
 * 
				if(target.GetComponent<Collider>()!=null){
					target.GetComponent<Collider>().enabled = false;
					target.GetComponent<Rigidbody>().useGravity = false;
				}
*/
				target.transform.localPosition = new Vector3(0.1f, 0.1f, 0.1f);
				moveAnimation = "Walk";
				print (target.gameObject.name);
				Carrying = true;
				carriedObject = target;
			}else{
				//if target==null
				gm.GiveInstruction(2f, "No Carryable Objects within range");
			}

		}

		anim.CrossFade("Attack_02", 0.2f);
		cc.PS = PlayerState.Attack;

		yield return new WaitForSeconds(0.2f);

		if(lastCarriedObject!=null && lastCarriedObject.GetComponent<Collider>()!=null && !Carrying){
			//move out of the object
			Mesh mesh = lastCarriedObject.GetComponentInChildren<MeshFilter>().mesh;
			float width = mesh.bounds.size.z/2;
			float depth = mesh.bounds.size.x/2;

			Vector3 temp = cc.transform.localPosition;
			cc.transform.localPosition = temp - new Vector3(depth, 0f, width);
			//z front, x sides, y updown

		//	Physics.IgnoreCollision(this.GetComponentInParent<Collider>(), lastCarriedObject.GetComponent<Collider>(), false);
		}

		cc.PS = PlayerState.Wait;
	}

	public IEnumerator Skill(Transform tr){
		//walk up and hold the object 
		yield return null;
	}

	//Can return null!!!!
	GameObject FindClosestCarryable(){

		GameObject[] objs = GameObject.FindGameObjectsWithTag("Carryable");
		GameObject closest = null;
		foreach (GameObject obj in objs){

			Vector3 ObjPos = obj.transform.position;
			Vector3 ObjXZPos = new Vector3(ObjPos.x, transform.position.y, ObjPos.z);

			if(Vector3.Distance(transform.position, ObjXZPos)<skillRange){
				//if the distance is within range
				print (obj.name);

				if(closest==null){
					closest = obj;
					Debug.Log (Vector3.Distance(transform.position, ObjXZPos) + " " + closest.name);
					continue;
				}

				if(Vector3.Distance(transform.position, ObjXZPos) 
				   <= Vector3.Distance(transform.position, closest.transform.position)){
					closest = obj;
					
				}
				Debug.Log (Vector3.Distance(transform.position, ObjXZPos) + " " + closest.name);
			}

		}
		return closest;

	}

	public void UpdateInfo(){
		//update skill range
		cc.SetSkillRange(skillRange);
		cc.sc = this;
		cc.controller.height = ccHeight;
		cc.controller.radius = ccRad;

		Vector3 tempCenter = cc.controller.center;
		tempCenter.y = ccCenterY;
		cc.controller.center = tempCenter;


	}

	/*
	void PickUp(){

		Vector3 pos = this.transform.position;
		RaycastHit ray;
		Vector3 target = pos + Camera.main.ScreenPointToRay();

		int x = Screen.width/2;
		int y = Screen.height/2;

	//	Ray ray = Camera.main.scr
	}*/
	
	public IEnumerator Hurt(){
		if(cc.curr_HP>0){
			cc.curr_HP-=1f;
			cc.PS = PlayerState.Damage;
			anim.CrossFade("Hit", 0.2f);
			
			yield return new WaitForSeconds(0.2f);
			cc.PS = PlayerState.Wait;
			//update UI
		}
		if(cc.curr_HP<=0){
			//TODO: die keeps repeating
			cc.PS = PlayerState.Dead;
			anim.CrossFade("Die", 0.2f);
		}
	}
}
