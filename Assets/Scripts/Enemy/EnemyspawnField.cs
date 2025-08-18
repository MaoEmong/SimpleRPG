using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 에너미 소환 스크립트
// 소환할 오리진 오브젝트
// 소환 위치
// 소환할 물량
public class EnemyspawnField : MonoBehaviour
{
	// 소환할 에너미 오브젝트
	public EnemyFSM OriginEnemy = null;
	// 살아있는 에너미 관리용 리스트
	public List<EnemyFSM> enemyList = new();
	// 생성 제한
	[SerializeField]
	int MaxCount = 0;
	// 현재 생성된 수
	[SerializeField]
	int curCount = 0;
	// 생성 에너미의 레벨
	int Level = 0;

	// Y값고정 x,z값 조절로 랜덤 소환
	float spawnMaxX;
	float spawnMinX;
	float spawnMaxZ;
	float spawnMinZ;

	private void Start()
	{
		// 소환할 값 설정
		spawnMaxX = transform.position.x + 5.0f;
		spawnMinX = transform.position.x - 5.0f;
		spawnMaxZ = transform.position.z + 5.0f;
		spawnMinZ = transform.position.z - 5.0f;
		// 에너미 자동 스폰 코루틴
		StartCoroutine(checkEnemyCount());
	}

	public void Update()
	{
		// 생존여부 확인
		checkEnemyDie();
	}

	void checkEnemyDie()
	{
		// 에너미 리스트에 있는 모든 에너미 검사(뒤에서 부터)
		for(int i = enemyList.Count - 1; i >= 0; i--)
		{
			// 죽어있는 객체 확인
			if (enemyList[i].isDie == true)
			{
				// 풀매니저로 되돌림
			//	Managers.Pool.Push(enemyList[i].enemyUI.gameObject);
			//	Managers.Pool.Push(enemyList[i].gameObject);
				enemyList.Remove(enemyList[i]);
			}
		}
		curCount = enemyList.Count;
	}

	// 에너미 객체수 확인 후 자동 소환
	IEnumerator checkEnemyCount()
	{
		float curTime = 0;
		// 자동으로 소환할 시간값
		int randval = Random.Range(1, 3);
		while (true)
		{
			yield return null;
			curTime += Time.deltaTime;
				// 설정한 만큼의 시간이 흐른 후
				if(curTime > randval)
				{
					curCount = enemyList.Count;

					// 현재 객체 수 확인
					if (curCount < MaxCount)
					{
						// 부족 할 시 소환
						Managers.CallWaitForSeconds(randval, () => { SpawnEnemy(); });
						// 타이머 랜덤 설정
						randval = Random.Range(1, 3);
					}
					// 현재 시간 초기화
					curTime = 0;
				}
				
		}		
	}
	// 초기화 설정
	public void Init(int level, int spawnCount = 3)
	{
		// 최대 생성 수 제한값 설정
		MaxCount = spawnCount;
		// 소환할 에너미 레벨 설정
		Level = level;
		// 생성할 에너미 오브젝트 풀 생성
		Managers.Pool.CreatePool(OriginEnemy.gameObject, MaxCount);
		
		// 에러 방지 위해 소환 위치 재설정
		spawnMaxX = transform.position.x + 5.0f;
		spawnMinX = transform.position.x - 5.0f;
		spawnMaxZ = transform.position.z + 5.0f;
		spawnMinZ = transform.position.z - 5.0f;
		
		// 제한 값만큼 소환
		while (curCount < MaxCount)
			SpawnEnemy();
	}

	// 에너미 소환( 몬스터 추가 시 마다 해당 몬스터 데이터 추가해줘야됨!!!!!!!!!!!!!!!!)
	void SpawnEnemy()
	{
		// 소환할 애너미 객체가 없다면 에러출력 후 반환
		if (OriginEnemy == null)
		{
			Debug.Log("OriginObject is NULL!");
			return;
		}
		// 소환할 위치값 설정 후 에너미 풀메니저에서 가져옴
		Vector3 EnemyPos = new Vector3(Random.Range(spawnMinX, spawnMaxX), transform.position.y, Random.Range(spawnMinZ, spawnMaxZ));
		GameObject obj = Managers.Pool.Pop(OriginEnemy.gameObject);
		obj.transform.position = EnemyPos;

		// 소환하는 에너미의 타입에 따라 각각 초기화
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

		// 생성 후 관리를 위해 에너미 리스트에 담기
		enemyList.Add(obj.GetComponent<EnemyFSM>());
		curCount++;
	}

}
