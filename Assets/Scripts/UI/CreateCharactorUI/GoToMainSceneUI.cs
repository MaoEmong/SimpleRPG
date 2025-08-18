using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���ξ����� �Ѿ�� UI
public class GoToMainSceneUI : MonoBehaviour
{
	// ȭ�� ��ȯ�� �� �̹���
	[SerializeField]
	Image BlackSceneImage;
	// ù��° �� �ؽ�Ʈ
	[SerializeField]
	Text WelcomeText;
	// �ι��� �� �ؽ�Ʈ
	[SerializeField]
	Text GoToMainText;
	// ���� ��ư
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
	// �ѱ��ھ� �߰��Ǵ� �׼�
	IEnumerator FadeInString()
	{
		string welcome = "ȯ���մϴ�, " + Managers.GData.playerinfo.Name;
		string startgame = "���ο� ����� ����մϴ�";
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
