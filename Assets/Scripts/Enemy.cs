using UnityEngine;
using System.Collections;

public enum EnemyState{

	Wait,
	Attack,
	Damage,
	Dead,
	Walk,
}

public class Enemy : MonoBehaviour {

	public EnemyState ES;
	public Animation anim;

	float Speed = 0f;

	public float MoveSpeed;
	public float AttackSpeed;
	public float AttackDelay = 100f;

	public float DiscoverRange;
	public float AttackRange;
	public Transform Player;
	public Character_Ctrl PlayerScript;

	public float max_HP = 100f;
	public float curr_HP;

	public float fadeTime = 6f;

	public AudioSource attackAud;

	void Start(){
		anim = this.GetComponent<Animation>();
		curr_HP = max_HP;
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		Player = player.transform;
		PlayerScript = player.GetComponent<Character_Ctrl>();
	}


	void DistanceCheck(){

		if(Vector3.Distance(Player.position,transform.position)>=DiscoverRange){

			ES = EnemyState.Wait;
			Speed = 0f;
		}else if(Vector3.Distance(Player.position,transform.position)<AttackRange){

			if(PlayerScript.PS!=PlayerState.Dead && ES != EnemyState.Damage){
				transform.rotation = Quaternion.LookRotation(
					new Vector3(Player.position.x,this.transform.position.y,Player.position.z)
					-transform.position);

				if(AttackDelay>=100f){
					StartCoroutine("Attack");
					AttackDelay=0f;
				}
				AttackDelay++;

			}else{
				ES = EnemyState.Wait;
			}
		}else{
			if(PlayerScript.PS!=PlayerState.Dead){
				ES = EnemyState.Walk;
				Speed = MoveSpeed;
				transform.rotation = Quaternion.LookRotation(
					new Vector3(Player.position.x,this.transform.position.y,Player.position.z)
					-transform.position);
				
				transform.Translate(Vector3.forward * Speed * Time.deltaTime);
			}else{
				ES=EnemyState.Wait;
			}


		}
	}

	public IEnumerator Attack(){
		Speed=0;
		ES=EnemyState.Attack;
		PlayerScript.HurtPlayer();
		yield return new WaitForSeconds(.5f);
		ES=EnemyState.Wait;
	}

	public void HurtEnemy(float Damage){
		StartCoroutine(Hurt (Damage));
	}
	
	public IEnumerator Hurt(float damage){
		if(curr_HP>0){
			curr_HP-=damage;
			ES = EnemyState.Damage;
			anim.CrossFade("Damage", 0.2f);
			yield return new WaitForSeconds(0.2f);
			ES = EnemyState.Wait;
			//update UI
		}
		if(curr_HP<=0){
			Die ();
		}
	}

	public void Die(){
		ES = EnemyState.Dead;
		anim.CrossFade("Dead", 0.2f);
		//fade out and destroy?
		StartCoroutine("FadeOut");

	}


	//TODO: generalize to all renderers
	IEnumerator FadeOut(){
	
		Material mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
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

	void AnimationUpdate (){
		if(ES==EnemyState.Wait){
			anim.CrossFade("Wait", 0.2f);
		}
		else if(ES==EnemyState.Walk){
			anim.CrossFade("Walk", 0.2f);
		}
		else if(ES==EnemyState.Attack){
			anim.CrossFade("Attack", 0.2f);
			attackAud.Play();
		}
	}


	void Update(){

		if(ES!=EnemyState.Dead && ES!=EnemyState.Attack && ES!=EnemyState.Damage){
			DistanceCheck();
			AnimationUpdate();
		}

	}

}
