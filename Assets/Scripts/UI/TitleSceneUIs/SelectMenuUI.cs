using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 타이틀에서 메뉴 선택 UI
public class SelectMenuUI : MonoBehaviour
{
	// 플레이어 이름
	[SerializeField]
	Text[] playerName;
	// 플레이어 레벨
	[SerializeField]
	Text[] playerLevel;
	// 데이터 삭제를 묻는 메시지 텍스트
	[SerializeField]
	Image DeleteMessage;
	// 데이터 삭제 완료 텍스트
	[SerializeField]
	Text DeleteText;

	// 볼륨 설정 
	[SerializeField]
	Slider BgmSlider;
	[SerializeField]
	Slider EffectSlider;
	[SerializeField]
	Text BgmVol;
	[SerializeField]
	Text EffectVol;

	// 화면 전환용 블랙 이미지
	[SerializeField]
	Image SceneBlackImage;

	[SerializeField]
	string playername;

	private void Start()
	{
		BgmSlider.value = Managers.Sound.GetBGMVol();
		EffectSlider.value = Managers.Sound.GetEffectVol();

		BgmSlider.onValueChanged.AddListener(Managers.Sound.SetBGMVol);
		EffectSlider.onValueChanged.AddListener(Managers.Sound.SetEffectVol);

		playername = Managers.GData.playerinfo.Name;

		// 현재 플레이어 데이터에서 레벨이 0이라면 데이터 없음 처리
		if(Managers.GData.playerinfo.Level <= 0)
		{
			foreach (var t in playerName)
				t.text = $"데이터 없음";
			foreach (var t in playerLevel)
				t.text = $"Lev.XX";

		}
		else
        {
			foreach (var t in playerName)
				t.text = $"이름 : {Managers.GData.playerinfo.Name}";
			foreach (var t in playerLevel)
				t.text = $"Lev.{Managers.GData.playerinfo.Level}";

		}


		DeleteMessage.gameObject.SetActive(false);
		DeleteText.gameObject.SetActive(false);
		SceneBlackImage.gameObject.SetActive(false);
	}

	private void Update()
	{
		BgmVol.text = $"{Mathf.Ceil(BgmSlider.value * 100)}%";
		EffectVol.text = $"{Mathf.Ceil(EffectSlider.value * 100)}%";

	}

	// 게임시작 콜백함수
	public void StartGame()
	{
		SceneBlackImage.gameObject.SetActive(true);
		StartCoroutine(MyTools.ImageFadeIn(SceneBlackImage,2f));
		// 플레이어 데이터의 레벨이 0보다 작다면(아직 데이터가 없다면)
		if (Managers.GData.playerinfo.Level <= 0)
		{
			// 캐릭터 생성 씬으로 이동
			Managers.CallWaitForSeconds(2.0f, () =>
			{
				Managers.Sound.BgmStop();
			});
			Managers.CallWaitForSeconds(3.0f, () => {
				SceneManager.LoadScene("CreateCharactorScene");
			});
		}
		// 플레이어 데이터의 레벨이 0보다 크다면(데이터가 존재한다면)
		else
		{
			// 메인씬으로 이동
			Managers.CallWaitForSeconds(2.0f, () =>
			{
				Managers.Sound.BgmStop();
			});
			Managers.CallWaitForSeconds(3.0f, () => {
				SceneManager.LoadScene("MainScene");
			});
		}
	}
	// 데이터 삭제 콜백함수
	public void DeleteData()
	{
		// 플레이어 데이터의 레벨이 0보다 작다면(아직 데이터가 없다면)
		if (Managers.GData.playerinfo.Level <= 0)
		{
			// 삭제 불가 메세지
			Managers.Sound.Play("Effect/UI/LimitButton");
			DeleteMessage.gameObject.SetActive(true);
			DeleteText.gameObject.SetActive(true);
			DeleteText.text = "삭제할 데이터가 없습니다";
		}
		// 플레이어 데이터의 레벨이 0보다 크다면(데이터가 존재한다면)
		else
		{
			// 데이터 삭제
			Managers.GData.Clear();

			DeleteMessage.gameObject.SetActive(true);
			DeleteText.gameObject.SetActive(true);
			DeleteText.text = "데이터 삭제 완료!";

			foreach (var t in playerName)
				t.text = $"데이터 없음";
			foreach (var t in playerLevel)
				t.text = $"Lev.XX";

			Managers.GData.SaveData();
		}

		Managers.CallWaitForSeconds(0.5f, () => {
			DeleteMessage.gameObject.SetActive(false);
			DeleteText.gameObject.SetActive(false);
		});

	}
	// 게임종료
	public void ExitGame()
	{
		Application.Quit();
	}

}
