using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 제작중 알림 스크립트(출입 방지용)
public class DevelopPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	bool isTouch = false;

	[SerializeField]
	Image DevelopImage;
	[SerializeField]
	Image BlackImage;

	public void OnPointerDown(PointerEventData eventData)
	{
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if(!isTouch) 
		{
			isTouch = true;
			Managers.TimePlay();
			DevelopImage.gameObject.SetActive(false);
			StartCoroutine(GoToMainScene());
		}
	}

	IEnumerator GoToMainScene()
	{
		BlackImage.gameObject.SetActive(true);
		StartCoroutine(MyTools.ImageFadeIn(BlackImage,1.0f));
		Managers.Sound.Play("Effect/UI/PortalSound");
		yield return new WaitForSeconds(1.3f);
		Managers.Scene.LeadScene(Define.SceneType.MainScene);
	}
}
