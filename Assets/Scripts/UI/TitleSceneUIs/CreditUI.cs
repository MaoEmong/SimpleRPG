using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreditUI : MonoBehaviour,IPointerUpHandler,IPointerDownHandler
{
	public Text CreditText;

	public Vector3 TextPos;
	Vector3 MoveUp;
	public float speed = 4.0f;

    void Start()
    {
		TextPos = CreditText.rectTransform.position;
		MoveUp = Vector3.up * speed;
	}

	void Update()
    {
		MoveUp = Vector3.up * speed;

		if (CreditText.rectTransform.position.y <= GetComponent<Image>().rectTransform.rect.yMax)
		{
			CreditText.rectTransform.position += MoveUp;

		}

	}

	public void OnPointerDown(PointerEventData eventData)
	{
		speed = 6.0f;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		speed = 4.0f;
	}

	public void CallCloseButton()
	{
		CreditText.rectTransform.position = TextPos;
		gameObject.SetActive(false);
	}
}
