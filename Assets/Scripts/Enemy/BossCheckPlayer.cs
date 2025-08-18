using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� �÷��̾� Ȯ�� �ݶ��̴�
public class BossCheckPlayer : MonoBehaviour
{
	EnemyFSM fsm;

	public void Init(EnemyFSM parant)
	{
		fsm = parant;
	}

	// �÷��̾ ���������� ���´ٸ� Ÿ������ ����, �ش� Ÿ���� �������� ����
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
