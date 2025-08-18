using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ʹ� ���� �ݶ��̴�
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
		// �÷��̾� Ÿ�� ��
		if(other.CompareTag("Player"))
		{

			other.GetComponent<CharactorMovement>().CallHitPlayer(fsm.State.Atk);

		}

	}

}
