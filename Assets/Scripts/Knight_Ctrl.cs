using UnityEngine;
using System.Collections;

public class Knight_Ctrl : MonoBehaviour, Spec_Ctrl {

	public Character_Ctrl cc;
	public Animation anim;

	public GameObject Bullet;
	public Transform ShotPoint;

	float skillRange = 500f;

	float ccHeight = 80f;
	float ccRad = 20f;
	float ccCenterY = 36f;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation>();
		UpdateInfo();
		//cc = GetComponent<Character_Ctrl>();
	}

	public void AnimationUpdate(){
		//print (PS);

		if(cc.PS==PlayerState.Wait){
			anim.CrossFade("Wait", 0.2f);
		}
		else if(cc.PS==PlayerState.Walk){
			anim.CrossFade("Walk", 0.2f);
		}
		else if(cc.PS==PlayerState.Attack){
			anim.CrossFade("Attack", 0.2f);
		}
	}

	public void UpdateInfo(){
		cc.SetSkillRange(skillRange);
		cc.sc = this;
		cc.controller.height = ccHeight;
		cc.controller.radius = ccRad;

		Vector3 tempCenter = cc.controller.center;
		tempCenter.y = ccCenterY;
		cc.controller.center = tempCenter;
	}


	public IEnumerator Skill(){
	
		GameObject bullet = Instantiate(Bullet, ShotPoint.position, Quaternion.LookRotation(ShotPoint.up)) as GameObject;

		//	aud.clip = ShotSound;
		//	aud.Play();
		Physics.IgnoreCollision(bullet.GetComponent<Collider>(), this.GetComponentInParent<Collider>());
		
		cc.PS = PlayerState.Attack;
		
		yield return new WaitForSeconds(0.15f);
		cc.PS = PlayerState.Wait;
	}

	public IEnumerator Skill(Transform tr){
		//walk up to a distance and attack
		print ("bullet made");

		Instantiate(Bullet, ShotPoint.position, Quaternion.LookRotation(ShotPoint.up));
		
		//	aud.clip = ShotSound;
		//	aud.Play();
		
		cc.PS = PlayerState.Attack;
		
		yield return new WaitForSeconds(0.15f);
		cc.PS = PlayerState.Wait;
	}

	public IEnumerator Hurt(){
		if(cc.curr_HP>0){
			cc.curr_HP-=1f;
			cc.PS = PlayerState.Damage;
			anim.CrossFade("Damage", 0.2f);
			
			yield return new WaitForSeconds(0.2f);
			cc.PS = PlayerState.Wait;
			//update UI
		}
		if(cc.curr_HP<=0){
			cc.PS = PlayerState.Dead;
			anim.CrossFade("Dead", 0.2f);
		}
	}

}
