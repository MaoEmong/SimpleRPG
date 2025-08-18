using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ĳ���� ���� ��
// 1�� ���� ���� UI
// ��Ƽ��� �����ý��丮 ���� �ֻ��� �귿
public class SetStateUI : MonoBehaviour
{
	// Str, Dex, Sti, Luk
	[SerializeField]
	int[] state = new int[4];
	[SerializeField]
	Text[] stateText = new Text[4];

	[SerializeField]
	Image ErrorImage;
	[SerializeField]
	Text ErrorText;

	private void Start()
	{
		// ��ü�� 4�� �ʱ�ȭ
		for (int i = 0; i < state.Length; i++)
		{
			state[i] = 4;
			stateText[i].text = $"{state[i]}";
		}

		RollState();

	}

	// �� �������ͽ��� ���� 23
	// �� �������ͽ��� �ּ�4, �ִ�9�� ��ġ�� ����
	// �����ý��丮 ĳ���� ���� �������ͽ� ���̽� �Ѹ� ���
	public void RollState()
	{
		Managers.Sound.Play("Effect/UI/RollingDice");

		// ������ 23
		int totalMaxValue = 23;

		for(int i = 0; i < state.Length; i++) 
		{
			// 4~9 ���̰� ����
			int value = Random.Range(4, 10);
			// ���� ������ �������ͽ� ����
			int total;
			total = 0;
			// ������� ������ �������ͽ����� ���հ� ���
			for (int j = 0; j < state.Length; j++)
			{
				if(j == i){ continue;	}
				else
				total += state[j];
			}
			// ������ �������ͽ� ���� ��
			if (i == state.Length - 1)
			{
				// ���� ���� ���� �������ͽ��� 9���� ũ�ٸ�
				if (totalMaxValue - total > 9)
				{
					// ó������ �ٽ�
					i = -1;
					// �� 4�� ��ü �ʱ�ȭ
					for (int j = 0; j < state.Length; j++)
					{
						state[j] = 4;
					}
				}
				//  ���� ���� �������ͽ��� 9���� �۴ٸ�
				else
				{
					// ���� �������͸� ��� ����
					state[i] = totalMaxValue - total;
				}
			}
			else
			{
				// �������� ���� - ���� �������ͽ��� ���� �۰ų� ������ ������ ����
				if (value <= totalMaxValue - total)
				{
					state[i] = value;
				}

				// �������� ���� - ���� �������ͽ��� ���� ũ�ٸ�
				else
				{
					// ���� �������ͽ� �ٽ� ����
					i--;
				}
			}

		}

		// �ؽ�Ʈ ����
		for (int i = 0; i < state.Length; i++)
		{
			stateText[i].text = $"{state[i]}";
		}

	}
	// �������� �Ѿ�� ���� ���� ����
	public void EnterState()
	{
		int Total = 0;
		foreach (int var in state)
			Total += var;

		if(Total < 23)
		{
			if (!ErrorImage.gameObject.activeSelf)
			{
				ErrorText.text = "�߸��� ���Դϴ�. ������ �缳�����ּ���.";
				ErrorImage.gameObject.SetActive(true);
				StartCoroutine(MyTools.ImageFadeOut(ErrorImage, 1.3f));
				Managers.CallWaitForSeconds(1.5f, () => { ErrorImage.gameObject.SetActive(false); });
				Managers.Sound.Play("Effect/UI/NotAccess");
			}
			return;

		}

		Managers.GData.playerinfo.Str = state[0];
		Managers.GData.playerinfo.Dex = state[1];
		Managers.GData.playerinfo.Sti = state[2];
		Managers.GData.playerinfo.Luk = state[3];
	}
}
