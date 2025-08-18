using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ĳ���� ���� ��
public class SceneCreateCharactor : SceneBase
{
	// ȭ�� ��ȯ �� ���� �̹���
	[SerializeField]
	Image BlackImage;
	// ������ �׼��� ���� ī�޶� ������Ʈ
	[SerializeField]
	GameObject MoveCamera;
	// �̸� �Է� ������Ʈ
	[SerializeField]
	GameObject InputArea;
	// �̸� �Է� ��ǲ�ʵ�
	[SerializeField]
	InputField InputName;
	// ������ ĳ���� �̸�
	[SerializeField]
	string charactorName;
	// 1�� ���� ���� ������Ʈ
	[SerializeField]
	GameObject SetRollStateArea;
	// ���� ���� ������Ʈ�� ������ �÷��̾��̸�
	[SerializeField]
	Text SetRollNameText;
	// 2�� ���� ���� ������Ʈ
	[SerializeField]
	GameObject SetAddStateArea;
	// �÷��̾�� ĳ���� ������Ʈ
	public GameObject PlayerOBJ;
	// ���� ��ư
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

	// UI���� �ݹ��� �Լ�
	public void EnterInputName()
	{
		string inputname = InputName.text;

		if(inputname.Length <= 0)
		{
			if (!ErrorImage.gameObject.activeSelf)
			{
				ErrorText.text = "�߸��� �Է��Դϴ�. �ٽ� �Է����ּ���";
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
		SetRollNameText.text = $"�̸� : {Managers.GData.playerinfo.Name}";

		InputArea.SetActive(false);
		SetRollStateArea.SetActive(true);
		HelpButton.SetActive(true);
	}
	// UI���� �ݹ��ϴ� �Լ�
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
