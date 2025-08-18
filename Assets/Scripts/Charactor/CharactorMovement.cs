using System.Collections;
using UnityEngine;

// �÷��̾� ���� ��ũ��Ʈ
public class CharactorMovement : MonoBehaviour
{
	// ���̽�ƽ ������
	[SerializeField]
	Joystick moveInput;
	// ���̽��� �Է°�
	private MyTools.Pair<float, float> Inputfloat = new();
	// �̵� �ӵ�
	[SerializeField]
	float moveSpeed = 5.0f;
	// �ִϸ��̼� ��Ʈ�ѷ�
	[SerializeField]
	Animator anim;
	//���� ���������� Ȯ��
	[SerializeField]
	bool isAttack = false;
	// �÷��̾� ���Ȯ��
	public bool isDie = false;
	// �÷��̾��� �������ͽ�
	public CharactorState MyState;
	// �ʵ� Ÿ�Կ� ���� Ű �Է� �� �ٸ� �׼��� ����
	public Define.FieldType fieldtype = Define.FieldType.Field;
	// �ֺ� NPC üũ�� Collider 
	PlayerCheckNPC checkNPC;
	// NPC�� ���������� true
	public bool isAlmostNPC;
	// �÷��̾��� �Ӹ� �� UI Ȱ��ȭ/��Ȱ��ȭ
	public bool HidePlayerUI = false;
	// �� ���� Ʈ����
	public TrailRenderer SwordTrail;
	// ���� �ڽ��ݶ��̴�
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
		// �ʿ��� ������ �޾ƿ���
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

	// �÷��̾� �̵� ��ƼŬ ȿ��
	IEnumerator CreateMoveDust()
	{
		// ������ �ð����� ���� �ִϸ��̼� üũ �� ��ƼŬ ���
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

	// �Է��� ���� ���̽�ƽ ����, ���� �ʵ��� Ÿ���� ����
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
	// �÷��̾� �ִϸ��̼� ��¿�
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
	// �÷��̾� �ൿ
	void Movement()
	{
		// �Է¿� ���̽�ƽ�� ������� return
		if (moveInput == null) return;
		// �÷��̾ �׾��� ��� return
		if (isDie)
			return;
		// ���� ���ݾ׼� ���� ��� return
		if (isAttack) return;


		// �Է� �� �޾ƿ���
		Inputfloat.First = moveInput.Horizontal;
		Inputfloat.Second = moveInput.Vertical;
		// �Է°��� �Ѵ� 0�� ���(���̽�ƽ�� �Է��� ���� ���)
		if (Inputfloat.First == 0 && Inputfloat.Second == 0)
		{
			// ��� �ִϸ��̼� ���, return
			AnimationState("Idle");
			return;
		}
		// �Է��� �ִٸ� ���� �ִϸ��̼� ���
		AnimationState("Move");
		// �޾ƿ� ������ �÷��̾� ������
		Vector3 Dir = new Vector3(Inputfloat.First, 0, Inputfloat.Second);
		transform.position += Dir * moveSpeed * Time.deltaTime;
		transform.forward = Vector3.Lerp(transform.forward, Dir, Time.deltaTime * moveSpeed * 3.0f);
	}
	// ���� �ݹ��
	public void CallAttack()
	{
		if (isDie)
			return;

		// ���� ���� �������̶�� return
		if (isAttack || Attackcoroutine != null) return;
		else
		{
			// ���� ���°� Field�� ��� ���ݽ���
			if (fieldtype == Define.FieldType.Field)
			{
				AnimationState("Attack");
				isAttack = true;
				Managers.Sound.Play("Effect/Player/PlayerAttack");
				Attackcoroutine = StartCoroutine(AttackDelay());
			}
			// �����̶�� NPCüũ
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
		// ���� ���°� Field�� ��� ���ݽ���
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
		// �����̶�� return
		else if (fieldtype == Define.FieldType.Town)
		{
			return;
		}

	}

	// ���� �ִϸ��̼� �����Ȳ�� ���� isAttack�� ����
	IEnumerator AttackDelay()
	{
		// �� ���� Ȱ��ȭ
		SwordTrail.enabled = true;
		isAttack = true;

		while (true)
		{
			yield return null;
			// ���� ������� �ִϸ��̼��� �̸��� Attack�϶�
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
			{
				// �ִϸ��̼��� �ð� �帧�� ���� ���ݹ����� BoxCollider�� Ȱ��ȭ/��Ȱ��ȭ
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
		// ���� �ִϸ��̼��� ���� �� �� ���� ��Ȱ��ȭ, ���ݹ��� BoxCollider ��Ȱ��ȭ
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


	// �÷��̾��� ��� ���� üũ
	void CheckPlayerDie()
	{
		// 1ȸ�� �����ϱ����� bool��
		if (isDie)
			return;

		// �÷��̾��� ü���� 0���� �۾����ٸ�
		if(MyState.CurState.Hp <= 0)
		{
			// ���ó��, ����ִϸ��̼� ���, ���UI���, ���� ���
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

	// ����ġ �߰�
	public void AddExp(int exp)
	{
		// ����ġ �߰�
		MyState.Exp += exp;
		// �ݺ��� ������ �����Ǵ� ����ġ��ŭ ������
		while (true)
		{
			if (MyState.Exp >= MyState.MaxExp)
			{
				// ���� ����ġ������ ���ҽ�Ű�� ������
				MyState.Exp -= MyState.MaxExp;
				// �������ϸ鼭 �ʿ� ����ġ�� �ٲ�� ����
				MyState.LevelUp();
			}
			// ���̻� �������� �ȵȴٸ� �ݺ��� Ż��
			else
				break;
		}
	}
	// �÷��̾� �ǰ� �ݹ�
	public void CallHitPlayer(float damage)
	{

		// ü�� �� ������ ���
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

		// �������̰ų� �׾��ִ°� �ƴ϶�� hit�ִϸ��̼� ���
		if (!isAttack && !isDie)
		{
			AnimationState("Hit");
		}
		else if (MyState.CurState.Hp <= 0)
			return;
	}
}
