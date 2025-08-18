using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// �κ��丮 �� ������ ���� ��ũ��Ʈ
public class InvenSlot : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
	InvenPanel MyParant;

	int ItemCode;
	public Image itemImage;
	public Text ItemCount;

	public Image RightImage;
	public Text RightName;
	public Text RightPlan;
	public Text RightCount;
	public Text RightPrice;

	public void Init(InvenPanel parant, int itemcode, int itemcount, Image rightimage, Text rightname, Text rightplan, Text rightcount, Text sellprice)
	{
		MyParant = parant;
		ItemCode = itemcode;
		ItemCount.text = $"X{itemcount}";
		itemImage.sprite = Managers.Resource.Load<Sprite>($"Sprites/UI/InvenIcon/{itemcode}");

		RightImage = rightimage;
		RightName = rightname;
		RightPlan = rightplan;
		RightCount = rightcount;
		RightPrice = sellprice;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		Managers.Sound.Play("Effect/UI/UIClick");

		RightImage.sprite = itemImage.sprite;
		RightName.text = $"{Managers.GData.ItemData[ItemCode].ItemName}";
		RightPlan.text = $"{Managers.GData.ItemData[ItemCode].ItemExplan}";
		RightCount.text = $"X{Managers.GData.PlayerItemData[ItemCode].Second}";
		RightPrice.text = $"�ǸŰ� : {Managers.GData.ItemData[ItemCode].Sell}";
		MyParant.ItemCode = ItemCode;
	}
}
