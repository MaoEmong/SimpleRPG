using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// NPC��ȭ�� ����
public class NPCUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	// �ش� UI�� ������ NPC������
	NPCInteration MyParant = null;

	// Dialog ���
	[SerializeField]
	List<string> DialogList = new();

	// NPC�̸�
	[SerializeField]
	Text Name;
	// NPC�� �Ϸ��� �ؽ�Ʈ
	[SerializeField]
	Text Dialog;

	// �ε��� ��ȣ�� 1���� ����(0�� �̸�)
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
				// ��ȭ���� NPC
				if (MyParant.type == Define.NPCType.Talk)
				{
					index = 1;
					MyParant.EndNpcTalk();
					return;
				}
				// ���� NPC
				else if (MyParant.type == Define.NPCType.Shop)
				{
					BuyItem.gameObject.SetActive(true);
					SellItem.gameObject.SetActive(true);
					Close.gameObject.SetActive(true);
					isTalk = false;
					return;
				}
				// ��ȭ NPC
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

	//========================================�Ʒ� ����NPC�� �ݹ�

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
