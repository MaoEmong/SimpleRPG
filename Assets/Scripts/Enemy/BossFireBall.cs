using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireBall : MonoBehaviour
{
	Transform TargetPos;

	float endTime = 5.0f;

	float Damage;

	float Speed = 15.0f;

	Coroutine MoveCor;

	public ParticleSystem particle;

	public void Init(Transform Target, float damage)
	{
		TargetPos = Target;
		Damage = damage;

		transform.LookAt(TargetPos);

		MoveCor = StartCoroutine(MoveFireBall());
	}

	IEnumerator MoveFireBall()
	{
		yield return null;

		float curTime = 0.0f;
		Vector3 Dir = TargetPos.position - transform.position;
		Dir = Dir.normalized;


		while (curTime < endTime)
		{
			yield return null;

			curTime += Time.deltaTime;

			transform.position += Dir * Speed * Time.deltaTime;

		}
		Debug.Log("Delete FireBall1");
		Managers.Pool.Push(this.gameObject);
		MoveCor = null;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<CharactorMovement>().CallHitPlayer(Damage);
			Managers.Sound.Play("Effect/Enemy/FireBallHit");
			Debug.Log("Delete FireBall2");

			var obj = Managers.Pool.Pop(particle.gameObject);
			obj.transform.position = other.transform.position;

			StopCoroutine(MoveCor);
			MoveCor = null;
			Managers.Pool.Push(this.gameObject);


		}
	}



}
