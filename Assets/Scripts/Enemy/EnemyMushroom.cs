using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMushroom : EnemyFSM
{
	[SerializeField]
	Material OriginMat;
	[SerializeField]
	Material HitMat;

	SkinnedMeshRenderer meshrender;


	protected override void Start()
	{
		base.Start();
		enemyType = Define.EnemyType.Mushroom;
		meshrender = GetComponentInChildren<SkinnedMeshRenderer>();
	}

	protected override void Update()
	{
		base.Update();


	}

	protected override void EnemyHit()
	{
		if(!isHit)
		{
			Managers.Sound.Play("Effect/Enemy/MushRoomHitVoice");
		}
		base.EnemyHit();
		if (isAttack || isDie)
			return;
		meshrender.material = HitMat;

		Managers.CallWaitForSeconds(0.1f, () => { meshrender.material = OriginMat; });


	}

	public void Init(Vector3 comeback, int level)
	{
		int lev = 0;
		if (level <= 0)
		{
			lev = Random.Range(1, Managers.GData.MushroomBasicState.Count + 1);
		}
		else
			lev = level;
		EnemyState mystate = new EnemyState(Managers.GData.MushroomBasicState[lev - 1]);

		base.Init(comeback, mystate);

	}
	protected override void EnemyDie()
	{

		// Die�׼� 1ȸ ����� ���� ���� �׾��� �� Ȯ�� �� ���� �ʾҴٸ�
		// ���� ó�� �� �ִϸ��̼� ���
		if (isDie)
			return;

		Managers.Sound.Play("Effect/Enemy/MushRoomDie");
		Managers.GData.PlayerFeat[1].AddCount();

		// ��� �̿� ���� false
		isDie = true;
		isHit = false;
		isAttack = false;

		int Val = Random.Range(0, 99);
		if (Val > 75)
		{
			var Cube = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Prefabs/Object/ItemCube"));
			Cube.transform.position = transform.position + new Vector3(0, 2, 0);
			Val = Random.Range(0, 99);
			if (Val < 25)
				Cube.GetComponent<ItemCube>().Init(2);
			else if (Val > 25)
				Cube.GetComponent<ItemCube>().Init(3);
			else
				Cube.GetComponent<ItemCube>().Init(7);

		}

		attackCollider.gameObject.SetActive(false);

		// Ÿ��(�÷��̾�)����ġ �߰�
		Target.GetComponent<CharactorMovement>().AddExp(state.EXP);

		// ������� �ڷ�ƾ �ִٸ� ���߰�
		if (CurCoroutine != null)
			StopCoroutine(CurCoroutine);
		// ��� �� �ڷ�ƾ ���
		StartCoroutine(DieAfter());
	}

	public override void CallHitEnemy(int Damage, bool critical)
	{
		base.CallHitEnemy(Damage, critical);
		Managers.Sound.Play("Effect/Enemy/SlimeHit");

	}

}
