using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���Ͱ� ����ϴ� ������ ť��
public class ItemCube : MonoBehaviour
{
	// �ش� ť�갡 ���� ������ �ڵ尪
	public int ItemCode;
	// �߷� ����� ���� ������ٵ�
	Rigidbody rigid;
	// �÷��̾� �浹�˻�� �ݶ��̴�
	SphereCollider checker;

	private void Start()
	{
		// ������ٵ�� ���� ������Ʈ����
		rigid = GetComponent<Rigidbody>();
		// �ݶ��̴��� �ڽĿ�����Ʈ���� ������
		checker = GetComponentInChildren<SphereCollider>();
	}

	public void Init(int itemCode)
	{
		Managers.Sound.Play("Effect/Object/DropItem");

		ItemCode = itemCode;

		float randX = Random.Range(0.8f, 1.0f);
		float randZ = Random.Range(0.8f, 1.0f);

		// ������ �������� ������ ���� ƨ�ܳ��µ��� �׼� ����
		Vector3 Dir = new Vector3(randX, 1, randZ);
		// Dir = Dir.normalized;

		rigid = GetComponent<Rigidbody>();
		checker = GetComponentInChildren<SphereCollider>();

		checker.enabled = false;
		Managers.CallWaitForSeconds(0.2f, () => { checker.enabled = true; });
		
		rigid.AddForce(Dir,ForceMode.Impulse);

	}


}
