using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 에너미 어택 콜라이더
public class EnemyAttackCollider : MonoBehaviour
{
	[SerializeField]
	EnemyFSM fsm;

	private void Start()
	{
		fsm = GetComponentInParent<EnemyFSM>();

	}

	private void OnTriggerEnter(Collider other)
	{
		// 플레이어 타격 시
		if(other.CompareTag("Player"))
		{

			other.GetComponent<CharactorMovement>().CallHitPlayer(fsm.State.Atk);

		}

	}

}
