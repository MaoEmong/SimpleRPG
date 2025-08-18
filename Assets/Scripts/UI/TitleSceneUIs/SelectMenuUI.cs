using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ÿ��Ʋ���� �޴� ���� UI
public class SelectMenuUI : MonoBehaviour
{
	// �÷��̾� �̸�
	[SerializeField]
	Text[] playerName;
	// �÷��̾� ����
	[SerializeField]
	Text[] playerLevel;
	// ������ ������ ���� �޽��� �ؽ�Ʈ
	[SerializeField]
	Image DeleteMessage;
	// ������ ���� �Ϸ� �ؽ�Ʈ
	[SerializeField]
	Text DeleteText;

	// ���� ���� 
	[SerializeField]
	Slider BgmSlider;
	[SerializeField]
	Slider EffectSlider;
	[SerializeField]
	Text BgmVol;
	[SerializeField]
	Text EffectVol;

	// ȭ�� ��ȯ�� �� �̹���
	[SerializeField]
	Image SceneBlackImage;

	[SerializeField]
	string playername;

	private void Start()
	{
		BgmSlider.value = Managers.Sound.GetBGMVol();
		EffectSlider.value = Managers.Sound.GetEffectVol();

		BgmSlider.onValueChanged.AddListener(Managers.Sound.SetBGMVol);
		EffectSlider.onValueChanged.AddListener(Managers.Sound.SetEffectVol);

		playername = Managers.GData.playerinfo.Name;

		// ���� �÷��̾� �����Ϳ��� ������ 0�̶�� ������ ���� ó��
		if(Managers.GData.playerinfo.Level <= 0)
		{
			foreach (var t in playerName)
				t.text = $"������ ����";
			foreach (var t in playerLevel)
				t.text = $"Lev.XX";

		}
		else
        {
			foreach (var t in playerName)
				t.text = $"�̸� : {Managers.GData.playerinfo.Name}";
			foreach (var t in playerLevel)
				t.text = $"Lev.{Managers.GData.playerinfo.Level}";

		}


		DeleteMessage.gameObject.SetActive(false);
		DeleteText.gameObject.SetActive(false);
		SceneBlackImage.gameObject.SetActive(false);
	}

	private void Update()
	{
		BgmVol.text = $"{Mathf.Ceil(BgmSlider.value * 100)}%";
		EffectVol.text = $"{Mathf.Ceil(EffectSlider.value * 100)}%";

	}

	// ���ӽ��� �ݹ��Լ�
	public void StartGame()
	{
		SceneBlackImage.gameObject.SetActive(true);
		StartCoroutine(MyTools.ImageFadeIn(SceneBlackImage,2f));
		// �÷��̾� �������� ������ 0���� �۴ٸ�(���� �����Ͱ� ���ٸ�)
		if (Managers.GData.playerinfo.Level <= 0)
		{
			// ĳ���� ���� ������ �̵�
			Managers.CallWaitForSeconds(2.0f, () =>
			{
				Managers.Sound.BgmStop();
			});
			Managers.CallWaitForSeconds(3.0f, () => {
				SceneManager.LoadScene("CreateCharactorScene");
			});
		}
		// �÷��̾� �������� ������ 0���� ũ�ٸ�(�����Ͱ� �����Ѵٸ�)
		else
		{
			// ���ξ����� �̵�
			Managers.CallWaitForSeconds(2.0f, () =>
			{
				Managers.Sound.BgmStop();
			});
			Managers.CallWaitForSeconds(3.0f, () => {
				SceneManager.LoadScene("MainScene");
			});
		}
	}
	// ������ ���� �ݹ��Լ�
	public void DeleteData()
	{
		// �÷��̾� �������� ������ 0���� �۴ٸ�(���� �����Ͱ� ���ٸ�)
		if (Managers.GData.playerinfo.Level <= 0)
		{
			// ���� �Ұ� �޼���
			Managers.Sound.Play("Effect/UI/LimitButton");
			DeleteMessage.gameObject.SetActive(true);
			DeleteText.gameObject.SetActive(true);
			DeleteText.text = "������ �����Ͱ� �����ϴ�";
		}
		// �÷��̾� �������� ������ 0���� ũ�ٸ�(�����Ͱ� �����Ѵٸ�)
		else
		{
			// ������ ����
			Managers.GData.Clear();

			DeleteMessage.gameObject.SetActive(true);
			DeleteText.gameObject.SetActive(true);
			DeleteText.text = "������ ���� �Ϸ�!";

			foreach (var t in playerName)
				t.text = $"������ ����";
			foreach (var t in playerLevel)
				t.text = $"Lev.XX";

			Managers.GData.SaveData();
		}

		Managers.CallWaitForSeconds(0.5f, () => {
			DeleteMessage.gameObject.SetActive(false);
			DeleteText.gameObject.SetActive(false);
		});

	}
	// ��������
	public void ExitGame()
	{
		Application.Quit();
	}

}
