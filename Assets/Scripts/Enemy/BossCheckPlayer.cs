using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 보스 전용 플레이어 확인 콜라이더
public class BossCheckPlayer : MonoBehaviour
{
	EnemyFSM fsm;

	public void Init(EnemyFSM parant)
	{
		fsm = parant;
	}

	// 플레이어가 범위안으로 들어온다면 타겟으로 설정, 해당 타겟은 해제되지 않음
	private void OnTriggerEnter(Collider other)
	{
		if (fsm.Target != null)
			return;

		if(other.CompareTag("Player"))
		{
			fsm.Target = other.transform;
		}
	}

}
