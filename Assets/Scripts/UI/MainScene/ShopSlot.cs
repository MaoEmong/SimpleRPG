using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ���� ������ ���� ��ũ��Ʈ
public class ShopSlot : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
	// ���� ������Ʈ
	public ShopPanel MyParant;
	// �Ǹ��� ������ �ڵ�
	public int ItemCode;
	// ������ ������
	public GameData.ItemClass Item;

	// ������ ���� ��¿�
	public Text ItemName;
	public Text ItemExplan;
	public Text PriceText;


	public void Init(ShopPanel parant, int itemcode)
	{
		MyParant = parant;

		ItemCode = itemcode;

		Item = Managers.GData.ItemData[ItemCode];

		string jsondata = Managers.Json.ObjectToJson(Item);
		Debug.Log(jsondata);

		ItemName.text = $"{Item.ItemName}";
		ItemExplan.text = $"{Item.ItemExplan}";
		PriceText.text = $"{Item.Sell}���";
	}

	public void OnPointerDown(PointerEventData eventData)
	{
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		CharactorState player = Managers.Player.GetComponent<CharactorState>();

		if(player.Money>=Item.Sell)
		{
			Managers.GData.PlayerItemData[ItemCode].Second++;
			player.Money -= Item.Sell;
			MyParant.ShopText.text = "���� �ŷ�����.";
			Managers.Sound.Play("Effect/UI/BuyItem");
		}
		else
		{
			MyParant.ShopText.text = "��尡 �����ϱ�.";
			Managers.Sound.Play("Effect/UI/NotAccess");
		}

	}
}
