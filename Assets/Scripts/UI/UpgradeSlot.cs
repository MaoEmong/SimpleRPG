using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour, IPointerUpHandler,IPointerDownHandler
{
    public Text UpgradeCount;

    public UpgradeExplan explan;

    public Define.UpgradeType type;

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        explan.SetUpgradeSlot(this);
        Managers.Sound.Play("Effect/UI/UIClick");
    }

    private void Update()
    {
        UpgradeCount.text = $"{Managers.GData.playerinfo.UpgradeWeapon}°­";
    }

}
