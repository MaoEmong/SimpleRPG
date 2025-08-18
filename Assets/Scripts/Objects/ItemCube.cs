using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터가 드랍하는 아이템 큐브
public class ItemCube : MonoBehaviour
{
	// 해당 큐브가 가질 아이템 코드값
	public int ItemCode;
	// 중력 사용을 위한 리지드바디
	Rigidbody rigid;
	// 플레이어 충돌검사용 콜라이더
	SphereCollider checker;

	private void Start()
	{
		// 리지드바디는 현재 오브젝트에서
		rigid = GetComponent<Rigidbody>();
		// 콜라이더는 자식오브젝트에서 가져옴
		checker = GetComponentInChildren<SphereCollider>();
	}

	public void Init(int itemCode)
	{
		Managers.Sound.Play("Effect/Object/DropItem");

		ItemCode = itemCode;

		float randX = Random.Range(0.8f, 1.0f);
		float randZ = Random.Range(0.8f, 1.0f);

		// 랜덤한 방향으로 힘을줘 위로 튕겨내는듯한 액션 제작
		Vector3 Dir = new Vector3(randX, 1, randZ);
		// Dir = Dir.normalized;

		rigid = GetComponent<Rigidbody>();
		checker = GetComponentInChildren<SphereCollider>();

		checker.enabled = false;
		Managers.CallWaitForSeconds(0.2f, () => { checker.enabled = true; });
		
		rigid.AddForce(Dir,ForceMode.Impulse);

	}


}
