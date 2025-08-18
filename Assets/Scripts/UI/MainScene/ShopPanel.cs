using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 구매창 스크립트
public class ShopPanel : MonoBehaviour
{
    public Text ShopText;

    public ShopSlot ShopSlot;

    public Text playerMoneyText;
    public CharactorState player;

    // 15번 - 물약
    // 현재 물약 외 따로 추가된 아이템 없음
    void Start()
    {
        ShopText.text = "사고싶은 물건을 골라보게.";
        ShopSlot.Init(this,15);
        player = Managers.Player.GetComponent<CharactorState>();
    }

    private void Update()
    {
        playerMoneyText.text = $"{player.Money} 골드";
    }

}
