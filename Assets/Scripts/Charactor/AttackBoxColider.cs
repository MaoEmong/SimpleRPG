using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾� ���� ����
public class AttackBoxColider : MonoBehaviour
{
	public enum AttackType
	{
		Normal,
		Skill1,

	}
	CharactorMovement MyPlayer;

	public AttackType Type;

	public void Init(CharactorMovement player)
	{
		MyPlayer = player;
	}

	private void OnTriggerEnter(Collider other)
	{
		// ���� �������� ��
		if(other.CompareTag("Enemy"))
		{
			EnemyFSM enemy = other.gameObject.GetComponent<EnemyFSM>();

			// ġ��Ÿ Ȯ��
			bool isCritical;
			float val = Random.Range(0.0f, 99.9f);

			// ������ ���
			int RealDamage = MyPlayer.MyState.Atk + (Managers.GData.playerinfo.UpgradeWeapon * 5);

			if (val <= MyPlayer.MyState.Critical)
			{
				isCritical = true;
				RealDamage = (int)(RealDamage * 1.5f);
			}
			else
			{
				isCritical = false;
			}
			// ���� Ÿ�ӿ� ���� ���������� ġ��Ÿ�� ����
			if (Type == AttackType.Normal)
			{
				enemy.CallHitEnemy(RealDamage, isCritical);
			}
			else if(Type == AttackType.Skill1)
			{
				RealDamage = RealDamage * 2;
				enemy.CallHitEnemy(RealDamage, isCritical);
			}


		}
	}

}
