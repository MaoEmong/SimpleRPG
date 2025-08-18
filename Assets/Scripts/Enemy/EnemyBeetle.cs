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
		// 입력받은 레벨의 값이 0보다 작거나 같다면(레벨은 1부터 시작이기 떄문에 1보다 작은수의 몬스터 레벨데이터는 존재하지않음)
		// 리스트에 있는 레벨 데이터중 무작위로 소환
		// 아니라면 해당 레벨 몬스터 소환
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

		// Die액션 1회 재생을 위해 현재 죽었는 지 확인 후 죽지 않았다면
		// 죽음 처리 후 애니메이션 재생
		if (isDie)
			return;

		// 사망 이외 전부 false
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

		// 타겟(플레이어)경험치 추가
		Target.GetComponent<CharactorMovement>().AddExp(state.EXP);

		// 재생중인 코루틴 있다면 멈추고
		if (CurCoroutine != null)
			StopCoroutine(CurCoroutine);
		// 사망 후 코루틴 재생
		StartCoroutine(DieAfter());
	}

	public override void CallHitEnemy(int Damage, bool critical)
	{
		base.CallHitEnemy(Damage, critical);
		Managers.Sound.Play("Effect/Enemy/SlimeHit");

	}

}
