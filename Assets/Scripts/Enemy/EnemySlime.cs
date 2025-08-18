using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EnemyFSM�� ��ӹ޴� Slime
public class EnemySlime : EnemyFSM
{
    // �Ϲݻ��¸� ���׸���
    [SerializeField]
    Material OriginMat;
    // ���� �޾����� ���׸���
    [SerializeField]
    Material HitMat;
    // ���׸��� ��ü�� Renderer
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

    // ���� �޾��� ��
    protected override void EnemyHit()
    {
        // ���� ���
        if (!isHit)
        {
            Managers.Sound.Play("Effect/Enemy/SlimeHitVoice");
		}
		base.EnemyHit();
        // �������̰ų� �������°� �ƴ϶��
		if (isAttack || isDie)
			return;
        // ��Ʈ ���׸���� ��ü
        meshrender.material = HitMat;
        // ���� �Ϲݻ��� ���׸���� �ٽ� ��ü
        Managers.CallWaitForSeconds(0.1f, () => { meshrender.material = OriginMat; });
        

	}

    // ���� �ʱ�ȭ
	public void Init(Vector3 comeback, int level)
    {
        // ������ ������ ����
        int lev = 0;
        if (level <= 0)
        {
            lev = Random.Range(1, Managers.GData.SlimeBasicState.Count+1);
        }
        else
            lev = level;

		// ������ ���� �����͸� ������� ���� ���� �´� �������ͽ� �����͸� ����
		EnemyState mystate = new EnemyState(Managers.GData.SlimeBasicState[lev - 1]);

        base.Init(comeback, mystate);

	}
    // ���� ���
    protected override void EnemyDie()
    {

		// Die�׼� 1ȸ ����� ���� ���� �׾��� �� Ȯ�� �� ���� �ʾҴٸ�
		// ���� ó�� �� �ִϸ��̼� ���
		if (isDie)
			return;

        // ��� ����
        Managers.Sound.Play("Effect/Enemy/SlimeDie");

        // �÷��̾��� ������ �ش� ������ ų ī��Ʈ �߰�
        Managers.GData.PlayerFeat[0].AddCount();

		// ��� �̿� ���� false
		isDie = true;
		isHit = false;
		isAttack = false;

        // �����ϰ� ������ ���
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
