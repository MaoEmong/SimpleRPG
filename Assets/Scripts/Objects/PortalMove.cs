using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 포탈 스트립트
public class PortalMove : MonoBehaviour
{
	// 이동할 씬의 정보
	public Define.SceneType nextScene;

	// 타이머
	[SerializeField]
	float timer = 0f;
	// 타이머 종료 시점
	[SerializeField]
	float targettime = 1.0f;

	bool GoToNextScene = false;

	CharactorMovement player = null;

	public bool isNext = false;

	[SerializeField]
	Image BlackImage;

	private void Start()
	{
		timer = 0;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			if (player == null)
				player = other.gameObject.GetComponent<CharactorMovement>();
		}

		OnTriggerStay(other);
	}
	private void OnTriggerStay(Collider other)
	{
		if (GoToNextScene)
			return;

		// 충돌중인 오브젝트의 태그가 Player라면
		if(other.CompareTag("Player"))
		{
			// 타이머 시작
			timer += Time.deltaTime;
			// 정해진 시간만큼 흐른 뒤
			if(timer > targettime)
			{
				// 화면 가린 후 씬이동(이동 전 현재 플레이어 데이터 저장, 이동 후 저장된 플레이어 데이터로 새로운플레이어 객체에 데이터 전달)
				GoToNextScene = true;

				BlackImage.gameObject.SetActive(true);

				StartCoroutine(MyTools.ImageFadeIn(BlackImage,1.0f));
				Managers.Sound.Play("Effect/UI/PortalSound");
				switch(isNext)
				{
					case true:
						Managers.GData.isNext = true;
						break;
					case false:
						Managers.GData.isNext = false;
						break;
				}
				Managers.CallWaitForSeconds(0.8f, () => { Managers.Sound.BgmStop(); });

				Managers.CallWaitForSeconds(1.2f, () => 
				{
					Managers.GData.SaveData();
					Managers.Scene.LeadScene(nextScene);
				});


			}
		}
	}
	private void OnTriggerExit(Collider other)
	{
		// 플레이어가 해당 오브젝트에서 빠져나간다면 
		if(other.CompareTag("Player"))
		{
			// 타이머 초기화
			timer = 0f;
		}
	}

}
