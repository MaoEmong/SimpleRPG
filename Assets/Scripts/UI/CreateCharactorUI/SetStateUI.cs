using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 캐릭터 생성 씬
// 1차 스텟 설정 UI
// 모티브는 메이플스토리 스텟 주사위 룰렛
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
		// 전체값 4로 초기화
		for (int i = 0; i < state.Length; i++)
		{
			state[i] = 4;
			stateText[i].text = $"{state[i]}";
		}

		RollState();

	}

	// 총 스테이터스의 합은 23
	// 각 스테이터스는 최소4, 최대9의 수치를 지님
	// 메이플스토리 캐릭터 생성 스테이터스 다이스 롤링 방식
	public void RollState()
	{
		Managers.Sound.Play("Effect/UI/RollingDice");

		// 총합은 23
		int totalMaxValue = 23;

		for(int i = 0; i < state.Length; i++) 
		{
			// 4~9 사이값 랜덤
			int value = Random.Range(4, 10);
			// 현재 설정된 스테이터스 총합
			int total;
			total = 0;
			// 현재까지 설정된 스테이터스들의 총합값 계산
			for (int j = 0; j < state.Length; j++)
			{
				if(j == i){ continue;	}
				else
				total += state[j];
			}
			// 마지막 스테이터스 설정 시
			if (i == state.Length - 1)
			{
				// 현재 남은 여유 스테이터스가 9보다 크다면
				if (totalMaxValue - total > 9)
				{
					// 처음부터 다시
					i = -1;
					// 값 4로 전체 초기화
					for (int j = 0; j < state.Length; j++)
					{
						state[j] = 4;
					}
				}
				//  남은 여유 스테이터스가 9보다 작다면
				else
				{
					// 남은 스테이터를 모두 적용
					state[i] = totalMaxValue - total;
				}
			}
			else
			{
				// 랜덤값이 총합 - 현재 스테이터스합 보다 작거나 같을때 랜덤값 적용
				if (value <= totalMaxValue - total)
				{
					state[i] = value;
				}

				// 랜덤값이 총합 - 현재 스테이터스합 보다 크다면
				else
				{
					// 현재 스테이터스 다시 리롤
					i--;
				}
			}

		}

		// 텍스트 적용
		for (int i = 0; i < state.Length; i++)
		{
			stateText[i].text = $"{state[i]}";
		}

	}
	// 다음으로 넘어가면 현재 스텟 적용
	public void EnterState()
	{
		int Total = 0;
		foreach (int var in state)
			Total += var;

		if(Total < 23)
		{
			if (!ErrorImage.gameObject.activeSelf)
			{
				ErrorText.text = "잘못된 값입니다. 스텟을 재설정해주세요.";
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
