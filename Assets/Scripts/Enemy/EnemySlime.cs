using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EnemyFSM을 상속받는 Slime
public class EnemySlime : EnemyFSM
{
    // 일반상태를 마테리얼
    [SerializeField]
    Material OriginMat;
    // 공격 받았을때 마테리얼
    [SerializeField]
    Material HitMat;
    // 마테리얼 교체용 Renderer
    SkinnedMeshRenderer meshrender;


    protected override void Start()
    {
        base.Start();
        enemyType = Define.EnemyType.Slime;
        meshrender = GetComponentInChildren<SkinnedMeshRenderer>();
	}

	protected override void Update()
    {
        base.Update();


	}

    // 공격 받았을 때
    protected override void EnemyHit()
    {
        // 사운드 출력
        if (!isHit)
        {
            Managers.Sound.Play("Effect/Enemy/SlimeHitVoice");
		}
		base.EnemyHit();
        // 공격중이거나 죽은상태가 아니라면
		if (isAttack || isDie)
			return;
        // 히트 마테리얼로 교체
        meshrender.material = HitMat;
        // 이후 일반상태 마테리얼로 다시 교체
        Managers.CallWaitForSeconds(0.1f, () => { meshrender.material = OriginMat; });
        

	}

    // 몬스터 초기화
	public void Init(Vector3 comeback, int level)
    {
        // 몬스터의 레벨값 설정
        int lev = 0;
        if (level <= 0)
        {
            lev = Random.Range(1, Managers.GData.SlimeBasicState.Count+1);
        }
        else
            lev = level;

		// 몬스터의 스텟 데이터를 기반으로 레벨 값에 맞는 스테이터스 데이터를 가짐
		EnemyState mystate = new EnemyState(Managers.GData.SlimeBasicState[lev - 1]);

        base.Init(comeback, mystate);

	}
    // 몬스터 사망
    protected override void EnemyDie()
    {

		// Die액션 1회 재생을 위해 현재 죽었는 지 확인 후 죽지 않았다면
		// 죽음 처리 후 애니메이션 재생
		if (isDie)
			return;

        // 사망 사운드
        Managers.Sound.Play("Effect/Enemy/SlimeDie");

        // 플레이어의 업적에 해당 몬스터의 킬 카운트 추가
        Managers.GData.PlayerFeat[0].AddCount();

		// 사망 이외 전부 false
		isDie = true;
		isHit = false;
		isAttack = false;

        // 랜덤하게 아이템 드랍
        int Val = Random.Range(0, 99);
        if(Val > 50)
        {
			var Cube = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Prefabs/Object/ItemCube"));
			Cube.transform.position = transform.position + new Vector3(0, 2, 0);
            Val = Random.Range(0, 99);
            if(Val < 25)
    			Cube.GetComponent<ItemCube>().Init(0);
            else if(Val > 25)
				Cube.GetComponent<ItemCube>().Init(1);
            else
				Cube.GetComponent<ItemCube>().Init(7);

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
