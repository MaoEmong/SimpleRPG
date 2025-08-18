using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 마지막스테이지 포탈
public class DevelopPortal : MonoBehaviour
{
	[SerializeField]
	Image DevelopMessage;

	private void Start()
	{
		DevelopMessage.gameObject.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag("Player"))
        {
			if (!DevelopMessage.gameObject.activeSelf)
			{
				DevelopMessage.gameObject.SetActive(true);
				Managers.TimePause();
			}

		}
	}
}
