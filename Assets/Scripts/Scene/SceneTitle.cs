using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 타이틀 씬
public class SceneTitle : SceneBase
{
	// 제작 로고
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

		// 제작 로고의 활성화
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
