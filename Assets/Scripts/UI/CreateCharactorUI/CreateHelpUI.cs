using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// ���� UI
public class CreateHelpUI : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
	[SerializeField]
	GameObject[] Panels;
	// ���ؽ���ȣ
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

	// ȭ�� ��ġ �׼��� ���� �������̽�Ŭ����
	public void OnPointerDown(PointerEventData eventData)
	{

	}

	// ȭ�� ��ġ �׼��� ���� �������̽�Ŭ����
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

	// ���� �ε��� ��ȣ�� ���� Panel ����
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
