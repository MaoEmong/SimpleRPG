using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// NPC대화문 설정
public class NPCUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	// 해당 UI를 가지는 NPC데이터
	NPCInteration MyParant = null;

	// Dialog 목록
	[SerializeField]
	List<string> DialogList = new();

	// NPC이름
	[SerializeField]
	Text Name;
	// NPC가 하려는 텍스트
	[SerializeField]
	Text Dialog;

	// 인덱스 번호는 1부터 시작(0은 이름)
	int index = 1;

	public Image BackGroundImage;
	public GameObject BackPanel;

	bool isTalk = true;

	public InvenPanel Inven;

	public GameObject BuyItemPanel;

	public Button BuyItem;
	public Button SellItem;
	public Button Close;
	public GameObject FakeCloseButton;

	public GameObject UpgradePanel;

	private void OnEnable()
	{
		Dialog.text = DialogList[1];
		isTalk = true;
		BuyItemPanel.SetActive(false);
		BuyItem.gameObject.SetActive(false);
		SellItem.gameObject.SetActive(false);
		Close.gameObject.SetActive(false);
		FakeCloseButton.gameObject.SetActive(false);
		BackPanel.gameObject.SetActive(true);
		UpgradePanel.gameObject.SetActive(false);
	}


	public void Init(NPCInteration parant)
	{
		BackGroundImage.enabled = true;
		BackPanel.SetActive(true);
		FakeCloseButton.SetActive(false);
		MyParant = parant;
		DialogList = Managers.Json.ImportdialogJsonData<List<string>>($"NPCDialog{MyParant.NPCCode}");
		
		isTalk = true;
		Name.text = DialogList[0];
		Dialog.text = DialogList[1];
		index = 1;
	}

	public void OnPointerDown(PointerEventData eventData)
	{

	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (isTalk)
		{
			index++;
			if (index == DialogList.Count)
			{
				// 대화전용 NPC
				if (MyParant.type == Define.NPCType.Talk)
				{
					index = 1;
					MyParant.EndNpcTalk();
					return;
				}
				// 상점 NPC
				else if (MyParant.type == Define.NPCType.Shop)
				{
					BuyItem.gameObject.SetActive(true);
					SellItem.gameObject.SetActive(true);
					Close.gameObject.SetActive(true);
					isTalk = false;
					return;
				}
				// 강화 NPC
				else if(MyParant.type == Define.NPCType.Upgrade)
				{
					UpgradePanel.SetActive(true);

					FakeCloseButton.SetActive(true);
					isTalk = false;
					return;

				}
			}
			Dialog.text = DialogList[index];
		}
	}

	//========================================아래 상점NPC용 콜백

	public void CloseButton()
	{
		BuyItemPanel.SetActive(false);
		BuyItem.gameObject.SetActive(false);
		SellItem.gameObject.SetActive(false);
		Close.gameObject.SetActive(false);
		BackPanel.gameObject.SetActive(false);
		FakeCloseButton.SetActive(true);

		Inven.isSell = false;
		Inven.gameObject.SetActive(false);

		MyParant.EndNpcTalk();

	}

	public void SellButton()
	{
		BuyItemPanel.SetActive(false);
		BuyItem.gameObject.SetActive(false);
		SellItem.gameObject.SetActive(false);
		Close.gameObject.SetActive(false);
		BackPanel.gameObject.SetActive(false);
		FakeCloseButton.gameObject.SetActive(true);
		BackGroundImage.enabled = false;

		Inven.isSell = true;
		Inven.gameObject.SetActive(true);

		Managers.Sound.Play("Effect/NPC/Shop1");
	}

	public void BuyButton()
	{
		BuyItem.gameObject.SetActive(false);
		SellItem.gameObject.SetActive(false);
		Close.gameObject.SetActive(false);
		BackPanel.gameObject.SetActive(false);
		FakeCloseButton.gameObject.SetActive(true);
		BackGroundImage.enabled = false;
		Inven.gameObject.SetActive(false);

		BuyItemPanel.SetActive(true);
		Managers.Sound.Play("Effect/NPC/Shop2");

	}
}
