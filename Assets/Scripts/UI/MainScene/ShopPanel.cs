using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ����â ��ũ��Ʈ
public class ShopPanel : MonoBehaviour
{
    public Text ShopText;

    public ShopSlot ShopSlot;

    public Text playerMoneyText;
    public CharactorState player;

    // 15�� - ����
    // ���� ���� �� ���� �߰��� ������ ����
    void Start()
    {
        ShopText.text = "������ ������ ��󺸰�.";
        ShopSlot.Init(this,15);
        player = Managers.Player.GetComponent<CharactorState>();
    }

    private void Update()
    {
        playerMoneyText.text = $"{player.Money} ���";
    }

}
