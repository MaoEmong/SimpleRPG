using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// FSM스크립트
// 몬스터 별 설정을 다양하게 하기위해 대부분의 함수는 virtual로 제작
// 각각의 몬스터 스크립트 작성 중 수정에 용이함
public class EnemyState
{
	// 인터페이스
	public string Name;
	public int Level;
	public int MaxHp;
	public int Hp;
	public int EXP;
	// 초기 스텟
	public int Str;
	public int Dex;
	public int Sti;
	public int Luk;
	// 전환 스텟
	public float Atk;
	public float Def;
	public float MoveSpeed;
	public float Critical;

	public EnemyState() { }
	public EnemyState(string name, int level, int maxHp, int hp, int eXP, int str, int dex, int sti, int luk, float atk, float def, float moveSpeed, float critical)
	{
		Name = name;
		Level = level;
		MaxHp = maxHp;
		Hp = hp;
		EXP = eXP;
		Str = str;
		Dex = dex;
		Sti = sti;
		Luk = luk;
		Atk = atk;
		Def = def;
		MoveSpeed = moveSpeed;
		Critical = critical;
	}
	public EnemyState(string name, int level, int maxHp, int hp, int eXP, int str, int dex, int sti, int luk)
	{
		Name = name;
		Level = level;
		MaxHp = maxHp;
		EXP = eXP;
		Str = str;
		Dex = dex;
		Sti = sti;
		Luk = luk;

		ExchageState();
	}
	public EnemyState(EnemyState state)
	{
		Name = state.Name;
		Level = state.Level;
		MaxHp = state.MaxHp;
		Hp = state.Hp;
		EXP = state.EXP;
		Str = state.Str;
		Dex = state.Dex;
		Sti = state.Sti;
		Luk = state.Luk;
		Atk = state.Atk;
		Def = state.Def;
		MoveSpeed = state.MoveSpeed;
		Critical = state.Critical;
	}

	public EnemyState(GameData.EnemyBasicState state)
	{
		Name = state.Name;
		Level = state.Level;
		MaxHp = state.MaxHp;
		EXP = state.EXP;
		Str = state.STR;
		Dex = state.Dex;
		Sti = state.STI;
		Luk = state.LUK;
		ExchageState();
	}

	void ExchageState()
	{
		MaxHp = Sti * 10;
		Hp = MaxHp;
		Atk = Str * 1.5f;
		Def = Sti * 1.5f;
		MoveSpeed = 1 + (Dex * 0.1f);
		Critical = Luk * 0.1f;
	}
}

// 에너미 기초 스크립트
// 돌아가야할 위치에서 제한 거리 사이에서 방황
// 이동중 제한 거리이상 이동할 경우 되돌아감
// 공격 사거리 내 플레이어가 들어온다면 공격하러 이동
// 이동중 제한 거리이상 이동할 경우 되돌아감
// 공격 중에는 피격당하더라도 애니메이션이 끊기지 않음
// 어떠한 상황에서드 Die애니메이션 및 상태가 우선시 됨
public abstract class EnemyFSM : MonoBehaviour
{
	// 현재 상태 enum
	public Define.EnemyType enemyType = Define.EnemyType.Unknown;
	// 적의 스테이터스
	protected EnemyState state = null;
	public EnemyState State {  get { return state; } }
	
	protected float moveSpeed = 3.0f;

	// 에너미 애니메이션
	protected Animator anim;
	// 플레이어 탐지(자식객체)
	protected EnemyCheckPlayer Checker;
	// 공격 트리거
	[SerializeField]
	protected EnemyAttackCollider attackCollider;

	// 현재 상태 표시
	[SerializeField]
	protected Define.CharacterState CurState = Define.CharacterState.Idle;

	// 돌아갈 위치
	public Vector3 ComebackPosition;
	// 제한 거리
	public float LimitDistacne = 10.0f;

	// 공격 딜레이
	[SerializeField]
	protected float AttackDelay = 2.0f;
	// 현재 딜레이 
	[SerializeField]
	protected float curDelay = 0.0f;
	// 공격할 적(플레이어)
	public Transform Target = null;
	// 에너미와 타겟 사이 거리 (갱신필요)
	protected float TargetDistance = 0.0f;
	// 현재 컴백포지션과 에너미 사이 떨어진 거리 (갱신필요)
	protected float CurDistance = 0.0f;
	// 공격 사거리 (갱신 없음)
	[SerializeField]
	protected float AttackDistance = 1.0f;
	// 현재 공격중인지 확인
	protected bool isAttack = false;
	// 에너미 사망 확인
	public bool isDie = false;
	// 히트 애니메이션 재생중인지 채크
	protected bool isHit = false;
	// 되돌아 가야되는지 체크
	public bool isComeback = false;
	// 현재 재생중인 코루틴 검사용
	protected Coroutine CurCoroutine = null;
		
	public EnemyCanvas enemyUI;


	protected virtual void Start()
	{
		anim = GetComponent<Animator>();
		Checker = GetComponentInChildren<EnemyCheckPlayer>();
		Checker.Init(this); 
		attackCollider = GetComponentInChildren<EnemyAttackCollider>();
		attackCollider.gameObject.SetActive(false);
		isAttack = false;
		isDie = false;

		StartCoroutine(CreateMoveDust());
	}

	IEnumerator CreateMoveDust()
	{
		float endTime = 0.3f;
		float curTime = 0.0f;

		while(true)
		{
			yield return null;
			curTime += Time.deltaTime;
			if(curTime >= endTime)
			{
				curTime = 0.0f;
				if(CurState == Define.CharacterState.Move)
				{
					var dust = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Particle/MoveDust"));
					//dust.transform.position = transform.position + new Vector3(0, -1, 0);
					dust.transform.position = transform.position;
				}
			}


		}

	}

	// 에너미 초기화
	protected virtual void Init(Vector3 comeback, EnemyState _state = null)
	{
		Debug.Log("EnemyFSM Init!");
		// 추가한 데이터가 없다면 데이터도 최초값(비어있음)
		if (_state == null)
			state = new EnemyState();
		else
			state = new EnemyState(_state);

		// 각각의 bool값 초기화
		isAttack = false;
		isDie = false;
		isHit = false;
		isComeback = false;

		Target = null;
		// 에너미UI 생성
		GameObject obj = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Prefabs/UI/EnemyCanvas"));
		enemyUI = obj.GetComponent<EnemyCanvas>();
		enemyUI.Init(this);

		ComebackPosition = comeback;
		CurDistance = Vector3.Distance(transform.position, ComebackPosition);

	}

	// 애니메이션 출력용
	public virtual void AnimationState(string name)
	{
		switch (name)
		{
			case "Idle":
				CurState = Define.CharacterState.Idle;
				anim.SetBool("Move", false);
				anim.SetBool("Hit", false);
				anim.SetBool("Die", false);
				anim.SetBool("Attack", false);
				break;
			case "Move":
				CurState = Define.CharacterState.Move;
				anim.SetBool("Move", true);
				anim.SetBool("Hit", false);
				anim.SetBool("Die", false);
				anim.SetBool("Attack", false);
				break;
			case "Attack":
				CurState = Define.CharacterState.Attack;
				anim.SetBool("Move", false);
				anim.SetBool("Hit", false);
				anim.SetBool("Die", false);
				anim.SetBool("Attack", true);
				break;
			case "Hit":
				CurState = Define.CharacterState.Hit;
				anim.SetBool("Move", false);
				anim.SetBool("Hit", true);
				anim.SetBool("Die", false);
				anim.SetBool("Attack", false);
				break;
			case "Die":
				CurState = Define.CharacterState.Die;
				anim.SetBool("Move", false);
				anim.SetBool("Hit", false);
				anim.SetBool("Die", true);
				anim.SetBool("Attack", false);
				break;
		}
	}

	protected virtual void Update()
	{
		CheckDieEnemy();
		CheckTargetDistance();
		CheckCurDistance();

		EnemyMovement();

	}
	// 에너미와 타겟 사이 거리 갱신
	protected virtual void CheckTargetDistance()
	{
		if (Target == null)
			return;
		TargetDistance = Vector3.Distance(transform.position,Target.position);
	}
	// 에너미와 컴백 포지션 사이 거리 갱신
	protected virtual void CheckCurDistance()
	{
		CurDistance = Vector3.Distance(transform.position, ComebackPosition);
	}
	// 체력을 확인하여 에너미의 사망여부 체크
	protected virtual void CheckDieEnemy()
	{
		if (state == null)
			return;

		if (state.Hp <= 0)
		{
			AnimationState("Die");
		}

	}


	protected virtual void EnemyMovement()
	{
		switch(CurState)
		{
			case Define.CharacterState.Idle:
				EnemyIdle();
				break;

			case Define.CharacterState.Move:
				EnemyMove();
				break;

			case Define.CharacterState.Attack:
				EnemyAttack();
				break;

			case Define.CharacterState.Hit:
				EnemyHit();
				break;

			case Define.CharacterState.Die:
				EnemyDie();
				break;
		}
	}

	protected virtual void EnemyIdle()
	{
		if (isAttack || isDie)
			return;

		if (isHit)
			return;

		// 타겟이 있을 때
		if (Target != null)
		{
			// 이동제한 거리가 아직 남아있을 때
			if (CurDistance < LimitDistacne)
			{
				// 공격 사거리보다 타겟과의 거리가 더 크다면
				if (AttackDistance < TargetDistance)
				{
					// 이동으로 전환
					AnimationState("Move");
					return;
				}
				// 타겟이 공격 사거리 안으로 들어왔을 떄
				else
				{
					// 공격 딜레이가 준비되어있고 현재 공격중이 아닐 때
					if (curDelay > AttackDelay && !isAttack)
					{
						// 공격 상태로 전환
						AnimationState("Attack");
						return;
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

		}
		else
		{
			AnimationState("Idle");
		}

	}
	protected virtual void EnemyMove()
	{
		if (isAttack || isDie)
			return;

		if (isHit)
			return;

		// 에너미와 타겟 사이 거리 갱신
		CheckTargetDistance();
		// 에너미와 컴백푖션 사이 거리 갱신
		CheckCurDistance();

		Vector3 Dir;

		// 컴백 포지선과의 거리가 이동제한거리보다 크거나 돌아가야하는 경우라면
		if (CurDistance > LimitDistacne || isComeback)
		{
			// comeback is true
			isComeback = true;
			// 타겟 비우기
			Target = null;
			// 이동값 설정
			Dir = ComebackPosition - transform.position;
			Dir = Dir.normalized;

			// 기존 이동속도의 2배로 빠르게 복귀
			transform.position += Dir * (moveSpeed * 2) * Time.deltaTime;
			transform.forward = Vector3.Lerp(transform.forward, Dir, Time.deltaTime * moveSpeed * 3.0f);
			// 돌아가는 거리가 1.0f보다 작거나 같다면
			if (CurDistance <= 1.0f)
			{
				// 타겟은 비워두고
				Target = null;
				// 더이상 돌아갈 필요없음
				isComeback = false;
				// 대기상태로 전환
				AnimationState("Idle");
				return;
			}
		}
		// 아니라면(타겟으로 이동)
		else
		{
			if (Target == null)
			{
				isComeback = true;
				return;
			}

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


	}
	protected virtual void EnemyAttack()
	{
		if (isDie)
			return;

		if (isAttack)
			return;

		// 공격중 상태로 전환
		isAttack = true;
		attackCollider.gameObject.SetActive(true);
		// 공격애니메이션 체크 실행
		CurCoroutine = StartCoroutine(AttackAction());

	}

	IEnumerator AttackAction()
	{
		yield return null;

		while (true)
		{
			yield return null;
			// 현재 재생중인 애니메이션의 정보를 받아와 액션이 끝났는지 확인
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
			{
				if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.90f)
				{

					isAttack = false;
					attackCollider.gameObject.SetActive(false);

					break;
				}
			}
		}
		// 애니메이션이 끝났다면 딜레이 초기화, 대기액션 
		curDelay = 0.0f;
		AnimationState("Idle");

	}

	protected virtual void EnemyHit()
	{
		if (isAttack || isDie)
			return;


		// Hit중으로 전환
		isHit = true;
		// 애니메이션 체크
		CurCoroutine = StartCoroutine(HitAction());

	}
	IEnumerator HitAction()
	{
		while (true)
		{
			yield return null;
			// 현재 재생중인 애니메이션의 정보를 받아와 액션이 끝났는지 확인
			if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.95f)
			{

				isHit = false;
				break;
			}
		}
		// 애니메이션이 끝났다면 대기액션 
		AnimationState("Idle");


	}

	protected virtual void EnemyDie()
	{
		// Die액션 1회 재생을 위해 현재 죽었는 지 확인 후 죽지 않았다면
		// 죽음 처리 후 애니메이션 재생
		if (isDie)
			return;

		// 사망 이외 전부 false
		isDie = true;
		isHit = false;
		isAttack = false;

		attackCollider.gameObject.SetActive(false);

		// 타겟(플레이어)경험치 추가
		Target.GetComponent<CharactorMovement>().AddExp(state.EXP);

		// 재생중인 코루틴 있다면 멈추고
		if(CurCoroutine != null)
			StopCoroutine(CurCoroutine);
		// 사망 후 코루틴 재생
		StartCoroutine(DieAfter());
	}
	
	// 잠깐의 시간 대기 후 사라지기 전 기본값 초기화
	protected IEnumerator DieAfter()
	{
		yield return new WaitForSeconds(1.0f);

		AnimationState("Idle");
		isAttack = false;
		isDie = false;
		isHit = false;
		isComeback = false;

		Managers.Pool.Push(enemyUI.gameObject);
		Managers.Pool.Push(this.gameObject);

	}

	// 히트 콜백 함수
	public virtual void CallHitEnemy(int Damage, bool critical)
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

		GameObject damageUI = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Prefabs/UI/HitCanvas"));
		damageUI.GetComponent<HitCanvasUI>().Init(HitDamage,transform.position, critical);


		if (State.Hp <= 0)
		{
			AnimationState("Die");
		}

		// 공격중이거나 죽어있는게 아니라면 hit애니메이션 재생
		if(!isAttack && !isDie)
		{
			AnimationState("Hit");
		}
		
	}

}
