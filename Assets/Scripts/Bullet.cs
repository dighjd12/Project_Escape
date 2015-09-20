using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float Speed=100f;
	public float Damage=50f;//amount of damage of the bullet
	public float Life = 200f;//duration of the bullet
	public float rotSpeed =5f;

	void Update () {
		Life-=Time.deltaTime;
		if(Life<=0f){
			Destroy(this.gameObject);
		}

		//transform.Rotate(new Vector3(0f, rotSpeed, 0f));
		transform.Translate(Vector3.forward*Speed*Time.deltaTime);
	}

	void OnCollisionEnter(Collision collision){

		Destroy(this.gameObject);
		//maybe add an effect?

		if(collision.gameObject.tag=="Enemy"){

			Enemy enemy = collision.gameObject.GetComponent<Enemy>();

			if(enemy.ES!=EnemyState.Dead){
				enemy.HurtEnemy(Damage);
			}

		}
	}
}
