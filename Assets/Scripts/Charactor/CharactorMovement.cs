using System.Collections;
using UnityEngine;

// 플레이어 조작 스크립트
public class CharactorMovement : MonoBehaviour
{
	// 조이스틱 데이터
	[SerializeField]
	Joystick moveInput;
	// 조이스팁 입력값
	private MyTools.Pair<float, float> Inputfloat = new();
	// 이동 속도
	[SerializeField]
	float moveSpeed = 5.0f;
	// 애니메이션 컨트롤러
	[SerializeField]
	Animator anim;
	//현재 공격중인지 확인
	[SerializeField]
	bool isAttack = false;
	// 플레이어 사망확인
	public bool isDie = false;
	// 플레이어의 스테이터스
	public CharactorState MyState;
	// 필드 타입에 따라 키 입력 시 다른 액션을 취함
	public Define.FieldType fieldtype = Define.FieldType.Field;
	// 주변 NPC 체크용 Collider 
	PlayerCheckNPC checkNPC;
	// NPC가 근접했을때 true
	public bool isAlmostNPC;
	// 플레이어의 머리 위 UI 활성화/비활성화
	public bool HidePlayerUI = false;
	// 검 궤적 트레일
	public TrailRenderer SwordTrail;
	// 공격 박스콜라이더
	public Collider AttackCollider;
	public Collider Skill1Collider;
		
	public float Skill1DelayTime = 3.0f;


	[SerializeField]
	PlayerDieCanvas dieCanvas;

	[SerializeField]
	ParticleSystem MoveDust;

	Coroutine Attackcoroutine = null;

	private void Start()
	{
		// 필요한 데이터 받아오기
		anim = GetComponentInChildren<Animator>();
		MyState = GetComponent<CharactorState>();
		checkNPC = GetComponentInChildren<PlayerCheckNPC>();
		isAlmostNPC = false;
		SwordTrail.enabled = false;
		AttackCollider.GetComponent<AttackBoxColider>().Init(this);
		AttackCollider.enabled = false;
		Skill1Collider.GetComponent<AttackBoxColider>().Init(this);
		Skill1Collider.enabled = false;

		dieCanvas.gameObject.SetActive(false);
		StartCoroutine(CreateMoveDust());

	}

	// 플레이어 이동 파티클 효과
	IEnumerator CreateMoveDust()
	{
		// 일정한 시간마다 현재 애니메이션 체크 후 파티클 재생
		float endTime = 0.2f;
		float curTime = 0.0f;

		while(true)
		{
			yield return null;
			curTime += Time.deltaTime;
			if (curTime > endTime)
			{				
				curTime = 0.0f;
				if(anim.GetBool("Move"))
				{
					var dust = Managers.Pool.Pop(MoveDust.gameObject);
					dust.transform.position = transform.position + new Vector3(0, -1, 0);
					Managers.Sound.Play("Effect/Player/PlayerMove");
				}
			}

		}

	}

	// 입력을 위한 조이스틱 배정, 현재 필드의 타입을 정함
	public void Init(Joystick _joy, Define.FieldType ftype)
	{
		moveInput = _joy;

		fieldtype = ftype;
	}

	private void Update()
	{
		CheckPlayerDie();
		Movement();

	}
	// 플레이어 애니메이션 출력용
	void AnimationState(string name)
	{
		switch(name)
		{
			case "Idle":
				anim.SetBool("Move", false);
				anim.SetBool("Hit", false);
				anim.SetBool("Die", false);
				anim.SetBool("Attack", false);
				anim.SetBool("Skill1", false);
				break;
			case "Move":
				anim.SetBool("Move", true);
				anim.SetBool("Hit", false);
				anim.SetBool("Die", false);
				anim.SetBool("Attack", false);
				anim.SetBool("Skill1", false);
				break;
			case "Attack":
				anim.SetBool("Move", false);
				anim.SetBool("Hit", false);
				anim.SetBool("Die", false);
				anim.SetBool("Attack", true);
				anim.SetBool("Skill1", false);
				break;
			case "Skill1":
				anim.SetBool("Move", false);
				anim.SetBool("Hit", false);
				anim.SetBool("Die", false);
				anim.SetBool("Attack", false);
				anim.SetBool("Skill1", true);
				break;
			case "Hit":
				anim.SetBool("Move", false);
				anim.SetBool("Hit", true);
				anim.SetBool("Die", false);
				anim.SetBool("Attack", false);
				anim.SetBool("Skill1", false);
				break;
			case "Die":
				anim.SetBool("Move", false);
				anim.SetBool("Hit", false);
				anim.SetBool("Die", true);
				anim.SetBool("Attack", false);
				anim.SetBool("Skill1", false);
				break;
		}
	}
	// 플레이어 행동
	void Movement()
	{
		// 입력용 조이스틱이 없을경우 return
		if (moveInput == null) return;
		// 플레이어가 죽었을 경우 return
		if (isDie)
			return;
		// 현재 공격액션 중일 경우 return
		if (isAttack) return;


		// 입력 값 받아오기
		Inputfloat.First = moveInput.Horizontal;
		Inputfloat.Second = moveInput.Vertical;
		// 입력값이 둘다 0일 경우(조이스틱의 입력이 없을 경우)
		if (Inputfloat.First == 0 && Inputfloat.Second == 0)
		{
			// 대기 애니메이션 출력, return
			AnimationState("Idle");
			return;
		}
		// 입력이 있다면 무브 애니메이션 출력
		AnimationState("Move");
		// 받아온 값으로 플레이어 움직임
		Vector3 Dir = new Vector3(Inputfloat.First, 0, Inputfloat.Second);
		transform.position += Dir * moveSpeed * Time.deltaTime;
		transform.forward = Vector3.Lerp(transform.forward, Dir, Time.deltaTime * moveSpeed * 3.0f);
	}
	// 공격 콜백용
	public void CallAttack()
	{
		if (isDie)
			return;

		// 현재 공격 진행중이라면 return
		if (isAttack || Attackcoroutine != null) return;
		else
		{
			// 현재 상태가 Field일 경우 공격실행
			if (fieldtype == Define.FieldType.Field)
			{
				AnimationState("Attack");
				isAttack = true;
				Managers.Sound.Play("Effect/Player/PlayerAttack");
				Attackcoroutine = StartCoroutine(AttackDelay());
			}
			// 마을이라면 NPC체크
			else if (fieldtype == Define.FieldType.Town)
			{
				if (isAlmostNPC)
				{
					checkNPC.npc.CallNPCUI(this);
				}
			}
		}

	}
	public void CallSkill1()
	{
		if (isDie)
			return;
		// 현재 상태가 Field일 경우 공격실행
		if (fieldtype == Define.FieldType.Field)
		{
			AnimationState("Skill1");
			isAttack = true;
			Managers.Sound.Play("Effect/Player/PlayerSkill1");
			if(Attackcoroutine != null)
			{
				StopCoroutine(Attackcoroutine);
			}
			Attackcoroutine = StartCoroutine(SkillAttackDelay());
		}
		// 마을이라면 return
		else if (fieldtype == Define.FieldType.Town)
		{
			return;
		}

	}

	// 공격 애니메이션 진행상황에 따라 isAttack값 지정
	IEnumerator AttackDelay()
	{
		// 검 궤적 활성화
		SwordTrail.enabled = true;
		isAttack = true;

		while (true)
		{
			yield return null;
			// 현재 재생중인 애니메이션의 이름이 Attack일때
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
			{
				// 애니메이션의 시간 흐름에 따라 공격범위의 BoxCollider의 활성화/비활성화
				if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f)
				{
					if (SwordTrail.enabled)
					{
						SwordTrail.enabled = false;
					}
				}
				if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.1f && !AttackCollider.enabled)
				{
					AttackCollider.enabled = true;
				}
				if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
				{
					break;
				}
			}
		}
		// 공격 애니메이션이 끝난 후 검 궤적 비활성화, 공격범위 BoxCollider 비활성화
		SwordTrail.enabled = false;
		AttackCollider.enabled = false; 
		isAttack = false;
		Managers.CallWaitForSeconds(0.3f,() => { Attackcoroutine = null; });
		
	}

	IEnumerator SkillAttackDelay()
	{
		AttackCollider.enabled = false;
		SwordTrail.enabled = true;

		isAttack = true;

		while (true)
		{
			yield return null;
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("Skill1"))
			{
				if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f)
				{
					if (SwordTrail.enabled)
					{
						SwordTrail.enabled = false;
					}
				}

				if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.1f && !Skill1Collider.enabled)
				{
					Skill1Collider.enabled = true;
				}


				if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
				{
					break;
				}
			}
		}
		SwordTrail.enabled = false;
		Skill1Collider.enabled = false;
		isAttack = false;
		Managers.CallWaitForSeconds(0.3f, () => { Attackcoroutine = null; });
	}


	// 플레이어의 사망 여부 체크
	void CheckPlayerDie()
	{
		// 1회만 적용하기위한 bool값
		if (isDie)
			return;

		// 플레이어의 체력이 0보다 작아진다면
		if(MyState.CurState.Hp <= 0)
		{
			// 사망처리, 사망애니메이션 재생, 사망UI출력, 사운드 출력
			GetComponent<CapsuleCollider>().enabled = false;
			isDie = true;
			isAttack = false;
			AnimationState("Die");
			Debug.Log("Player Die!");
			dieCanvas.gameObject.SetActive(true);
			Managers.Sound.Play("BGM/PlayerDie", Define.Sound.Bgm);
			Managers.CallWaitForSeconds(0.5f, () => { dieCanvas.StartAction(); });
		}
	}

	// 경험치 추가
	public void AddExp(int exp)
	{
		// 경험치 추가
		MyState.Exp += exp;
		// 반복문 돌리며 오버되는 경험치만큼 레벨업
		while (true)
		{
			if (MyState.Exp >= MyState.MaxExp)
			{
				// 현재 경험치량부터 감소시키고 레벨업
				MyState.Exp -= MyState.MaxExp;
				// 레벨업하면서 필요 경험치량 바뀌기 때문
				MyState.LevelUp();
			}
			// 더이상 레벨업이 안된다면 반복문 탈출
			else
				break;
		}
	}
	// 플레이어 피격 콜백
	public void CallHitPlayer(float damage)
	{

		// 체력 및 데미지 계산
		float Hit = damage - (MyState.Def * 0.5f);
		int HitDamage = (int)Mathf.Ceil(Hit);

		if (HitDamage <= 0)
		{
			HitDamage = 0;
		}
		else
		{
			var hit = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Particle/HitParticle"));
			hit.transform.position = transform.position;
			Managers.Sound.Play("Effect/Player/PlayerHit");
		}

		MyState.CurState.Hp -= HitDamage;

		GameObject damageUI = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Prefabs/UI/HitCanvas"));
		damageUI.GetComponent<HitCanvasUI>().Init(HitDamage, transform.position,false);

		// 공격중이거나 죽어있는게 아니라면 hit애니메이션 재생
		if (!isAttack && !isDie)
		{
			AnimationState("Hit");
		}
		else if (MyState.CurState.Hp <= 0)
			return;
	}
}
