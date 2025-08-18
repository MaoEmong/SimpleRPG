using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �κ��丮 ��ũ��Ʈ
public class InvenPanel : MonoBehaviour
{
	// �������� �� ����
	public Image RightImage;
	public Text rightName;
	public Text rightExplan;
	public Text rightCount;
	// ������ ���� �ڵ� ����
	public GameObject Container;
	// ������
	public Text MoneyText;
	// �Ǹ� �ݾ� �ؽ�Ʈ
	public Text SellPrice;

	public int ItemCode = 0;

	// ���� ���� �� �ǸŹ�ư
	public Button MySellButton;
	// ����Ÿ�� Ȯ��(�������� or �κ�����)
	public bool isSell = false;

	// �κ� UI ���� �� ���� ȣ��
	private void OnEnable()
	{
		// ������ ������ ����
		foreach(Transform child in Container.transform)
		{
			Destroy(child.gameObject);
		}
		// ���� ������ �ʱ�ȭ
		RightImage.sprite= null;
		rightName.text = "";
		rightExplan.text = "";
		SellPrice.text = "";
		MoneyText.text = $"��� : {Managers.Player.GetComponent<CharactorState>().Money}";
		// ���� �κ��丮 �����Ϳ� �����ϴ� ������ ����ŭ ������ ���� ����
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

	// �Ǹ� ��ư
	public void SellButton()
	{
		Managers.Sound.Play("Effect/UI/LimitButton");
		// ������ �������� ���� �Ǹ�
		Managers.Player.GetComponent<CharactorState>().Money += (Managers.GData.PlayerItemData[ItemCode].Second * Managers.GData.ItemData[ItemCode].Sell);
		Managers.GData.PlayerItemData[ItemCode].Second = 0;

		// �κ��丮 ����Ʈ �ʱ�ȭ
		foreach (Transform child in Container.transform)
		{
			Destroy(child.gameObject);
		}
		RightImage.sprite = null;
		rightName.text = "";
		rightExplan.text = "";
		rightCount.text = "";
		SellPrice.text = "";
		MoneyText.text = $"��� : {Managers.Player.GetComponent<CharactorState>().Money}";
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
