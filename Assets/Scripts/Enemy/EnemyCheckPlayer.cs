using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//에너미에서 플레이어를 감지하기 위한 OnTrigger
public class EnemyCheckPlayer : MonoBehaviour
{
	EnemyFSM fsm = null;

	public void Init(EnemyFSM MyParant)
	{
		fsm = MyParant;
	}

	private void OnTriggerEnter(Collider other)
	{
		// 플레이어 감지
		if(other.CompareTag("Player"))
		{
			Debug.Log("Find Player");
			// 부모객체가 할당되어있을 떄
			if(fsm != null)
			{
				if (fsm.isComeback)
					return;

				// 부모객체의 타겟이 없다면
				if(fsm.Target == null)
					// 타겟 설정
					fsm.Target = other.gameObject.transform;
			}
		}
	}
	private void OnTriggerStay(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			if(fsm.Target == null)
			{
				fsm.Target = other.gameObject.transform;
			}
		}
	}
	private void OnTriggerExit(Collider other)
	{
		// 플레이어 감지
		if(other.CompareTag("Player"))
		{
			Debug.Log("Exit Player");
			// 부모객체가 할당되어 있을 떄
			if (fsm != null)
			{
				if(fsm.isComeback)
				{
					fsm.Target = null;
					return;
				}
				// 부모객체의 타겟이 남아있다면
				if (fsm.Target != null)
					// 타겟 비움
					fsm.Target = null;
			}
		}
	}


}
