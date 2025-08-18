using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 플레이어 사망 UI
public class PlayerDieCanvas : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
	public Image BackImage;
	public Text DieText;
	public Text TouchText;

	// 중복 입력 방지
	bool ActionEnd = false;

	// 코루틴 관리용
	Coroutine curCoroutine;

	public void OnPointerDown(PointerEventData eventData)
	{

	}

	public void OnPointerUp(PointerEventData eventData)
	{
		// 중복 실행 방지
		if(ActionEnd)
		{
			Managers.Sound.Play("Effect/UI/TouchScreen");
			ActionEnd = false;
			// 화면 어두워짐
			StopCoroutine(curCoroutine);
			StartCoroutine(MyTools.TextFadeOut(DieText, 1f));
			StartCoroutine(MyTools.TextFadeOut(TouchText, 1f));
			// 씬 전환
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

		// 플레이어 게임 데이터 저장
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
