using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 인벤토리 스크립트
public class InvenPanel : MonoBehaviour
{
	// 아이템의 상세 정보
	public Image RightImage;
	public Text rightName;
	public Text rightExplan;
	public Text rightCount;
	// 아이템 슬롯 자동 정렬
	public GameObject Container;
	// 소지금
	public Text MoneyText;
	// 판매 금액 텍스트
	public Text SellPrice;

	public int ItemCode = 0;

	// 상점 오픈 시 판매버튼
	public Button MySellButton;
	// 오픈타입 확인(상점오픈 or 인벤오픈)
	public bool isSell = false;

	// 인벤 UI 오픈 시 마다 호출
	private void OnEnable()
	{
		// 기존의 데이터 삭제
		foreach(Transform child in Container.transform)
		{
			Destroy(child.gameObject);
		}
		// 기존 데이터 초기화
		RightImage.sprite= null;
		rightName.text = "";
		rightExplan.text = "";
		SellPrice.text = "";
		MoneyText.text = $"골드 : {Managers.Player.GetComponent<CharactorState>().Money}";
		// 현재 인벤토리 데이터에 존재하는 아이템 수만큼 아이템 슬롯 생성
		for (int i = 0; i < Managers.GData.PlayerItemData.Count;i++)
		{
			if (Managers.GData.PlayerItemData[i].Second <= 0)
			{
				continue;
			}
			GameObject ItemSlots = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Prefabs/UI/Slot"),Container.transform);
			ItemSlots.GetComponent<InvenSlot>().Init(this, i, Managers.GData.PlayerItemData[i].Second, RightImage, rightName, rightExplan, rightCount, SellPrice);
		}

		if(isSell)
		{
			MySellButton.gameObject.SetActive(true);
		}
		else
		{
			MySellButton.gameObject.SetActive(false);
		}
	}

	// 판매 버튼
	public void SellButton()
	{
		Managers.Sound.Play("Effect/UI/LimitButton");
		// 선택한 아이템을 전부 판매
		Managers.Player.GetComponent<CharactorState>().Money += (Managers.GData.PlayerItemData[ItemCode].Second * Managers.GData.ItemData[ItemCode].Sell);
		Managers.GData.PlayerItemData[ItemCode].Second = 0;

		// 인벤토리 리스트 초기화
		foreach (Transform child in Container.transform)
		{
			Destroy(child.gameObject);
		}
		RightImage.sprite = null;
		rightName.text = "";
		rightExplan.text = "";
		rightCount.text = "";
		SellPrice.text = "";
		MoneyText.text = $"골드 : {Managers.Player.GetComponent<CharactorState>().Money}";
		for (int i = 0; i < Managers.GData.PlayerItemData.Count; i++)
		{
			if (Managers.GData.PlayerItemData[i].Second <= 0)
			{
				continue;
			}
			GameObject ItemSlots = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Prefabs/UI/Slot"), Container.transform);
			ItemSlots.GetComponent<InvenSlot>().Init(this, i, Managers.GData.PlayerItemData[i].Second, RightImage, rightName, rightExplan, rightCount, SellPrice);
		}

	}

	public void Close()
	{
		isSell = false;
	}

}
