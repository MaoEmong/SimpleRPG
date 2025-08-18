using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 메인씬으로 넘어가는 UI
public class GoToMainSceneUI : MonoBehaviour
{
	// 화면 전환용 블랙 이미지
	[SerializeField]
	Image BlackSceneImage;
	// 첫번째 줄 텍스트
	[SerializeField]
	Text WelcomeText;
	// 두번쨰 줄 텍스트
	[SerializeField]
	Text GoToMainText;
	// 도움말 버튼
	[SerializeField]
	GameObject HelpButton;

	public void Init()
	{
		HelpButton.SetActive(false);
		BlackSceneImage.gameObject.SetActive(true);
		WelcomeText.text = "";
		GoToMainText.text = "";
		Managers.CallWaitForSeconds(0.3f, () =>
		{
			StartCoroutine(FadeInString());
		});
	}
	// 한글자씩 추가되는 액션
	IEnumerator FadeInString()
	{
		string welcome = "환영합니다, " + Managers.GData.playerinfo.Name;
		string startgame = "새로운 세계로 출발합니다";
		string curtext1 = "";
		string curtext2 = "";

		for (int i = 0; i < welcome.Length; i++)
		{
			yield return new WaitForSeconds(0.1f);
			curtext1 += welcome[i];
			WelcomeText.text = curtext1;
			Managers.Sound.Play("Effect/UI/StringUI");
		}
		yield return new WaitForSeconds(0.3f);
		for (int i = 0; i < startgame.Length; i++)
		{
			yield return new WaitForSeconds(0.1f);
			curtext2 += startgame[i];
			GoToMainText.text = curtext2;
			Managers.Sound.Play("Effect/UI/StringUI");
		}
		yield return new WaitForSeconds(0.5f);
		BlackSceneImage.gameObject.SetActive(true);
		StartCoroutine(MyTools.ImageFadeIn(BlackSceneImage, 1.5f));
		Managers.CallWaitForSeconds(1.4f, () => { Managers.Sound.BgmStop(); } );
		Managers.CallWaitForSeconds(1.8f, () => { GoToMainScene(); } );
	}

	void GoToMainScene()
	{
		Managers.Scene.LeadScene(Define.SceneType.MainScene);
	}
}
