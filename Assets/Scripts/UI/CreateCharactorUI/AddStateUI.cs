using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ĳ���� ���������� ���̴� UI
// 2�� �߰����� ���� UI
public class AddStateUI : MonoBehaviour
{
	// �÷��̾� �̸�
	[SerializeField]
	Text Name;
	// ���� �ؽ�Ʈ
	[SerializeField]
	Text[] StateText;
	// �߰� ���� �ؽ�Ʈ
	[SerializeField]
	Text RemainText;
	// �߰� ���� Max
	int MaxRemain = 9;
	// �����ִ� �߰� ����
	int CurRemain;
	// ���� �迭 / Str, Dex, Sti, Luk
	int[] AddState = new int[4];
	// ���� �߰� ��ư
	[SerializeField]
	Button[] AddButton;

	// ȭ�� ��ȯ�� ���̹���
	[SerializeField]
	GameObject GoToMainImage;
	
	
	[SerializeField]
	Image ErrorImage;
	[SerializeField]
	Text ErrorText;

	private void Start()
	{
		GoToMainImage.SetActive(false);

		CurRemain = MaxRemain;
		for (int i = 0; i < AddState.Length; i++)
			AddState[i] = 0;


		RemainText.text = $"{CurRemain}";
		StateText[0].text = $"{Managers.GData.playerinfo.Str} + {AddState[0]}";
		StateText[1].text = $"{Managers.GData.playerinfo.Dex} + {AddState[1]}";
		StateText[2].text = $"{Managers.GData.playerinfo.Sti} + {AddState[2]}";
		StateText[3].text = $"{Managers.GData.playerinfo.Luk} + {AddState[3]}";
	}

	#region UI���� �ݹ��ϴ� �����߰� �Լ�
	public void SetStr() 
	{
		if (CurRemain <= 0)
			return;

		Managers.Sound.Play("Effect/UI/UIClick");

		AddState[0]++;
		CurRemain--;

		if (CurRemain <= 0)
			ActiveFalseButton();

		RefreshText();
	}
	public void SetDex() 
	{
		if (CurRemain <= 0)
			return;
		Managers.Sound.Play("Effect/UI/UIClick");

		AddState[1]++;
		CurRemain--;

		if (CurRemain <= 0)
			ActiveFalseButton();

		RefreshText();
	}
	public void SetSti() 
	{
		if (CurRemain <= 0)
			return;
		Managers.Sound.Play("Effect/UI/UIClick");

		AddState[2]++;
		CurRemain--;

		if (CurRemain <= 0)
			ActiveFalseButton();

		RefreshText();
	}
	public void SetLuk() 
	{
		if (CurRemain <= 0)
			return;
		Managers.Sound.Play("Effect/UI/UIClick");

		AddState[3]++;
		CurRemain--;

		if (CurRemain <= 0)
			ActiveFalseButton();

		RefreshText();
	}
	#endregion

	// �ؽ�Ʈ �缳��
	public void RefreshText()
	{
		Name.text = $"�̸� : {Managers.GData.playerinfo.Name}";
		RemainText.text = $"{CurRemain}";
		StateText[0].text = $"{Managers.GData.playerinfo.Str} + {AddState[0]}";
		StateText[1].text = $"{Managers.GData.playerinfo.Dex} + {AddState[1]}";
		StateText[2].text = $"{Managers.GData.playerinfo.Sti} + {AddState[2]}";
		StateText[3].text = $"{Managers.GData.playerinfo.Luk} + {AddState[3]}";
	}

	// �����Ȳ �ʱ�ȭ
	public void ReStartSet()
	{
		ActiveTrueButton();
		for(int i = 0; i < AddState.Length; i++)
			AddState[i] = 0;
		CurRemain = MaxRemain;
		RefreshText();
	}

	// ���� ���� ����
	public void EnterSet()
	{
		if (CurRemain > 0)
		{
			if (!ErrorImage.gameObject.activeSelf)
			{
				ErrorText.text = "���� ��������Ʈ�� ���� �Ҹ����ּ���.";
				ErrorImage.gameObject.SetActive(true);
				StartCoroutine(MyTools.ImageFadeOut(ErrorImage, 1.3f));
				Managers.CallWaitForSeconds(1.5f, () => { ErrorImage.gameObject.SetActive(false); });
				Managers.Sound.Play("Effect/UI/NotAccess");
			}
			return;
		}

		Managers.GData.playerinfo.Str += AddState[0];
		Managers.GData.playerinfo.Dex += AddState[1];
		Managers.GData.playerinfo.Sti += AddState[2];
		Managers.GData.playerinfo.Luk += AddState[3];
		Managers.GData.playerinfo.MaxHp = 100 + (Managers.GData.playerinfo.Sti * 10);
		Managers.GData.playerinfo.Money = 500;

		Managers.GData.SaveData();

		GoToMainImage.SetActive(true);
		GoToMainImage.GetComponent<GoToMainSceneUI>().Init();
		this.gameObject.SetActive(false);
	}

	// �����߰���ư�� Ȱ��ȭ
	void ActiveTrueButton()
	{
		foreach(var n in AddButton)
		{
			n.gameObject.SetActive(true);
		}
	}
	// �����߰���ư�� ��Ȱ��ȭ
	void ActiveFalseButton()
	{
		foreach (var n in AddButton)
		{
			n.gameObject.SetActive(false);
		}

	}
}
