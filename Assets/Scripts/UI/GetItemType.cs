using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ȹ�� ������ ���� ��� UI
public class GetItemType : MonoBehaviour
{
	// ȹ������� �̹���
	[SerializeField]
	Image ItemImage;
	// ȹ�� ������ �̸�
	[SerializeField]
	Text ItemName;

	// �ʱ�ȭ : ������ �ڵ带 �޾ƿ�
	public void Init(int itemCode, Vector3 TargetPos)
	{
		gameObject.SetActive(true);

		// �޾ƿ� ������ �ڵ带 ������� �̹����� �̸� ����
		ItemImage.sprite = Managers.Resource.Load<Sprite>($"Sprites/UI/InvenIcon/{itemCode}");
		ItemName.text = $"{Managers.GData.ItemData[itemCode].ItemName}";


		StartCoroutine(StartAction());
	}

	// ������ �̵� �׼�
	IEnumerator StartAction()
	{
		yield return null;

		float endTime = 1.3f;
		float curTime = 0;

		Vector3 Dir = Vector3.up*30;

		float speed = 3.0f;

		while(curTime < endTime)
		{
			yield return null;
			curTime += Time.deltaTime;

			transform.position += (Dir * Time.deltaTime * speed);

		}

		Managers.Pool.Push(this.gameObject);

	}


}
