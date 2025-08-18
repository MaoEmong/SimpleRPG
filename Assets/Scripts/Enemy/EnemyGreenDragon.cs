using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

// 보스몬스터
// EnemyFSM을 상속받으나 많은 부분을 수정함
public class EnemyGreenDragon : EnemyFSM
{
	// 히트 표시용 마테리얼
	[SerializeField]
	Material OriginMat;
	[SerializeField]
	Material HitMat;
	// 마테리얼 교체를 위한 메시렌더러
	SkinnedMeshRenderer meshrender;

	// 보스몬스터 전용 캔버스
	public Canvas BossCanvas;
	public Image BossHpbar;
	public Text BossNameText;
	public Text BossLevelText;
	public Text HpText;

	// 플레이어 탐지 범위
	BossCheckPlayer BossChecker;
	// 플레이어 탐지 확인
	bool isFind = false;
	bool endScreamAction = false;

	// 보스몬스터의 공격 콜라이더는 2개 존재함
	[SerializeField]
	EnemyAttackCollider[] BossAttackCollider;

	// 파이어볼 발사 위치
	public Transform FireBallPos;

	// 시네마틱 카메라 설정
	public PlayableDirector BossDirector;
	public GameObject BossCameras;

	// 최초시작 시 
	protected override void Start()
	{
		anim = GetComponent<Animator>();
		// 보스의 플레이어 탐지 범위 스크립트
		BossChecker = GetComponentInChildren<BossCheckPlayer>();
		BossChecker.Init(this);
		// 보스의 공격 콜라이더
		BossAttackCollider = GetComponentsInChildren<EnemyAttackCollider>();
		foreach (var n in BossAttackCollider)
			n.gameObject.SetActive(false);

		isAttack = false;
		isDie = false;
		isFind = false;
		enemyType = Define.EnemyType.GreenDragon;
		meshrender = GetComponentInChildren<SkinnedMeshRenderer>();

		curDelay = AttackDelay;
		Init();
		 
		BossCanvas.gameObject.SetActive(true);
		BossNameText.text = State.Name;
		BossLevelText.text = $"Lv.{State.Level}";
		BossHpbar.fillAmount = (float)State.Hp / State.MaxHp;
		HpText.text = $"{State.Hp} / {State.MaxHp}";
		BossCanvas.gameObject.SetActive(false);


		BossDirector.gameObject.SetActive(false);
		BossCameras.SetActive(false);

	}

	protected override void Update()
	{
		// 사망 여부 확인
		CheckDieEnemy();
		// 플레이어와의 거리 확인
		CheckTargetDistance();
		// 애니메이션 재생
		EnemyMovement();
		// 캔버스 수치 변경
		refreshCanvas();
	}

	// 보스 상태 UI 최신화
	void refreshCanvas()
	{
		BossHpbar.fillAmount = (float)State.Hp / State.MaxHp;
		HpText.text = $"{State.Hp} / {State.MaxHp}";

	}

	protected override void EnemyMovement()
	{

		switch (CurState)
		{
			case Define.CharacterState.Idle:
				EnemyIdle();
				break;

			case Define.CharacterState.Move:
				EnemyMove();
				break;
//=============================================================
			case Define.CharacterState.Attack1:
				EnemyBasicAttack();
				break;
			case Define.CharacterState.Attack2:
				EnemyTailAttack();
				break;
			case Define.CharacterState.Attack3:
				EnemyFireBall();
				break;
//============================================================
			case Define.CharacterState.Hit:
				EnemyHit();
				break;

			case Define.CharacterState.Die:
				EnemyDie();
				break;
		}

	}

	protected override void EnemyIdle()
	{

		if (isAttack || isDie)
			return;

		if (isHit)
			return;

		// 타겟이 있을 때
		if (Target != null)
		{
			// 플레이어를 찾음 최초 1회
			if(!isFind)
			{
				// 울부짖음
				isFind = true;
				anim.SetTrigger("Scream");
				// 울부짖음 애니메이션 체크
				StartCoroutine(CheckScreamAction());
				Managers.Sound.BgmStop();
				return;
			}
			// 스크림 애니메이션이 끝나지 않았다면 return
			if (!endScreamAction)
				return;

			// 딜레이가 준비 되었고 현재 공격중이 아닐 때
			if (curDelay > AttackDelay && !isAttack)
			{
				// 플레이어가 사거리 안에 없다면
				if (AttackDistance < TargetDistance)
				{
					float val = Random.Range(0.0f, 99.0f);


					if (val < 50.0f)
					{
						// 이동으로 전환
						AnimationState("Move");
						return;

					}
					else
					{
						AnimationState("FireBall");
						return;
					}


				}
				// 플레이어가 사거리 안에 존재한다면
				else
				{
					// 공격 상태로 전환
					int val = Random.Range(0, 2);

					switch (val)
					{
						case 0:
							AnimationState("BasicAttack");
							break;
						case 1:
							AnimationState("TailAttack");
							break;
					}
					return;
				}	
			}
			// 공격중이거나 아직 딜레이가 모자랄 때
			else
			{
				// 딜레이 기다리면서 대기
				curDelay += Time.deltaTime;
				Vector3 Dir = Target.position - transform.position;
				Dir = Dir.normalized;
				transform.forward = Vector3.Lerp(transform.forward, Dir, Time.deltaTime * moveSpeed * 3.0f);
				AnimationState("Idle");
				return;
			}

		}
	}

	protected override void EnemyMove()
	{

		if (isAttack || isDie)
			return;

		if (isHit)
			return;

		// 에너미와 타겟 사이 거리 갱신
		CheckTargetDistance();
		Vector3 Dir;

		// 이동값 설정
		Dir = Target.transform.position - transform.position;
		Dir = Dir.normalized;

		transform.position += Dir * moveSpeed * Time.deltaTime;
		transform.forward = Vector3.Lerp(transform.forward, Dir, Time.deltaTime * moveSpeed * 3.0f);
		// 공격사거리안으로 타겟이 들어왔다면
		if (AttackDistance > TargetDistance)
		{
			// 이동을 멈추고 대기상태로 전환 -> 이후 대기상태에서 검사 후 공격상태로 전환
			AnimationState("Idle");
			return;
		}
		



	}
	// 보스 울부짖음 애니메이션 처리
	IEnumerator CheckScreamAction()
	{
		// 카메라 흔들기 액션을 위한 CameraMovement
		CameraMovement cameramove = Camera.main.GetComponent<CameraMovement>();

		Managers.CallWaitForSeconds(0.15f, () => { Managers.Sound.Play("Effect/Enemy/DragonScream"); });
		Managers.CallWaitForSeconds(0.3f, () => { Managers.Sound.Play("BGM/StartBossBattle", Define.Sound.Bgm); });

		while (true)
		{
			yield return null;

			if(anim.GetCurrentAnimatorStateInfo(0).IsName("Scream"))
			{
				if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f)
				{
					cameramove.isshake = true;
				}

				if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.90f)
				{
					endScreamAction = true;

					cameramove.isshake = false;

					// 보스전 캔버스 ActiveTrue
					BossCanvas.gameObject.SetActive(true);
					break;
				}
			}

			// 만약 스크림중 보스가 사망했다면
			// 카메라 흔들기 액션 취소
			if(isDie)
			{
				cameramove.isshake = false;
				break;
			}

		}
	}

	// 보스 깨물기 공격
	void EnemyBasicAttack()
	{
		if (isDie || isAttack)
			return;
		Debug.Log("Call Basic Attack");

		// 공격중 상태로 전환
		isAttack = true;
		// 공격애니메이션 체크 실행
		CurCoroutine = StartCoroutine(BasicAttackCoroutine());

	}
	IEnumerator BasicAttackCoroutine()
	{
		yield return null;
		Managers.Sound.Play("Effect/Enemy/DragonAttack1");

		while (true)
		{
			yield return null;
			// 현재 재생중인 애니메이션의 정보를 받아와 액션이 끝났는지 확인
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("BasicAttack"))
			{
				if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f)
				{
					BossAttackCollider[1].gameObject.SetActive(true);

				}
				if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.90f)
				{
					BossAttackCollider[1].gameObject.SetActive(false);

					break;
				}
			}
		}
		yield return new WaitForSeconds(0.3f);
		isAttack = false;
		// 애니메이션이 끝났다면 딜레이 초기화, 대기액션 
		curDelay = 0.0f;
		AnimationState("Idle");
	}

	// 보스 꼬리 공격
	void EnemyTailAttack()
	{
		if (isDie || isAttack)
			return;
		Debug.Log("Call Tail Attack");

		// 공격중 상태로 전환
		isAttack = true;
		BossAttackCollider[0].gameObject.SetActive(true);
		// 공격애니메이션 체크 실행
		CurCoroutine = StartCoroutine(TailAttackCoroutine());


	}
	IEnumerator TailAttackCoroutine()
	{
		yield return null;
		Managers.CallWaitForSeconds(0.1f, () => { Managers.Sound.Play("Effect/Enemy/DragonAttack2"); });

		while (true)
		{
			yield return null;
			// 현재 재생중인 애니메이션의 정보를 받아와 액션이 끝났는지 확인
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("TailAttack"))
			{
				if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.90f)
				{
					BossAttackCollider[0].gameObject.SetActive(false);

					break;
				}
			}
		}
		yield return new WaitForSeconds(0.5f);
		isAttack = false;
		// 애니메이션이 끝났다면 딜레이 초기화, 대기액션 
		curDelay = 0.0f;
		AnimationState("Idle");
	}

	void EnemyFireBall()
	{
		if (isDie || isAttack)
			return;
		Debug.Log("FireBall Attack");

		// 공격중 상태로 전환
		isAttack = true;
		// 공격애니메이션 체크 실행
		CurCoroutine = StartCoroutine(SpawnFireBall());

	}
	IEnumerator SpawnFireBall()
	{
		yield return null;
		Managers.CallWaitForSeconds(0.1f, () => { Managers.Sound.Play("Effect/Enemy/DragonAttack3"); });

		bool isSpawn = false;

		while (true)
		{
			yield return null;
			// 현재 재생중인 애니메이션의 정보를 받아와 액션이 끝났는지 확인
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("FireBall"))
			{

				if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.45f && !isSpawn)
				{
					Debug.Log("Spawn FireBall!");
					isSpawn = true;
					GameObject obj = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Prefabs/Enemy/BossFireBall"));
					obj.transform.position = FireBallPos.position;
					obj.GetComponentInParent<BossFireBall>().Init(Target.transform, state.Atk);
				}

				if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.90f)
				{
					break;
				}
			}
		}
		Debug.Log("EndFire!");
		yield return new WaitForSeconds(0.5f);
		isAttack = false;
		// 애니메이션이 끝났다면 딜레이 초기화, 대기액션 
		curDelay = 0.0f;
		AnimationState("Idle");

		Debug.Log("!!!EndFire!");
	}

	// 보스몬스터 애니메이션
	public override void AnimationState(string name)
	{
		switch (name)
		{
			case "Idle":
				CurState = Define.CharacterState.Idle;
				anim.SetBool("Move", false);
				anim.SetBool("Hit", false);
				anim.SetBool("Die", false);
				anim.SetBool("BasicAttack", false);
				anim.SetBool("TailAttack", false);
				anim.SetBool("FireBall", false);
				break;
			case "Move":
				CurState = Define.CharacterState.Move;
				anim.SetBool("Move", true);
				anim.SetBool("Hit", false);
				anim.SetBool("Die", false);
				anim.SetBool("BasicAttack", false);
				anim.SetBool("TailAttack", false);
				anim.SetBool("FireBall", false);
				break;
			case "BasicAttack":
				CurState = Define.CharacterState.Attack1;
				anim.SetBool("Move", false);
				anim.SetBool("Hit", false);
				anim.SetBool("Die", false);
				anim.SetBool("BasicAttack", true);
				anim.SetBool("TailAttack", false);
				anim.SetBool("FireBall", false);
				break;
			case "TailAttack":
				CurState = Define.CharacterState.Attack2;
				anim.SetBool("Move", false);
				anim.SetBool("Hit", false);
				anim.SetBool("Die", false);
				anim.SetBool("BasicAttack", false);
				anim.SetBool("TailAttack", true);
				anim.SetBool("FireBall", false);
				break;
			case "FireBall":
				CurState = Define.CharacterState.Attack3;
				anim.SetBool("Move", false);
				anim.SetBool("Hit", false);
				anim.SetBool("Die", false);
				anim.SetBool("BasicAttack", false);
				anim.SetBool("TailAttack", false);
				anim.SetBool("FireBall", true);
				break;
			case "Hit":
				CurState = Define.CharacterState.Hit;
				anim.SetBool("Move", false);
				anim.SetBool("Hit", true);
				anim.SetBool("Die", false);
				anim.SetBool("BasicAttack", false);
				anim.SetBool("TailAttack", false);
				anim.SetBool("FireBall", false);
				break;
			case "Die":
				CurState = Define.CharacterState.Die;
				anim.SetBool("Move", false);
				anim.SetBool("Hit", false);
				anim.SetBool("Die", true);
				anim.SetBool("BasicAttack", false);
				anim.SetBool("TailAttack", false);
				anim.SetBool("FireBall", false);
				break;
		}
	}

	protected override void EnemyHit()
	{
		if(!isHit)
		{
			Managers.Sound.Play("Effect/Enemy/DragonHitVoice");
		}
		base.EnemyHit();
		if (isAttack || isDie)
			return;
		meshrender.material = HitMat;

		Managers.CallWaitForSeconds(0.1f, () => { meshrender.material = OriginMat; });


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

		BossCameras.SetActive(true);
		BossDirector.gameObject.SetActive(true);
		BossDirector.Play();
		StartCoroutine(BossCinemachineAction());

		Managers.Sound.Play("Effect/Enemy/DragonDie");
		Managers.GData.PlayerFeat[5].AddCount();

		Managers.Sound.BgmStop();		
		Managers.CallWaitForSeconds(1.2f, () => { Managers.Sound.Play("BGM/EndBossBattle", Define.Sound.Bgm); });

		foreach (var n in BossAttackCollider)
			n.gameObject.SetActive(false);

		// 아이템 드랍 (최소 1개 최대 3개 아이템 드랍)
		var Cube = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Prefabs/Object/ItemCube"));
		Cube.transform.position = transform.position + new Vector3(0, 3, 0);
		Cube.GetComponent<ItemCube>().Init(11);


		int Val = Random.Range(0, 99);
		if (Val > 75)
		{
			var Cube2 = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Prefabs/Object/ItemCube"));
			Cube2.transform.position = transform.position + new Vector3(0, 3, 0);
			Val = Random.Range(0, 99);
			if (Val < 25)
				Cube2.GetComponent<ItemCube>().Init(12);
			else
				Cube2.GetComponent<ItemCube>().Init(8);

			if (Val > 25)
			{
				var Cube3 = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Prefabs/Object/ItemCube"));
				Cube3.transform.position = transform.position + new Vector3(0, 3, 0);
				Cube3.GetComponent<ItemCube>().Init(4);
			}

		}


		// 타겟(플레이어)경험치 추가
		Target.GetComponent<CharactorMovement>().AddExp(state.EXP);

		// 재생중인 코루틴 있다면 멈추고
		StopCoroutine(CurCoroutine);
		// 사망 애니메이션 재생
		AnimationState("Die");
		GetComponent<Rigidbody>().useGravity = false;
		// 충돌 검사 제거
		GetComponent<BoxCollider>().enabled = false;
		// 보스 캔버스 비활성화
		BossCanvas.gameObject.SetActive(false);
	}

	IEnumerator BossCinemachineAction()
	{
		while (true)
		{
			yield return null;

			if (BossDirector.time >= BossDirector.duration)
			{
				Debug.Log("Finish Action");

				BossCameras.SetActive(false);
				BossDirector.gameObject.SetActive(false);	

				break;
			}

		}
	}


	void Init()
	{
		// GreenDragon은 보스몬스터로 하나의 레벨만 존재
		EnemyState mystate = new EnemyState(Managers.GData.GreenDragonState);

		state = new(mystate);

		// 각각의 bool값 초기화
		isAttack = false;
		isDie = false;
		isHit = false;
		isComeback = false;
		isFind = false;
		Target = null;


	}

	public override void CallHitEnemy(int Damage, bool critical)
	{
		// 체력 및 데미지 계산
		float Hit = Damage - (state.Def * 0.5f);
		int HitDamage = (int)Mathf.Ceil(Hit);

		if (HitDamage <= 0)
			HitDamage = 0;
		else
		{
			var hit = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Particle/HitParticle"));
			hit.transform.position = transform.position;
		}



		State.Hp -= HitDamage;

		Vector3 SpawnPos = transform.position + new Vector3(0, 2, 0);

		GameObject damageUI = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Prefabs/UI/HitCanvas"));
		damageUI.GetComponent<HitCanvasUI>().Init(HitDamage, SpawnPos, critical);

		// 체력이 0보다 작아지면 죽음처리
		if (State.Hp <= 0)
		{
			AnimationState("Die");
		}

		// 공격중이 아니면서 살아있고 포효애니메이션이 끝난상태라면 hit애니메이션 재생
		if (!isAttack && !isDie && endScreamAction)
		{
			AnimationState("Hit");
		}

	}


}
