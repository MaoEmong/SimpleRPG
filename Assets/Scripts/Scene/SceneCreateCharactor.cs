using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 캐릭터 생성 씬
public class SceneCreateCharactor : SceneBase
{
	// 화면 전환 용 검은 이미지
	[SerializeField]
	Image BlackImage;
	// 간단한 액션을 위한 카메라 오브젝트
	[SerializeField]
	GameObject MoveCamera;
	// 이름 입력 오브젝트
	[SerializeField]
	GameObject InputArea;
	// 이름 입력 인풋필드
	[SerializeField]
	InputField InputName;
	// 설정한 캐릭터 이름
	[SerializeField]
	string charactorName;
	// 1차 스텟 설정 오브젝트
	[SerializeField]
	GameObject SetRollStateArea;
	// 스텟 설정 오브젝트에 적용할 플레이어이름
	[SerializeField]
	Text SetRollNameText;
	// 2차 스텟 설정 오브젝트
	[SerializeField]
	GameObject SetAddStateArea;
	// 플레이어블 캐릭터 오브젝트
	public GameObject PlayerOBJ;
	// 도움말 버튼
	[SerializeField]
	GameObject HelpButton;

	[SerializeField]
	Image ErrorImage;
	[SerializeField]
	Text ErrorText;

	// Awake
	protected override void Init()
	{
		base.Init();
		type = Define.SceneType.CreateScene;

		BlackImage.gameObject.SetActive(false);
		InputArea.SetActive(false);
		SetRollStateArea.SetActive(false);
		SetAddStateArea.SetActive(false);
		HelpButton.SetActive(false);
		ErrorImage.gameObject.SetActive(false);
	}

	protected override void Start()
	{
		base.Start();
		Managers.Scene.CurScene = this;
		Managers.GData.Clear();
		BlackImage.gameObject.SetActive(true);
		StartCoroutine(MyTools.ImageFadeOut(BlackImage, 2.0f));
		Managers.CallWaitForSeconds(2.0f, () => { BlackImage.gameObject.SetActive(false); });
		Managers.CallWaitForSeconds(3.0f, () => { StartCoroutine(CameraMove()); });
		Managers.CallWaitForSeconds(5.0f, () => { InputArea.SetActive(true); });

		Managers.CallWaitForSeconds(0.3f, () => { Managers.Sound.Play("BGM/CreateScene", Define.Sound.Bgm); });		
	}

	private void Update()
	{
		PlayerOBJ.transform.Rotate(new Vector3(0, 1, 0) * 45 * Time.deltaTime);
	}


	IEnumerator CameraMove()
	{
		Vector3 TargetPos = MoveCamera.transform.position + new Vector3(-1,0,0);
		Vector3 Dir = TargetPos - MoveCamera.transform.position;
		float Distance = Vector3.Distance(MoveCamera.transform.position, TargetPos);
		Dir = Dir.normalized;
		float endTime = 1.0f;
		float curTime = 0;
		float speed = Distance / endTime;

		while(curTime < endTime)
		{
			yield return null;
			MoveCamera.transform.position += Dir * speed * Time.deltaTime;

			curTime += Time.deltaTime;
		}

	}

	// UI에서 콜백할 함수
	public void EnterInputName()
	{
		string inputname = InputName.text;

		if(inputname.Length <= 0)
		{
			if (!ErrorImage.gameObject.activeSelf)
			{
				ErrorText.text = "잘못된 입력입니다. 다시 입력해주세요";
				ErrorImage.gameObject.SetActive(true);
				StartCoroutine(MyTools.ImageFadeOut(ErrorImage, 1.3f));
				Managers.CallWaitForSeconds(1.5f, () => { ErrorImage.gameObject.SetActive(false); });
				Managers.Sound.Play("Effect/UI/NotAccess");
			}
			return;
		}

		charactorName = InputName.text;
		Managers.GData.playerinfo.Name = charactorName;
		Managers.GData.playerinfo.Level = 1;
		SetRollNameText.text = $"이름 : {Managers.GData.playerinfo.Name}";

		InputArea.SetActive(false);
		SetRollStateArea.SetActive(true);
		HelpButton.SetActive(true);
	}
	// UI에서 콜백하는 함수
	public void EnterSetRollState()
	{
		SetRollStateArea.SetActive(false);
		SetAddStateArea.SetActive(true);
		CallUIClickSound();
	}

	public void CallUIClickSound()
	{
		Managers.Sound.Play("Effect/UI/UIClick2");
	}

	public override void Clear()
	{
	}
}
