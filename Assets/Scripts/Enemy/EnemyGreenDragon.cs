using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

// ��������
// EnemyFSM�� ��ӹ����� ���� �κ��� ������
public class EnemyGreenDragon : EnemyFSM
{
	// ��Ʈ ǥ�ÿ� ���׸���
	[SerializeField]
	Material OriginMat;
	[SerializeField]
	Material HitMat;
	// ���׸��� ��ü�� ���� �޽÷�����
	SkinnedMeshRenderer meshrender;

	// �������� ���� ĵ����
	public Canvas BossCanvas;
	public Image BossHpbar;
	public Text BossNameText;
	public Text BossLevelText;
	public Text HpText;

	// �÷��̾� Ž�� ����
	BossCheckPlayer BossChecker;
	// �÷��̾� Ž�� Ȯ��
	bool isFind = false;
	bool endScreamAction = false;

	// ���������� ���� �ݶ��̴��� 2�� ������
	[SerializeField]
	EnemyAttackCollider[] BossAttackCollider;

	// ���̾ �߻� ��ġ
	public Transform FireBallPos;

	// �ó׸�ƽ ī�޶� ����
	public PlayableDirector BossDirector;
	public GameObject BossCameras;

	// ���ʽ��� �� 
	protected override void Start()
	{
		anim = GetComponent<Animator>();
		// ������ �÷��̾� Ž�� ���� ��ũ��Ʈ
		BossChecker = GetComponentInChildren<BossCheckPlayer>();
		BossChecker.Init(this);
		// ������ ���� �ݶ��̴�
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
		// ��� ���� Ȯ��
		CheckDieEnemy();
		// �÷��̾���� �Ÿ� Ȯ��
		CheckTargetDistance();
		// �ִϸ��̼� ���
		EnemyMovement();
		// ĵ���� ��ġ ����
		refreshCanvas();
	}

	// ���� ���� UI �ֽ�ȭ
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

		// Ÿ���� ���� ��
		if (Target != null)
		{
			// �÷��̾ ã�� ���� 1ȸ
			if(!isFind)
			{
				// ���¢��
				isFind = true;
				anim.SetTrigger("Scream");
				// ���¢�� �ִϸ��̼� üũ
				StartCoroutine(CheckScreamAction());
				Managers.Sound.BgmStop();
				return;
			}
			// ��ũ�� �ִϸ��̼��� ������ �ʾҴٸ� return
			if (!endScreamAction)
				return;

			// �����̰� �غ� �Ǿ��� ���� �������� �ƴ� ��
			if (curDelay > AttackDelay && !isAttack)
			{
				// �÷��̾ ��Ÿ� �ȿ� ���ٸ�
				if (AttackDistance < TargetDistance)
				{
					float val = Random.Range(0.0f, 99.0f);


					if (val < 50.0f)
					{
						// �̵����� ��ȯ
						AnimationState("Move");
						return;

					}
					else
					{
						AnimationState("FireBall");
						return;
					}


				}
				// �÷��̾ ��Ÿ� �ȿ� �����Ѵٸ�
				else
				{
					// ���� ���·� ��ȯ
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

	protected override void EnemyMove()
	{

		if (isAttack || isDie)
			return;

		if (isHit)
			return;

		// ���ʹ̿� Ÿ�� ���� �Ÿ� ����
		CheckTargetDistance();
		Vector3 Dir;

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
	// ���� ���¢�� �ִϸ��̼� ó��
	IEnumerator CheckScreamAction()
	{
		// ī�޶� ���� �׼��� ���� CameraMovement
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

					// ������ ĵ���� ActiveTrue
					BossCanvas.gameObject.SetActive(true);
					break;
				}
			}

			// ���� ��ũ���� ������ ����ߴٸ�
			// ī�޶� ���� �׼� ���
			if(isDie)
			{
				cameramove.isshake = false;
				break;
			}

		}
	}

	// ���� ������ ����
	void EnemyBasicAttack()
	{
		if (isDie || isAttack)
			return;
		Debug.Log("Call Basic Attack");

		// ������ ���·� ��ȯ
		isAttack = true;
		// ���ݾִϸ��̼� üũ ����
		CurCoroutine = StartCoroutine(BasicAttackCoroutine());

	}
	IEnumerator BasicAttackCoroutine()
	{
		yield return null;
		Managers.Sound.Play("Effect/Enemy/DragonAttack1");

		while (true)
		{
			yield return null;
			// ���� ������� �ִϸ��̼��� ������ �޾ƿ� �׼��� �������� Ȯ��
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
		// �ִϸ��̼��� �����ٸ� ������ �ʱ�ȭ, ���׼� 
		curDelay = 0.0f;
		AnimationState("Idle");
	}

	// ���� ���� ����
	void EnemyTailAttack()
	{
		if (isDie || isAttack)
			return;
		Debug.Log("Call Tail Attack");

		// ������ ���·� ��ȯ
		isAttack = true;
		BossAttackCollider[0].gameObject.SetActive(true);
		// ���ݾִϸ��̼� üũ ����
		CurCoroutine = StartCoroutine(TailAttackCoroutine());


	}
	IEnumerator TailAttackCoroutine()
	{
		yield return null;
		Managers.CallWaitForSeconds(0.1f, () => { Managers.Sound.Play("Effect/Enemy/DragonAttack2"); });

		while (true)
		{
			yield return null;
			// ���� ������� �ִϸ��̼��� ������ �޾ƿ� �׼��� �������� Ȯ��
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
		// �ִϸ��̼��� �����ٸ� ������ �ʱ�ȭ, ���׼� 
		curDelay = 0.0f;
		AnimationState("Idle");
	}

	void EnemyFireBall()
	{
		if (isDie || isAttack)
			return;
		Debug.Log("FireBall Attack");

		// ������ ���·� ��ȯ
		isAttack = true;
		// ���ݾִϸ��̼� üũ ����
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
			// ���� ������� �ִϸ��̼��� ������ �޾ƿ� �׼��� �������� Ȯ��
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
		// �ִϸ��̼��� �����ٸ� ������ �ʱ�ȭ, ���׼� 
		curDelay = 0.0f;
		AnimationState("Idle");

		Debug.Log("!!!EndFire!");
	}

	// �������� �ִϸ��̼�
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

		// Die�׼� 1ȸ ����� ���� ���� �׾��� �� Ȯ�� �� ���� �ʾҴٸ�
		// ���� ó�� �� �ִϸ��̼� ���
		if (isDie)
			return;

		// ��� �̿� ���� false
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

		// ������ ��� (�ּ� 1�� �ִ� 3�� ������ ���)
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


		// Ÿ��(�÷��̾�)����ġ �߰�
		Target.GetComponent<CharactorMovement>().AddExp(state.EXP);

		// ������� �ڷ�ƾ �ִٸ� ���߰�
		StopCoroutine(CurCoroutine);
		// ��� �ִϸ��̼� ���
		AnimationState("Die");
		GetComponent<Rigidbody>().useGravity = false;
		// �浹 �˻� ����
		GetComponent<BoxCollider>().enabled = false;
		// ���� ĵ���� ��Ȱ��ȭ
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
		// GreenDragon�� �������ͷ� �ϳ��� ������ ����
		EnemyState mystate = new EnemyState(Managers.GData.GreenDragonState);

		state = new(mystate);

		// ������ bool�� �ʱ�ȭ
		isAttack = false;
		isDie = false;
		isHit = false;
		isComeback = false;
		isFind = false;
		Target = null;


	}

	public override void CallHitEnemy(int Damage, bool critical)
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

		Vector3 SpawnPos = transform.position + new Vector3(0, 2, 0);

		GameObject damageUI = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Prefabs/UI/HitCanvas"));
		damageUI.GetComponent<HitCanvasUI>().Init(HitDamage, SpawnPos, critical);

		// ü���� 0���� �۾����� ����ó��
		if (State.Hp <= 0)
		{
			AnimationState("Die");
		}

		// �������� �ƴϸ鼭 ����ְ� ��ȿ�ִϸ��̼��� �������¶�� hit�ִϸ��̼� ���
		if (!isAttack && !isDie && endScreamAction)
		{
			AnimationState("Hit");
		}

	}


}
