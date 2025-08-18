using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNeedleSnail : EnemyFSM
{

	[SerializeField]
	Material OriginMat;
	[SerializeField]
	Material HitMat;

	SkinnedMeshRenderer meshrender;


	protected override void Start()
	{
		base.Start();
		enemyType = Define.EnemyType.NeedleSnail;
		meshrender = GetComponentInChildren<SkinnedMeshRenderer>();
	}

	protected override void Update()
	{
		base.Update();


	}

	protected override void EnemyHit()
	{
		if (!isHit)
		{
			Managers.Sound.Play("Effect/Enemy/SnailHitVoice");
		}
		base.EnemyHit();
		if (isAttack || isDie)
			return;
		meshrender.material = HitMat;

		Managers.CallWaitForSeconds(0.1f, () => { meshrender.material = OriginMat; });


	}

	public void Init(Vector3 comeback, int level)
	{
		// �Է¹��� ������ ���� 0���� �۰ų� ���ٸ�(������ 1���� �����̱� ������ 1���� �������� ���� ���������ʹ� ������������)
		// ����Ʈ�� �ִ� ���� �������� �������� ��ȯ
		// �ƴ϶�� �ش� ���� ���� ��ȯ
		Debug.Log("NeedleSnail Init!");

		int lev = 0;


		if (level <= 0)
		{
			lev = Random.Range(1, Managers.GData.NeedleSnailBasicState.Count + 1);
		}
		else
			lev = level;

		if(lev > Managers.GData.NeedleSnailBasicState[Managers.GData.NeedleSnailBasicState.Count - 1].Level)
		{
			Debug.Log("Error Level is Over!");
			return;
		}

		Debug.Log("NeedleSnail Level Set!");

		EnemyState mystate = new EnemyState(Managers.GData.NeedleSnailBasicState[lev - 1]);

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

		Managers.Sound.Play("Effect/Enemy/SnailDie");
		Managers.GData.PlayerFeat[2].AddCount();


		int Val = Random.Range(0, 99);
		if (Val > 75)
		{
			var Cube = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Prefabs/Object/ItemCube"));
			Cube.transform.position = transform.position + new Vector3(0, 2, 0);
			Val = Random.Range(0, 99);
			if (Val < 25)
				Cube.GetComponent<ItemCube>().Init(4);
			else if (Val > 25)
				Cube.GetComponent<ItemCube>().Init(5);
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
