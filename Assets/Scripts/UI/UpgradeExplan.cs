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
		UpgradeType.text = "��ȭ";
		PlayerGold.text = $"������� : {Player.Money}G";
		PresentCount.enabled = false;
		PresentDamage.enabled = false;
		UpgradePer.enabled = false;
		FutureCount.enabled = false;
		FutureDamage.enabled = false;
		UpgradeGold.enabled = false;
		MessageImage.gameObject.SetActive(false);
	}

	// ��ȭ�� ��� ����(���� ���� ��ȭ�� ����)
	public void SetUpgradeSlot(UpgradeSlot slot)
	{
		Slot = slot;
		PresentCount.enabled = true;
		PresentDamage.enabled = true;
		UpgradePer.enabled = true;
		FutureCount.enabled = true;
		FutureDamage.enabled = true;
		UpgradeGold.enabled = true;
		
		// ������ ���
		switch (slot.type)
		{
			case Define.UpgradeType.Weapon:
				UpgradeType.text = "���� ��ȭ";
				upgradeCount = Managers.GData.playerinfo.UpgradeWeapon;
				upgradeper = 1 + (99 - (upgradeCount * 8.33f));
				if (upgradeper <= 0)
					upgradeper = 0.5f;
				break;
		}
		// ���� ��ȭ��ġ�� ���� ��ȭ �� �ɷ�ġ ǥ��
		if(upgradeCount >= 8)
		{
			PresentCount.text = $"{upgradeCount}��";
			FutureCount.text = $"�ִ�ġ";
			PresentDamage.text = $"{upgradeCount * 5}������";
			FutureDamage.text = $"";
			UpgradePer.text = $"";
			UpgradeGold.text = $"";

		}
		else
		{
			PresentCount.text = $"{upgradeCount}��";
			FutureCount.text = $"{upgradeCount + 1}��";
			PresentDamage.text = $"{upgradeCount * 5}������";
			FutureDamage.text = $"{(upgradeCount + 1) * 5} ������";
			UpgradePer.text = $"{upgradeper.ToString("F2")} %";
			UpgradeGold.text = $"�Ҹ��� : {upgradeCount * 50}G";

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
			PresentCount.text = $"{upgradeCount}��";
			FutureCount.text = $"�ִ�ġ";
			PresentDamage.text = $"{upgradeCount * 5}������";
			FutureDamage.text = $"";
			UpgradePer.text = $"";
			UpgradeGold.text = $"";

		}
		else
		{
			PresentCount.text = $"{upgradeCount}��";
			FutureCount.text = $"{upgradeCount + 1}��";
			PresentDamage.text = $"{upgradeCount * 5}������";
			FutureDamage.text = $"{(upgradeCount + 1) * 5} ������";
			UpgradePer.text = $"{upgradeper.ToString("F2")} %";
			UpgradeGold.text = $"�Ҹ��� : {upgradeCount * 50}G";

		}
		PlayerGold.text = $"������� : {Player.Money}G";
	}

	// ��ȭ ��ư
	public void CallUpgradeButton()
	{
		// �ȳ��޼����� ���� �����Ѵٸ� ���
		if(MessageImage.gameObject.activeSelf)
		{
			return;
		}
		// ������ ����� ���ٸ� return
		if(Slot == null)
		{
			Managers.Sound.Play("Effect/UI/LimitButton");
			MessageImage.gameObject.SetActive(true);
			MessageText.text = "��ȭ ����� �������ּ���!";
			StartCoroutine(MyTools.ImageFadeOut(MessageImage,1.0f));
			Managers.CallWaitForSeconds(1.0f, () => { MessageImage.gameObject.SetActive(false); });
			return;
		}
		// ������ ����� ��ȭ��ġ�� �ִ��� return
		if (Managers.GData.playerinfo.UpgradeWeapon >= 8)
		{
			Managers.Sound.Play("Effect/UI/LimitButton");
			MessageImage.gameObject.SetActive(true);
			MessageText.text = "��ȭ��ġ�� �ִ� �Դϴ�!!";
			StartCoroutine(MyTools.ImageFadeOut(MessageImage, 1.0f));
			Managers.CallWaitForSeconds(1.0f, () => { MessageImage.gameObject.SetActive(false); });
			return;
		}
		// �������� �����ϴٸ� return
		if (Player.Money < upgradeCount* 50)
		{
			Managers.Sound.Play("Effect/UI/LimitButton");

			MessageImage.gameObject.SetActive(true);
			MessageText.text = "������尡 �����մϴ�!";
			StartCoroutine(MyTools.ImageFadeOut(MessageImage, 1.0f));
			Managers.CallWaitForSeconds(1.0f, () => { MessageImage.gameObject.SetActive(false); });
			return;
		}
		// ������ Ȯ���� ���� ��ȭ ����/����
		else
		{
			Player.Money -= upgradeCount* 50;
			float val = Random.Range(1, 99.9f);
			// ��ȭ ����
			if(val < upgradeper)
			{
				MessageImage.gameObject.SetActive(true);
				MessageText.text = "��ȭ ����!";
				StartCoroutine(MyTools.ImageFadeOut(MessageImage, 1.0f));
				Managers.CallWaitForSeconds(1.0f, () => { MessageImage.gameObject.SetActive(false); });
				Managers.GData.playerinfo.UpgradeWeapon++;
				Slot.UpgradeCount.text = $"{Managers.GData.playerinfo.UpgradeWeapon}��";
				Managers.Sound.Play("Effect/UI/UpgradeSuccess");
			}
			// ��ȭ ����
			else
			{
				MessageImage.gameObject.SetActive(true);
				MessageText.text = "��ȭ ����!";
				StartCoroutine(MyTools.ImageFadeOut(MessageImage, 1.0f));
				Managers.CallWaitForSeconds(1.0f, () => { MessageImage.gameObject.SetActive(false); });
				Managers.Sound.Play("Effect/UI/UpgradeFail");

			}
			// �ؽ�Ʈ �ֽ�ȭ
			RefreshText();
		}

	}
}
