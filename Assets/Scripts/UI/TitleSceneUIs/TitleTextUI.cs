using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Ÿ��Ʋ ���� �ؽ�Ʈ UI
public class TitleTextUI : MonoBehaviour,IPointerDownHandler , IPointerUpHandler
{
	// �ΰ� �̹���
	[SerializeField]
	Image LogoImage;
	// ���� �̸� �ؽ�Ʈ
	[SerializeField]
	Text TitleText;
	// ȭ�� ��ġ �ؽ�Ʈ
	[SerializeField]
	Text TouchText;

	// �޴�UI
	[SerializeField]
	GameObject MenuPanel;
	[SerializeField]
	GameObject SelectMenuPanel;

	bool isStart = false;

	private void Awake()
	{
		LogoImage.gameObject.SetActive(false);
		TitleText.gameObject.SetActive(false);
		TouchText.gameObject.SetActive(false);
		MenuPanel.SetActive(false);
		SelectMenuPanel.SetActive(false);
	}

	private void Start()
	{
		LogoImage.gameObject.SetActive(true);
		LogoImage.color = new Color(LogoImage.color.r, LogoImage.color.g, LogoImage.color.b, 0);
		StartCoroutine(MyTools.ImageFadeIn(LogoImage, 1.5f));
		Managers.CallWaitForSeconds(2.0f, () => { StartCoroutine(MyTools.ImageFadeOut(LogoImage, 1.0f)); });
		Managers.CallWaitForSeconds(4.0f, () => {
			LogoImage.gameObject.SetActive(false);
			TitleText.gameObject.SetActive(true);
			Managers.Sound.Play("BGM/TitleScene",Define.Sound.Bgm);
			StartCoroutine(MyTools.TextFadeIn(TitleText, 2.0f));
			StartCoroutine(MyTools.ImageFadeOut(GetComponent<Image>(), 2.0f));
		});
		Managers.CallWaitForSeconds(7.0f, () => {
			isStart = true;
			TouchText.gameObject.SetActive(true); 
			StartCoroutine(BreathText()); 
		});	
	
	}

	// ȭ�� ��ġ �ؽ�Ʈ Breath �׼�
	IEnumerator BreathText()
	{
		while (true) 
		{
			StartCoroutine(MyTools.TextFadeIn(TouchText, 1.5f));
			yield return new WaitForSeconds(2.0f);
			StartCoroutine(MyTools.TextFadeOut(TouchText, 1.5f));
			yield return new WaitForSeconds(2.0f);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!isStart)
			return;

		TouchText.gameObject.SetActive(false);
		TitleText.gameObject.SetActive(false);

		Managers.Sound.Play("Effect/UI/TouchScreen");

		Image image = GetComponent<Image>();
		image.color = Color.white;
		StartCoroutine(MyTools.ImageFadeOut(image, 0.3f));
		Managers.CallWaitForSeconds(0.5f, () => {
			gameObject.SetActive(false);
			Managers.CallWaitForSeconds(0.5f, () => { MenuPanel.SetActive(true);SelectMenuPanel.SetActive(true); });
		});
		isStart = false;

	}

	public void OnPointerDown(PointerEventData eventData) { }
}
