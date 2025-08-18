using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 마을 씬(메인씬)
public class SceneMain : SceneBase
{
	// 조이스틱
	[SerializeField]
	Joystick joystick;
	public Joystick Joystick { get; private set; }
	// 플레이어 캐릭터
	[SerializeField]
	GameObject player;
	// UI캔버스
	[SerializeField]
	Canvas mainCanvas;

	// 화면 전환용 이미지
	public Image BlackImage;

	[SerializeField]
	Transform NextPlayerPos;
	[SerializeField]
	Transform BackPlayerPos;

	[SerializeField]
	Canvas FieldNameCanvas;
	[SerializeField]
	Text FieldNameText;

	public GameObject FPSPBJ;

	// Awake에서 바로 불러옴
	protected override void Init()
	{
		base.Init();
		// 오브젝트를 찾아 가져오는건 Init(Awake)에서 처리
		type = Define.SceneType.MainScene;
		joystick = GameObject.Find("JoyStick").GetComponent<Joystick>();
		player = GameObject.Find("Player").gameObject;
	}

	protected override void Start()
	{
		// 다른 스크립트에 데이터 복사, 전달하는건 Start에서 처리
		base.Start();
		Managers.Scene.CurScene = this;
		Managers.Player = player;

		switch (Managers.GData.isNext)
		{
			case true:
				player.transform.position = NextPlayerPos.position;
				break;
			case false:
				player.transform.position = BackPlayerPos.position;
				break;
		}

		BlackImage.gameObject.SetActive(true);
		StartCoroutine(MyTools.ImageFadeOut(BlackImage, 1.0f));
		Managers.CallWaitForSeconds(1.0f, () => { BlackImage.gameObject.SetActive(false); });
		// 게임데이터를 기반으로 한 플레이어 데이터 설정
		State info = new(
			Managers.GData.playerinfo.Name,
			Managers.GData.playerinfo.MaxHp,
			Managers.GData.playerinfo.Hp,
			Managers.GData.playerinfo.Str,
			Managers.GData.playerinfo.Dex,
			Managers.GData.playerinfo.Sti,
			Managers.GData.playerinfo.Luk
			);
		player.GetComponent<CharactorState>().Init(info, Managers.GData.playerinfo.Level);
		player.GetComponent<CharactorMovement>().Init(joystick,Define.FieldType.Town);
		player.GetComponent<CharactorState>().CurState.Hp = player.GetComponent<CharactorState>().CurState.MaxHp;

		mainCanvas.GetComponent<MainCanvasUI>().Init(player.GetComponent<CharactorMovement>());

		FieldNameCanvas.gameObject.SetActive(false);
		StartCoroutine(FieldNameFade());

		Managers.CallWaitForSeconds(0.5f, () => { Managers.Sound.Play("BGM/MainScene", Define.Sound.Bgm); });

		FPSPBJ.SetActive(Managers.GData.playerinfo.ShowFPS);
	}

	IEnumerator FieldNameFade()
	{
		yield return new WaitForSeconds(0.5f);
		FieldNameCanvas.gameObject.SetActive(true);
		StartCoroutine(MyTools.TextFadeIn(FieldNameText, 1.0f));
		yield return new WaitForSeconds(1.8f);
		StartCoroutine(MyTools.TextFadeOut(FieldNameText, 1.0f));
		yield return new WaitForSeconds(3.0f);
		FieldNameCanvas.gameObject.SetActive(false);
	}

	public override void Clear()
	{

	}
}
