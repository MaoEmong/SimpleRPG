using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 획득 아이템 정보 출력 UI
public class GetItemType : MonoBehaviour
{
	// 획득아이템 이미지
	[SerializeField]
	Image ItemImage;
	// 획득 아이템 이름
	[SerializeField]
	Text ItemName;

	// 초기화 : 아이템 코드를 받아옴
	public void Init(int itemCode, Vector3 TargetPos)
	{
		gameObject.SetActive(true);

		// 받아온 아이템 코드를 기반으로 이미지와 이름 설정
		ItemImage.sprite = Managers.Resource.Load<Sprite>($"Sprites/UI/InvenIcon/{itemCode}");
		ItemName.text = $"{Managers.GData.ItemData[itemCode].ItemName}";


		StartCoroutine(StartAction());
	}

	// 간단한 이동 액션
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
