using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �׽�Ʈ�� ��ũ��Ʈ
public class TestScripts : MonoBehaviour
{
	public ScrollRect scrollRect;
	public Vector2 scrollPosition;

	private void Start()
	{
		scrollRect = GetComponent<ScrollRect>();
		scrollRect.normalizedPosition = new Vector2(1f, 1f);
	}


}
