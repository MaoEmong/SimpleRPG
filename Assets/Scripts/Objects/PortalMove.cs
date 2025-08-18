using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ��Ż ��Ʈ��Ʈ
public class PortalMove : MonoBehaviour
{
	// �̵��� ���� ����
	public Define.SceneType nextScene;

	// Ÿ�̸�
	[SerializeField]
	float timer = 0f;
	// Ÿ�̸� ���� ����
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

		// �浹���� ������Ʈ�� �±װ� Player���
		if(other.CompareTag("Player"))
		{
			// Ÿ�̸� ����
			timer += Time.deltaTime;
			// ������ �ð���ŭ �帥 ��
			if(timer > targettime)
			{
				// ȭ�� ���� �� ���̵�(�̵� �� ���� �÷��̾� ������ ����, �̵� �� ����� �÷��̾� �����ͷ� ���ο��÷��̾� ��ü�� ������ ����)
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
		// �÷��̾ �ش� ������Ʈ���� ���������ٸ� 
		if(other.CompareTag("Player"))
		{
			// Ÿ�̸� �ʱ�ȭ
			timer = 0f;
		}
	}

}
