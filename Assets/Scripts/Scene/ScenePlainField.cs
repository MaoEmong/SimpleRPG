using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenePlainField : SceneBase
{   // ���̽�ƽ
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
	Transform NextPlayerSpawnPos;
	[SerializeField]
	Transform BackPlayerSpawnPos;

	[SerializeField]
	int Spawnlevel;
	[SerializeField]
	List<EnemyspawnField> SpawnEnemy;
	[SerializeField]
	EnemyGreenDragon Boss;

	[SerializeField]
	GameObject BlockOBJ;

	[SerializeField]
	Canvas FieldNameCanvas;
	[SerializeField]
	Text FieldNameText;

	public GameObject FPSPBJ;

	// Awake
	protected override void Init()
	{
		base.Init();
		// ������Ʈ�� ã�� �������°� Init(Awake)���� ó��
		type = Define.SceneType.PlainFieldScene;
		joystick = GameObject.Find("JoyStick").GetComponent<Joystick>();
		player = GameObject.Find("Player").gameObject;

	}


	protected override void Start()
	{
		base.Start();
		Managers.Scene.CurScene = this;
		Managers.Player = player;
		// ��Ż ��ġ�� ����
		switch (Managers.GData.isNext)
		{
			case true:
				player.transform.position = NextPlayerSpawnPos.position;
				break;
			case false:
				player.transform.position = BackPlayerSpawnPos.position;
				break;
		}

		// ȭ�� ��ȯ �̹���
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
		// �÷��̾� �ʱ�ȭ
		player.GetComponent<CharactorState>().Init(info, Managers.GData.playerinfo.Level);
		player.GetComponent<CharactorMovement>().Init(joystick, Define.FieldType.Field);

		/*======================== �׽�Ʈ��
		player.GetComponent<CharactorState>().CurState.MaxHp = 1000;
		player.GetComponent<CharactorState>().CurState.Hp = 1000;
		player.GetComponent<CharactorState>().CurState.Str = 1000;
		player.GetComponent<CharactorState>().CurState.Sti = 1000;
		player.GetComponent<CharactorState>().SetOtherState();

		*/

		// ���� ĵ���� �ʱ�ȭ
		mainCanvas.GetComponent<MainCanvasUI>().Init(player.GetComponent<CharactorMovement>());

		foreach (var n in SpawnEnemy)
		{
			int val = Random.Range(1, 3);
			n.Init(Spawnlevel, val);
		}

		FieldNameCanvas.gameObject.SetActive(false);
		StartCoroutine(FieldNameFade());

		Managers.Sound.Play("BGM/PlainScene", Define.Sound.Bgm);
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

	private void Update()
	{
		if (Boss.isDie)
		{
			if (BlockOBJ.activeSelf)
			{
				BlockOBJ.SetActive(false);
			}
		}
	}

	public override void Clear()
	{
	}

}
