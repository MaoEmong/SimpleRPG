using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ÿ��Ʋ ��
public class SceneTitle : SceneBase
{
	// ���� �ΰ�
	[SerializeField]
	GameObject MyLogo;


	protected override void Init()
	{
		base.Init();
		type = Define.SceneType.TitleScene;

	}

	protected override void Start()
	{
		base.Start();
		Managers.Scene.CurScene = this;

		// ���� �ΰ��� Ȱ��ȭ
		MyLogo.SetActive(true);

	}

	public override void Clear()
	{
	}

	public void CallButtonSound()
	{
		Managers.Sound.Play("Effect/UI/UIClick");
	}
	public void CallStartGame()
	{
		Managers.Sound.Play("Effect/UI/StartGame");
	}
}
