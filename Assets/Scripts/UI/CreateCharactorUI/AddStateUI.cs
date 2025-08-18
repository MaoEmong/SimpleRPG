using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 캐릭터 생성씬에서 쓰이는 UI
// 2차 추가스텟 설정 UI
public class AddStateUI : MonoBehaviour
{
	// 플레이어 이름
	[SerializeField]
	Text Name;
	// 스텟 텍스트
	[SerializeField]
	Text[] StateText;
	// 추가 스텟 텍스트
	[SerializeField]
	Text RemainText;
	// 추가 스텟 Max
	int MaxRemain = 9;
	// 남아있는 추가 스텟
	int CurRemain;
	// 스텟 배열 / Str, Dex, Sti, Luk
	int[] AddState = new int[4];
	// 스텟 추가 버튼
	[SerializeField]
	Button[] AddButton;

	// 화면 전환용 블랙이미지
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

	#region UI에서 콜백하는 스텟추가 함수
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

	// 텍스트 재설정
	public void RefreshText()
	{
		Name.text = $"이름 : {Managers.GData.playerinfo.Name}";
		RemainText.text = $"{CurRemain}";
		StateText[0].text = $"{Managers.GData.playerinfo.Str} + {AddState[0]}";
		StateText[1].text = $"{Managers.GData.playerinfo.Dex} + {AddState[1]}";
		StateText[2].text = $"{Managers.GData.playerinfo.Sti} + {AddState[2]}";
		StateText[3].text = $"{Managers.GData.playerinfo.Luk} + {AddState[3]}";
	}

	// 진행상황 초기화
	public void ReStartSet()
	{
		ActiveTrueButton();
		for(int i = 0; i < AddState.Length; i++)
			AddState[i] = 0;
		CurRemain = MaxRemain;
		RefreshText();
	}

	// 이후 게임 시작
	public void EnterSet()
	{
		if (CurRemain > 0)
		{
			if (!ErrorImage.gameObject.activeSelf)
			{
				ErrorText.text = "남은 스텟포인트를 전수 소모해주세요.";
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

	// 스텟추가버튼의 활성화
	void ActiveTrueButton()
	{
		foreach(var n in AddButton)
		{
			n.gameObject.SetActive(true);
		}
	}
	// 스텟추가버튼의 비활성화
	void ActiveFalseButton()
	{
		foreach (var n in AddButton)
		{
			n.gameObject.SetActive(false);
		}

	}
}
