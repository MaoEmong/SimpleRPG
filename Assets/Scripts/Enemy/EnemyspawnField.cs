using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ʹ� ��ȯ ��ũ��Ʈ
// ��ȯ�� ������ ������Ʈ
// ��ȯ ��ġ
// ��ȯ�� ����
public class EnemyspawnField : MonoBehaviour
{
	// ��ȯ�� ���ʹ� ������Ʈ
	public EnemyFSM OriginEnemy = null;
	// ����ִ� ���ʹ� ������ ����Ʈ
	public List<EnemyFSM> enemyList = new();
	// ���� ����
	[SerializeField]
	int MaxCount = 0;
	// ���� ������ ��
	[SerializeField]
	int curCount = 0;
	// ���� ���ʹ��� ����
	int Level = 0;

	// Y������ x,z�� ������ ���� ��ȯ
	float spawnMaxX;
	float spawnMinX;
	float spawnMaxZ;
	float spawnMinZ;

	private void Start()
	{
		// ��ȯ�� �� ����
		spawnMaxX = transform.position.x + 5.0f;
		spawnMinX = transform.position.x - 5.0f;
		spawnMaxZ = transform.position.z + 5.0f;
		spawnMinZ = transform.position.z - 5.0f;
		// ���ʹ� �ڵ� ���� �ڷ�ƾ
		StartCoroutine(checkEnemyCount());
	}

	public void Update()
	{
		// �������� Ȯ��
		checkEnemyDie();
	}

	void checkEnemyDie()
	{
		// ���ʹ� ����Ʈ�� �ִ� ��� ���ʹ� �˻�(�ڿ��� ����)
		for(int i = enemyList.Count - 1; i >= 0; i--)
		{
			// �׾��ִ� ��ü Ȯ��
			if (enemyList[i].isDie == true)
			{
				// Ǯ�Ŵ����� �ǵ���
			//	Managers.Pool.Push(enemyList[i].enemyUI.gameObject);
			//	Managers.Pool.Push(enemyList[i].gameObject);
				enemyList.Remove(enemyList[i]);
			}
		}
		curCount = enemyList.Count;
	}

	// ���ʹ� ��ü�� Ȯ�� �� �ڵ� ��ȯ
	IEnumerator checkEnemyCount()
	{
		float curTime = 0;
		// �ڵ����� ��ȯ�� �ð���
		int randval = Random.Range(1, 3);
		while (true)
		{
			yield return null;
			curTime += Time.deltaTime;
				// ������ ��ŭ�� �ð��� �帥 ��
				if(curTime > randval)
				{
					curCount = enemyList.Count;

					// ���� ��ü �� Ȯ��
					if (curCount < MaxCount)
					{
						// ���� �� �� ��ȯ
						Managers.CallWaitForSeconds(randval, () => { SpawnEnemy(); });
						// Ÿ�̸� ���� ����
						randval = Random.Range(1, 3);
					}
					// ���� �ð� �ʱ�ȭ
					curTime = 0;
				}
				
		}		
	}
	// �ʱ�ȭ ����
	public void Init(int level, int spawnCount = 3)
	{
		// �ִ� ���� �� ���Ѱ� ����
		MaxCount = spawnCount;
		// ��ȯ�� ���ʹ� ���� ����
		Level = level;
		// ������ ���ʹ� ������Ʈ Ǯ ����
		Managers.Pool.CreatePool(OriginEnemy.gameObject, MaxCount);
		
		// ���� ���� ���� ��ȯ ��ġ �缳��
		spawnMaxX = transform.position.x + 5.0f;
		spawnMinX = transform.position.x - 5.0f;
		spawnMaxZ = transform.position.z + 5.0f;
		spawnMinZ = transform.position.z - 5.0f;
		
		// ���� ����ŭ ��ȯ
		while (curCount < MaxCount)
			SpawnEnemy();
	}

	// ���ʹ� ��ȯ( ���� �߰� �� ���� �ش� ���� ������ �߰�����ߵ�!!!!!!!!!!!!!!!!)
	void SpawnEnemy()
	{
		// ��ȯ�� �ֳʹ� ��ü�� ���ٸ� ������� �� ��ȯ
		if (OriginEnemy == null)
		{
			Debug.Log("OriginObject is NULL!");
			return;
		}
		// ��ȯ�� ��ġ�� ���� �� ���ʹ� Ǯ�޴������� ������
		Vector3 EnemyPos = new Vector3(Random.Range(spawnMinX, spawnMaxX), transform.position.y, Random.Range(spawnMinZ, spawnMaxZ));
		GameObject obj = Managers.Pool.Pop(OriginEnemy.gameObject);
		obj.transform.position = EnemyPos;

		// ��ȯ�ϴ� ���ʹ��� Ÿ�Կ� ���� ���� �ʱ�ȭ
		switch(obj.GetComponent<EnemyFSM>().enemyType)
		{
			case Define.EnemyType.Unknown:
				Debug.Log("Error Type is Unknown!");
				break;
			case Define.EnemyType.Slime:
				var slime = obj.GetComponent<EnemySlime>();
				slime.Init(EnemyPos, Level);
				break;
			case Define.EnemyType.Mushroom:
				var mushroom = obj.GetComponent<EnemyMushroom>();
				mushroom.Init(EnemyPos, Level);
				break;
			case Define.EnemyType.NeedleSnail:
				var needlesnail = obj.GetComponent<EnemyNeedleSnail>();
				needlesnail.Init(EnemyPos, Level);
				break;
			case Define.EnemyType.Cactus:
				var cactus = obj.GetComponent<EnemyCactus>();
				cactus.Init(EnemyPos, Level);
				break;
			case Define.EnemyType.Beetle:
				var beetle = obj.GetComponent<EnemyBeetle>();
				beetle.Init(EnemyPos, Level);
				break;

		}

		// ���� �� ������ ���� ���ʹ� ����Ʈ�� ���
		enemyList.Add(obj.GetComponent<EnemyFSM>());
		curCount++;
	}

}
