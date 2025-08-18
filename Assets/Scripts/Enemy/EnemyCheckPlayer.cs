using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���ʹ̿��� �÷��̾ �����ϱ� ���� OnTrigger
public class EnemyCheckPlayer : MonoBehaviour
{
	EnemyFSM fsm = null;

	public void Init(EnemyFSM MyParant)
	{
		fsm = MyParant;
	}

	private void OnTriggerEnter(Collider other)
	{
		// �÷��̾� ����
		if(other.CompareTag("Player"))
		{
			Debug.Log("Find Player");
			// �θ�ü�� �Ҵ�Ǿ����� ��
			if(fsm != null)
			{
				if (fsm.isComeback)
					return;

				// �θ�ü�� Ÿ���� ���ٸ�
				if(fsm.Target == null)
					// Ÿ�� ����
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
		// �÷��̾� ����
		if(other.CompareTag("Player"))
		{
			Debug.Log("Exit Player");
			// �θ�ü�� �Ҵ�Ǿ� ���� ��
			if (fsm != null)
			{
				if(fsm.isComeback)
				{
					fsm.Target = null;
					return;
				}
				// �θ�ü�� Ÿ���� �����ִٸ�
				if (fsm.Target != null)
					// Ÿ�� ���
					fsm.Target = null;
			}
		}
	}


}
