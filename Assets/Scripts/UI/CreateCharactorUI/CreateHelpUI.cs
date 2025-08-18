using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 도움말 UI
public class CreateHelpUI : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
	[SerializeField]
	GameObject[] Panels;
	// 인텍스번호
	int idx = 0;

	void Start()
	{
		for(int i = 0; i < Panels.Length; i++) 
		{
			if(i == idx)
			{
				Panels[i].SetActive(true);
			}
			else
				Panels[i].SetActive(false);
		}
	}

	// 화면 터치 액션을 위한 인터페이스클래스
	public void OnPointerDown(PointerEventData eventData)
	{

	}

	// 화면 터치 액션을 위한 인터페이스클래스
	public void OnPointerUp(PointerEventData eventData)
	{
		idx++;
		if (idx == Panels.Length)
		{
			idx = 0;
			gameObject.SetActive(false);
		}
		RefreshPanels();
	}

	// 현재 인덱스 번호에 맞춰 Panel 설정
	void RefreshPanels()
	{
		for (int i = 0; i < Panels.Length; i++)
		{
			if (i == idx)
			{
				Panels[i].SetActive(true);
			}
			else
				Panels[i].SetActive(false);
		}

	}

}
