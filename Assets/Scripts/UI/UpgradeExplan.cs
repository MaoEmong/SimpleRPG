using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeExplan : MonoBehaviour
{
	public Text UpgradeType;
	public Text PresentCount;
	public Text PresentDamage;
	public Text UpgradePer;
	public Text FutureCount;
	public Text FutureDamage;
	public Text UpgradeGold;
	public Text PlayerGold;

	public Image MessageImage;
	public Text MessageText;

	public UpgradeSlot Slot = null;
	int upgradeCount;
	float upgradeper;

	CharactorState Player;

	private void OnEnable()
	{
		Player = Managers.Player.GetComponent<CharactorState>();
		Slot = null;
		upgradeper = 0.0f;
		UpgradeType.text = "강화";
		PlayerGold.text = $"소지골드 : {Player.Money}G";
		PresentCount.enabled = false;
		PresentDamage.enabled = false;
		UpgradePer.enabled = false;
		FutureCount.enabled = false;
		FutureDamage.enabled = false;
		UpgradeGold.enabled = false;
		MessageImage.gameObject.SetActive(false);
	}

	// 강화할 장비 설정(현재 무기 강화만 존재)
	public void SetUpgradeSlot(UpgradeSlot slot)
	{
		Slot = slot;
		PresentCount.enabled = true;
		PresentDamage.enabled = true;
		UpgradePer.enabled = true;
		FutureCount.enabled = true;
		FutureDamage.enabled = true;
		UpgradeGold.enabled = true;
		
		// 선택한 장비
		switch (slot.type)
		{
			case Define.UpgradeType.Weapon:
				UpgradeType.text = "무기 강화";
				upgradeCount = Managers.GData.playerinfo.UpgradeWeapon;
				upgradeper = 1 + (99 - (upgradeCount * 8.33f));
				if (upgradeper <= 0)
					upgradeper = 0.5f;
				break;
		}
		// 현재 강화수치에 따른 강화 후 능력치 표기
		if(upgradeCount >= 8)
		{
			PresentCount.text = $"{upgradeCount}강";
			FutureCount.text = $"최대치";
			PresentDamage.text = $"{upgradeCount * 5}데미지";
			FutureDamage.text = $"";
			UpgradePer.text = $"";
			UpgradeGold.text = $"";

		}
		else
		{
			PresentCount.text = $"{upgradeCount}강";
			FutureCount.text = $"{upgradeCount + 1}강";
			PresentDamage.text = $"{upgradeCount * 5}데미지";
			FutureDamage.text = $"{(upgradeCount + 1) * 5} 데미지";
			UpgradePer.text = $"{upgradeper.ToString("F2")} %";
			UpgradeGold.text = $"소모골드 : {upgradeCount * 50}G";

		}
	}

	void RefreshText()
	{
		upgradeCount = Managers.GData.playerinfo.UpgradeWeapon;
		upgradeper = 1 + (99 - (upgradeCount * 8.33f));
		if (upgradeper <= 0)
			upgradeper = 0.5f;

		if (upgradeCount >= 8)
		{
			PresentCount.text = $"{upgradeCount}강";
			FutureCount.text = $"최대치";
			PresentDamage.text = $"{upgradeCount * 5}데미지";
			FutureDamage.text = $"";
			UpgradePer.text = $"";
			UpgradeGold.text = $"";

		}
		else
		{
			PresentCount.text = $"{upgradeCount}강";
			FutureCount.text = $"{upgradeCount + 1}강";
			PresentDamage.text = $"{upgradeCount * 5}데미지";
			FutureDamage.text = $"{(upgradeCount + 1) * 5} 데미지";
			UpgradePer.text = $"{upgradeper.ToString("F2")} %";
			UpgradeGold.text = $"소모골드 : {upgradeCount * 50}G";

		}
		PlayerGold.text = $"소지골드 : {Player.Money}G";
	}

	// 강화 버튼
	public void CallUpgradeButton()
	{
		// 안내메세지가 아직 존재한다면 대기
		if(MessageImage.gameObject.activeSelf)
		{
			return;
		}
		// 선택한 목록이 없다면 return
		if(Slot == null)
		{
			Managers.Sound.Play("Effect/UI/LimitButton");
			MessageImage.gameObject.SetActive(true);
			MessageText.text = "강화 목록을 설정해주세요!";
			StartCoroutine(MyTools.ImageFadeOut(MessageImage,1.0f));
			Managers.CallWaitForSeconds(1.0f, () => { MessageImage.gameObject.SetActive(false); });
			return;
		}
		// 선택한 장비의 강화수치가 최대라면 return
		if (Managers.GData.playerinfo.UpgradeWeapon >= 8)
		{
			Managers.Sound.Play("Effect/UI/LimitButton");
			MessageImage.gameObject.SetActive(true);
			MessageText.text = "강화수치가 최대 입니다!!";
			StartCoroutine(MyTools.ImageFadeOut(MessageImage, 1.0f));
			Managers.CallWaitForSeconds(1.0f, () => { MessageImage.gameObject.SetActive(false); });
			return;
		}
		// 소지금이 부족하다면 return
		if (Player.Money < upgradeCount* 50)
		{
			Managers.Sound.Play("Effect/UI/LimitButton");

			MessageImage.gameObject.SetActive(true);
			MessageText.text = "소지골드가 부족합니다!";
			StartCoroutine(MyTools.ImageFadeOut(MessageImage, 1.0f));
			Managers.CallWaitForSeconds(1.0f, () => { MessageImage.gameObject.SetActive(false); });
			return;
		}
		// 지정된 확률에 따라 강화 성공/실패
		else
		{
			Player.Money -= upgradeCount* 50;
			float val = Random.Range(1, 99.9f);
			// 강화 성공
			if(val < upgradeper)
			{
				MessageImage.gameObject.SetActive(true);
				MessageText.text = "강화 성공!";
				StartCoroutine(MyTools.ImageFadeOut(MessageImage, 1.0f));
				Managers.CallWaitForSeconds(1.0f, () => { MessageImage.gameObject.SetActive(false); });
				Managers.GData.playerinfo.UpgradeWeapon++;
				Slot.UpgradeCount.text = $"{Managers.GData.playerinfo.UpgradeWeapon}강";
				Managers.Sound.Play("Effect/UI/UpgradeSuccess");
			}
			// 강화 실패
			else
			{
				MessageImage.gameObject.SetActive(true);
				MessageText.text = "강화 실패!";
				StartCoroutine(MyTools.ImageFadeOut(MessageImage, 1.0f));
				Managers.CallWaitForSeconds(1.0f, () => { MessageImage.gameObject.SetActive(false); });
				Managers.Sound.Play("Effect/UI/UpgradeFail");

			}
			// 텍스트 최신화
			RefreshText();
		}

	}
}
