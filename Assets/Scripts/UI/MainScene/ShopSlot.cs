using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 상점 아이템 슬롯 스크립트
public class ShopSlot : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
	// 상점 오브젝트
	public ShopPanel MyParant;
	// 판매할 아이템 코드
	public int ItemCode;
	// 아이템 데이터
	public GameData.ItemClass Item;

	// 아이템 정보 출력용
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
		PriceText.text = $"{Item.Sell}골드";
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
			MyParant.ShopText.text = "좋은 거래였네.";
			Managers.Sound.Play("Effect/UI/BuyItem");
		}
		else
		{
			MyParant.ShopText.text = "골드가 부족하군.";
			Managers.Sound.Play("Effect/UI/NotAccess");
		}

	}
}
