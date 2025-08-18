using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���� ���� UI
public class OptionScripts : MonoBehaviour
{
	// ���� ������ UI 
	public Slider BGMSoundSlider;
	public Slider EffectSoundSlider;
	public Text BgmVolText;
	public Text EffectVolText;
	// ȭ�� ��ȯ�� �� �̹���
	public Image BlackImage;

	public Toggle FPSTogle;
	public GameObject FPSOBJ;

	private void Start()
	{
		BGMSoundSlider.value = Managers.Sound.GetBGMVol();
		EffectSoundSlider.value = Managers.Sound.GetEffectVol();

		BGMSoundSlider.onValueChanged.AddListener(Managers.Sound.SetBGMVol);
		EffectSoundSlider.onValueChanged.AddListener(Managers.Sound.SetEffectVol);

	}

	public void OnEnable()
	{
		if (FPSTogle.gameObject.activeSelf)
			FPSTogle.isOn = true;
		else
			FPSTogle.isOn = false;

	}

	private void Update()
	{
		BgmVolText.text = $"{Mathf.Ceil(BGMSoundSlider.value * 100)}%";
		EffectVolText.text = $"{Mathf.Ceil(EffectSoundSlider.value * 100)}%";

	}

	// Ÿ��Ʋ�� ���ư��� �ݹ��Լ�
	public void GoToTitle()
	{
		Debug.Log("Call Go To Title");
		BlackImage.gameObject.SetActive(true);
		StartCoroutine(GoToTitleCoroutine());
	}
	IEnumerator GoToTitleCoroutine()
	{
		Managers.TimePlay();
		StartCoroutine(MyTools.ImageFadeIn(BlackImage, 1.0f));
		yield return new WaitForSeconds(1.5f);
		Debug.Log("Go To Title!");
		Managers.Scene.LeadScene(Define.SceneType.TitleScene);

	}

	// ���� ���� �ݹ��Լ�
	public void GameExit()
	{
		Application.Quit();
	}

	public void CallFPSTogle(bool _bool)
	{
		Managers.GData.playerinfo.ShowFPS = _bool;
		FPSOBJ.SetActive(_bool);
	}

}
