using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// �÷��̾� ��� UI
public class PlayerDieCanvas : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
	public Image BackImage;
	public Text DieText;
	public Text TouchText;

	// �ߺ� �Է� ����
	bool ActionEnd = false;

	// �ڷ�ƾ ������
	Coroutine curCoroutine;

	public void OnPointerDown(PointerEventData eventData)
	{

	}

	public void OnPointerUp(PointerEventData eventData)
	{
		// �ߺ� ���� ����
		if(ActionEnd)
		{
			Managers.Sound.Play("Effect/UI/TouchScreen");
			ActionEnd = false;
			// ȭ�� ��ο���
			StopCoroutine(curCoroutine);
			StartCoroutine(MyTools.TextFadeOut(DieText, 1f));
			StartCoroutine(MyTools.TextFadeOut(TouchText, 1f));
			// �� ��ȯ
			Managers.CallWaitForSeconds(2.0f, () => { Managers.Scene.LeadScene(Define.SceneType.MainScene); });
		}
	}

	private void Start()
	{
		BackImage.gameObject.SetActive(false);
		DieText.gameObject.SetActive(false);
		TouchText.gameObject.SetActive(false);

		ActionEnd = false;
	}

	public void StartAction()
	{
		BackImage.gameObject.SetActive(true);
		StartCoroutine(MyTools.ImageFadeIn(BackImage,1.0f));
		Managers.CallWaitForSeconds(1.5f, () => { 
			DieText.gameObject.SetActive(true);	
			StartCoroutine(MyTools.TextFadeIn(DieText,1.0f)); 
		});
		curCoroutine = StartCoroutine(BreathText());

		// �÷��̾� ���� ������ ����
		Managers.GData.SaveData();
	}
	IEnumerator BreathText()
	{
		yield return new WaitForSeconds(3.0f);
		TouchText.gameObject.SetActive(true);
		ActionEnd = true;
		while(true)
		{
			yield return null;
			StartCoroutine(MyTools.TextFadeIn(TouchText, 0.5f));
			yield return new WaitForSeconds(0.5f);
			StartCoroutine(MyTools.TextFadeOut(TouchText, 0.5f));
			yield return new WaitForSeconds(0.5f);

		}

	}
}
