using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 주변 NPC체크용 스크립트
public class PlayerCheckNPC : MonoBehaviour
{
	// 플레이어 관리 스크립트
	CharactorMovement player;
	// 플레이어의 머리 위 UI
	[SerializeField]
	Canvas playercanvas;
	// 주변에 들어오는 NPC데이터
	public NPCInteration npc;

	private void Start()
	{
		player = GetComponentInParent<CharactorMovement>();
	}

	private void LateUpdate()
	{
		// 플레이어의 머리위 UI가 켜져있을때
		if(playercanvas.gameObject.activeSelf)
		{
			// 플레이어에서 해당 UI를 숨긴다고 하면
			if(player.HidePlayerUI)
			{
				// 해당하는 UI를 숨김
				playercanvas.gameObject.SetActive(false);
				return;
			}
			// 아니라면 카메라를 바라보며 일정한 rotation유지
			playercanvas.transform.LookAt(Camera.main.transform);
		}
		// 플레이어의 주변에 NPC가 있고, 머리위 UI를 숨기지 않겠다 했을떄
		else if(player.isAlmostNPC && !player.HidePlayerUI )
		{
			// 머리위 UI가 꺼져있는 상태라면
			if (!playercanvas.gameObject.activeSelf)
				// UI 활성화
				playercanvas.gameObject.SetActive(true);
		}
	}

	// 충돌검사
	private void OnTriggerEnter(Collider other)
	{
		// NPC가 범위 내에 들어왔을때
		if (other.CompareTag("NPC"))
		{
			Debug.Log("Check! NPC");
			// 해당 NPC의 데이터를 받아옴
			player.isAlmostNPC = true;
			playercanvas.gameObject.SetActive(true);
			npc = other.gameObject.GetComponent<NPCInteration>();
			Managers.Sound.Play("Effect/Player/MeetNpc");
		}
	}
	private void OnTriggerExit(Collider other)
	{
		// NPC가 범위에서 나간다면
		if (other.CompareTag("NPC"))
		{
			Debug.Log("Exit! NPC");
			// 해당 NPC의 데이터 날림
			player.isAlmostNPC = false;
			playercanvas.gameObject.SetActive(false);
			npc = null;
		}
	}
}
