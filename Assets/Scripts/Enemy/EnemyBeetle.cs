using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeetle : EnemyFSM
{
	[SerializeField]
	Material[] LevelOfMaterials;
	[SerializeField]
	Material HitMat;

	SkinnedMeshRenderer meshrender;

	int indexVal = 0;

	protected override void Start()
	{
		base.Start();
		enemyType = Define.EnemyType.Beetle;
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
			Managers.Sound.Play("Effect/Enemy/BeetleHitVoice");
		}
		base.EnemyHit();
		if (isAttack || isDie)
			return;
		meshrender.material = HitMat;

		Managers.CallWaitForSeconds(0.1f, () => { meshrender.material = LevelOfMaterials[indexVal]; });


	}

	public void Init(Vector3 comeback, int level)
	{
		// �Է¹��� ������ ���� 0���� �۰ų� ���ٸ�(������ 1���� �����̱� ������ 1���� �������� ���� ���������ʹ� ������������)
		// ����Ʈ�� �ִ� ���� �������� �������� ��ȯ
		// �ƴ϶�� �ش� ���� ���� ��ȯ
		int lev = 0;

		if (level <= 0)
		{
			lev = Random.Range(1, Managers.GData.BeetleBasicState.Count + 1);
		}
		else
			lev = level;

		if (lev > Managers.GData.BeetleBasicState[Managers.GData.BeetleBasicState.Count - 1].Level)
		{
			Debug.Log("Error Level is Over!");
			return;
		}

		if (lev <= 2)
		{
			indexVal = 0;
		}
		else if(lev <= 4)
		{
			indexVal = 1;
		}
		else
		{
			indexVal = 2;
		}

		meshrender = GetComponentInChildren<SkinnedMeshRenderer>();
		meshrender.material = LevelOfMaterials[indexVal];

		Debug.Log("NeedleSnail Level Set!");

		EnemyState mystate = new EnemyState(Managers.GData.BeetleBasicState[lev - 1]);

		base.Init(comeback, mystate);

	}

	protected override void EnemyDie()
	{

		// Die�׼� 1ȸ ����� ���� ���� �׾��� �� Ȯ�� �� ���� �ʾҴٸ�
		// ���� ó�� �� �ִϸ��̼� ���
		if (isDie)
			return;

		// ��� �̿� ���� false
		isDie = true;
		isHit = false;
		isAttack = false;

		Managers.Sound.Play("Effect/Enemy/BeetleDie");
		Managers.GData.PlayerFeat[4].AddCount();

		int Val = Random.Range(0, 99);
		if (Val > 75)
		{
			var Cube = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Prefabs/Object/ItemCube"));
			Cube.transform.position = transform.position + new Vector3(0, 2, 0);
			Val = Random.Range(0, 99);
			if (Val < 25)
				Cube.GetComponent<ItemCube>().Init(1);
			else if (Val > 25)
				Cube.GetComponent<ItemCube>().Init(9);
			else
				Cube.GetComponent<ItemCube>().Init(8);

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
