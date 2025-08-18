using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 공격 범위
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
		// 적을 공격했을 때
		if(other.CompareTag("Enemy"))
		{
			EnemyFSM enemy = other.gameObject.GetComponent<EnemyFSM>();

			// 치명타 확인
			bool isCritical;
			float val = Random.Range(0.0f, 99.9f);

			// 데미지 계산
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
			// 공격 타임에 따른 데미지값과 치명타값 적용
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
