using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// FSM��ũ��Ʈ
// ���� �� ������ �پ��ϰ� �ϱ����� ��κ��� �Լ��� virtual�� ����
// ������ ���� ��ũ��Ʈ �ۼ� �� ������ ������
public class EnemyState
{
	// �������̽�
	public string Name;
	public int Level;
	public int MaxHp;
	public int Hp;
	public int EXP;
	// �ʱ� ����
	public int Str;
	public int Dex;
	public int Sti;
	public int Luk;
	// ��ȯ ����
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

// ���ʹ� ���� ��ũ��Ʈ
// ���ư����� ��ġ���� ���� �Ÿ� ���̿��� ��Ȳ
// �̵��� ���� �Ÿ��̻� �̵��� ��� �ǵ��ư�
// ���� ��Ÿ� �� �÷��̾ ���´ٸ� �����Ϸ� �̵�
// �̵��� ���� �Ÿ��̻� �̵��� ��� �ǵ��ư�
// ���� �߿��� �ǰݴ��ϴ��� �ִϸ��̼��� ������ ����
// ��� ��Ȳ������ Die�ִϸ��̼� �� ���°� �켱�� ��
public abstract class EnemyFSM : MonoBehaviour
{
	// ���� ���� enum
	public Define.EnemyType enemyType = Define.EnemyType.Unknown;
	// ���� �������ͽ�
	protected EnemyState state = null;
	public EnemyState State {  get { return state; } }
	
	protected float moveSpeed = 3.0f;

	// ���ʹ� �ִϸ��̼�
	protected Animator anim;
	// �÷��̾� Ž��(�ڽİ�ü)
	protected EnemyCheckPlayer Checker;
	// ���� Ʈ����
	[SerializeField]
	protected EnemyAttackCollider attackCollider;

	// ���� ���� ǥ��
	[SerializeField]
	protected Define.CharacterState CurState = Define.CharacterState.Idle;

	// ���ư� ��ġ
	public Vector3 ComebackPosition;
	// ���� �Ÿ�
	public float LimitDistacne = 10.0f;

	// ���� ������
	[SerializeField]
	protected float AttackDelay = 2.0f;
	// ���� ������ 
	[SerializeField]
	protected float curDelay = 0.0f;
	// ������ ��(�÷��̾�)
	public Transform Target = null;
	// ���ʹ̿� Ÿ�� ���� �Ÿ� (�����ʿ�)
	protected float TargetDistance = 0.0f;
	// ���� �Ĺ������ǰ� ���ʹ� ���� ������ �Ÿ� (�����ʿ�)
	protected float CurDistance = 0.0f;
	// ���� ��Ÿ� (���� ����)
	[SerializeField]
	protected float AttackDistance = 1.0f;
	// ���� ���������� Ȯ��
	protected bool isAttack = false;
	// ���ʹ� ��� Ȯ��
	public bool isDie = false;
	// ��Ʈ �ִϸ��̼� ��������� äũ
	protected bool isHit = false;
	// �ǵ��� ���ߵǴ��� üũ
	public bool isComeback = false;
	// ���� ������� �ڷ�ƾ �˻��
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

	// ���ʹ� �ʱ�ȭ
	protected virtual void Init(Vector3 comeback, EnemyState _state = null)
	{
		Debug.Log("EnemyFSM Init!");
		// �߰��� �����Ͱ� ���ٸ� �����͵� ���ʰ�(�������)
		if (_state == null)
			state = new EnemyState();
		else
			state = new EnemyState(_state);

		// ������ bool�� �ʱ�ȭ
		isAttack = false;
		isDie = false;
		isHit = false;
		isComeback = false;

		Target = null;
		// ���ʹ�UI ����
		GameObject obj = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Prefabs/UI/EnemyCanvas"));
		enemyUI = obj.GetComponent<EnemyCanvas>();
		enemyUI.Init(this);

		ComebackPosition = comeback;
		CurDistance = Vector3.Distance(transform.position, ComebackPosition);

	}

	// �ִϸ��̼� ��¿�
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
	// ���ʹ̿� Ÿ�� ���� �Ÿ� ����
	protected virtual void CheckTargetDistance()
	{
		if (Target == null)
			return;
		TargetDistance = Vector3.Distance(transform.position,Target.position);
	}
	// ���ʹ̿� �Ĺ� ������ ���� �Ÿ� ����
	protected virtual void CheckCurDistance()
	{
		CurDistance = Vector3.Distance(transform.position, ComebackPosition);
	}
	// ü���� Ȯ���Ͽ� ���ʹ��� ������� üũ
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

		// Ÿ���� ���� ��
		if (Target != null)
		{
			// �̵����� �Ÿ��� ���� �������� ��
			if (CurDistance < LimitDistacne)
			{
				// ���� ��Ÿ����� Ÿ�ٰ��� �Ÿ��� �� ũ�ٸ�
				if (AttackDistance < TargetDistance)
				{
					// �̵����� ��ȯ
					AnimationState("Move");
					return;
				}
				// Ÿ���� ���� ��Ÿ� ������ ������ ��
				else
				{
					// ���� �����̰� �غ�Ǿ��ְ� ���� �������� �ƴ� ��
					if (curDelay > AttackDelay && !isAttack)
					{
						// ���� ���·� ��ȯ
						AnimationState("Attack");
						return;
					}
					// �������̰ų� ���� �����̰� ���ڶ� ��
					else
					{
						// ������ ��ٸ��鼭 ���
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

		// ���ʹ̿� Ÿ�� ���� �Ÿ� ����
		CheckTargetDistance();
		// ���ʹ̿� �Ĺ�c�� ���� �Ÿ� ����
		CheckCurDistance();

		Vector3 Dir;

		// �Ĺ� ���������� �Ÿ��� �̵����ѰŸ����� ũ�ų� ���ư����ϴ� �����
		if (CurDistance > LimitDistacne || isComeback)
		{
			// comeback is true
			isComeback = true;
			// Ÿ�� ����
			Target = null;
			// �̵��� ����
			Dir = ComebackPosition - transform.position;
			Dir = Dir.normalized;

			// ���� �̵��ӵ��� 2��� ������ ����
			transform.position += Dir * (moveSpeed * 2) * Time.deltaTime;
			transform.forward = Vector3.Lerp(transform.forward, Dir, Time.deltaTime * moveSpeed * 3.0f);
			// ���ư��� �Ÿ��� 1.0f���� �۰ų� ���ٸ�
			if (CurDistance <= 1.0f)
			{
				// Ÿ���� ����ΰ�
				Target = null;
				// ���̻� ���ư� �ʿ����
				isComeback = false;
				// �����·� ��ȯ
				AnimationState("Idle");
				return;
			}
		}
		// �ƴ϶��(Ÿ������ �̵�)
		else
		{
			if (Target == null)
			{
				isComeback = true;
				return;
			}

			// �̵��� ����
			Dir = Target.transform.position - transform.position;
			Dir = Dir.normalized;

			transform.position += Dir * moveSpeed * Time.deltaTime;
			transform.forward = Vector3.Lerp(transform.forward, Dir, Time.deltaTime * moveSpeed * 3.0f);
			// ���ݻ�Ÿ������� Ÿ���� ���Դٸ�
			if (AttackDistance > TargetDistance)
			{
				// �̵��� ���߰� �����·� ��ȯ -> ���� �����¿��� �˻� �� ���ݻ��·� ��ȯ
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

		// ������ ���·� ��ȯ
		isAttack = true;
		attackCollider.gameObject.SetActive(true);
		// ���ݾִϸ��̼� üũ ����
		CurCoroutine = StartCoroutine(AttackAction());

	}

	IEnumerator AttackAction()
	{
		yield return null;

		while (true)
		{
			yield return null;
			// ���� ������� �ִϸ��̼��� ������ �޾ƿ� �׼��� �������� Ȯ��
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
		// �ִϸ��̼��� �����ٸ� ������ �ʱ�ȭ, ���׼� 
		curDelay = 0.0f;
		AnimationState("Idle");

	}

	protected virtual void EnemyHit()
	{
		if (isAttack || isDie)
			return;


		// Hit������ ��ȯ
		isHit = true;
		// �ִϸ��̼� üũ
		CurCoroutine = StartCoroutine(HitAction());

	}
	IEnumerator HitAction()
	{
		while (true)
		{
			yield return null;
			// ���� ������� �ִϸ��̼��� ������ �޾ƿ� �׼��� �������� Ȯ��
			if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.95f)
			{

				isHit = false;
				break;
			}
		}
		// �ִϸ��̼��� �����ٸ� ���׼� 
		AnimationState("Idle");


	}

	protected virtual void EnemyDie()
	{
		// Die�׼� 1ȸ ����� ���� ���� �׾��� �� Ȯ�� �� ���� �ʾҴٸ�
		// ���� ó�� �� �ִϸ��̼� ���
		if (isDie)
			return;

		// ��� �̿� ���� false
		isDie = true;
		isHit = false;
		isAttack = false;

		attackCollider.gameObject.SetActive(false);

		// Ÿ��(�÷��̾�)����ġ �߰�
		Target.GetComponent<CharactorMovement>().AddExp(state.EXP);

		// ������� �ڷ�ƾ �ִٸ� ���߰�
		if(CurCoroutine != null)
			StopCoroutine(CurCoroutine);
		// ��� �� �ڷ�ƾ ���
		StartCoroutine(DieAfter());
	}
	
	// ����� �ð� ��� �� ������� �� �⺻�� �ʱ�ȭ
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

	// ��Ʈ �ݹ� �Լ�
	public virtual void CallHitEnemy(int Damage, bool critical)
	{
		// ü�� �� ������ ���
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

		// �������̰ų� �׾��ִ°� �ƴ϶�� hit�ִϸ��̼� ���
		if(!isAttack && !isDie)
		{
			AnimationState("Hit");
		}
		
	}

}
