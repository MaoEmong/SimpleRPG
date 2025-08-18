using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// ���� ��(���ξ�)
public class SceneMain : SceneBase
{
	// ���̽�ƽ
	[SerializeField]
	Joystick joystick;
	public Joystick Joystick { get; private set; }
	// �÷��̾� ĳ����
	[SerializeField]
	GameObject player;
	// UIĵ����
	[SerializeField]
	Canvas mainCanvas;

	// ȭ�� ��ȯ�� �̹���
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

	// Awake���� �ٷ� �ҷ���
	protected override void Init()
	{
		base.Init();
		// ������Ʈ�� ã�� �������°� Init(Awake)���� ó��
		type = Define.SceneType.MainScene;
		joystick = GameObject.Find("JoyStick").GetComponent<Joystick>();
		player = GameObject.Find("Player").gameObject;
	}

	protected override void Start()
	{
		// �ٸ� ��ũ��Ʈ�� ������ ����, �����ϴ°� Start���� ó��
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
		// ���ӵ����͸� ������� �� �÷��̾� ������ ����
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
