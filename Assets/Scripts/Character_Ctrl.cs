using UnityEngine;
using System.Collections;

public enum PlayerState{
	
	Wait,
	Attack,// == use skill
	Damage,
	Dead,
	Walk,
	
}

public interface Spec_Ctrl {

	void AnimationUpdate();
	IEnumerator Skill();
	IEnumerator Skill(Transform tr);
	IEnumerator Hurt();
	void UpdateInfo();

}

public class Character_Ctrl : MonoBehaviour {

	public PlayerState PS;
	public CharacterController controller;
	//public GameObject Cursor;
	
	//public Animation anim;
	public GameMananger gm;
	public Spec_Ctrl sc;

	//public bool FollowMouse = false;

	//public GameObject Bullet; //should come from the project folders if it's being created
	//public Transform ShotPoint;

	public float moveSpeed;
	public float jumpSpeed;
	public float gravity;
	private Vector3 moveDirection = Vector3.zero;

	public float max_HP = 3;
	public float curr_HP;

	public float skillRange = 1000f;

	public GameObject Character1; //Knight
	public GameObject Character2; //Troll
	public GameObject CurrentPlayer;

	public bool mouseMoving = false;

	//public Vector3 followPos;

	void Start (){
		controller = GetComponent<CharacterController>();
		//anim = GetComponent<Animation>();
		PS = PlayerState.Wait;
		curr_HP = max_HP;
		Character1.transform.position = this.transform.position;
		Character1.transform.rotation = this.transform.rotation;
		Character2.transform.position = this.transform.position - new Vector3(2f, 0f, 2f);
		Character2.transform.rotation = this.transform.rotation;

		CurrentPlayer=Character1;
		sc = CurrentPlayer.GetComponent<Spec_Ctrl>();
	}

	public IEnumerator MoveTowards(Vector3 followPos)
	{
		mouseMoving = true;
		PS = PlayerState.Walk;
		followPos.y = transform.position.y;
		float speed= .5f;
		float distance = Vector3.Distance (followPos, transform.position);
		float time = distance/speed;

		Vector3 direction = (followPos - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, .5f);

		moveDirection = followPos - transform.position;
		moveDirection = transform.TransformDirection(moveDirection);
		moveDirection *= .5f; //speed
		moveDirection.y -= gravity * Time.deltaTime;

		while (time > 0)
		{
			controller.Move(moveDirection * Time.deltaTime);

			time-=Time.deltaTime;
			yield return null;
		}
		yield return new WaitForSeconds(.15f);
		PS = PlayerState.Wait;
		mouseMoving = false;
	}
	
	void Move (){
		float xx = Input.GetAxisRaw ("Vertical");
		float zz = Input.GetAxisRaw ("Horizontal");

		if(PS!=PlayerState.Attack){

			transform.Rotate(0f, zz * 90f * Time.deltaTime, 0f);
			
			if(controller.isGrounded){
				moveDirection = Vector3.forward * xx;
				moveDirection = transform.TransformDirection(moveDirection);
				moveDirection *= moveSpeed;
				
				if (Input.GetButton("Jump")){
					moveDirection.y = jumpSpeed;
				}
			}

			moveDirection.y -= gravity * Time.deltaTime;
			controller.Move(moveDirection * Time.deltaTime);
			PS=PlayerState.Walk;

			if(xx==0&&zz==0 && PS!=PlayerState.Wait){
				PS = PlayerState.Wait;
			}

		}

		if(Input.GetKeyDown(KeyCode.A) && PS!=PlayerState.Dead){
			StartCoroutine(sc.Skill());
			//keyboard skill
		}

	}

	//use skill using mouse click
	public void UseSkill(Transform target){
		if(PS!=PlayerState.Dead){
			Vector3 followPos = target.position;
			followPos.y = transform.position.y;

			//TODO: fix rotation
			Vector3 direction = (followPos - transform.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, .1f);

			if(Vector3.Distance(this.transform.position, target.position) - skillRange <= 0){
				print (target.gameObject.name);
				StartCoroutine(sc.Skill(target));
			}else{
				MoveTowards(followPos);
				if(Vector3.Distance(this.transform.position, target.position) - skillRange <= 0){
					print (target.gameObject.name);
					StartCoroutine(sc.Skill(target));
				}
			}

		}
	}

	public void SetSkillRange(float newRange){
		skillRange = newRange;
	}

	public void HurtPlayer(){
		StartCoroutine(sc.Hurt ());
		//gm.DimImage("Life" + (curr_HP+1).ToString());
		if(curr_HP==2) {
			gm.DimImage(gm.Life3);
		}else if(curr_HP==1) {
			gm.DimImage(gm.Life2);
		}else if(curr_HP==0) {
			gm.DimImage(gm.Life1);
		}

	}

	void Update () {

		if(PS!=PlayerState.Dead && PS!=PlayerState.Damage){

			if(!mouseMoving){
				Move ();
			}

			sc.AnimationUpdate();

		}
	}

}
