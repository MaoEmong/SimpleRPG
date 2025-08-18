using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// �ֺ� NPCüũ�� ��ũ��Ʈ
public class PlayerCheckNPC : MonoBehaviour
{
	// �÷��̾� ���� ��ũ��Ʈ
	CharactorMovement player;
	// �÷��̾��� �Ӹ� �� UI
	[SerializeField]
	Canvas playercanvas;
	// �ֺ��� ������ NPC������
	public NPCInteration npc;

	private void Start()
	{
		player = GetComponentInParent<CharactorMovement>();
	}

	private void LateUpdate()
	{
		// �÷��̾��� �Ӹ��� UI�� ����������
		if(playercanvas.gameObject.activeSelf)
		{
			// �÷��̾�� �ش� UI�� ����ٰ� �ϸ�
			if(player.HidePlayerUI)
			{
				// �ش��ϴ� UI�� ����
				playercanvas.gameObject.SetActive(false);
				return;
			}
			// �ƴ϶�� ī�޶� �ٶ󺸸� ������ rotation����
			playercanvas.transform.LookAt(Camera.main.transform);
		}
		// �÷��̾��� �ֺ��� NPC�� �ְ�, �Ӹ��� UI�� ������ �ʰڴ� ������
		else if(player.isAlmostNPC && !player.HidePlayerUI )
		{
			// �Ӹ��� UI�� �����ִ� ���¶��
			if (!playercanvas.gameObject.activeSelf)
				// UI Ȱ��ȭ
				playercanvas.gameObject.SetActive(true);
		}
	}

	// �浹�˻�
	private void OnTriggerEnter(Collider other)
	{
		// NPC�� ���� ���� ��������
		if (other.CompareTag("NPC"))
		{
			Debug.Log("Check! NPC");
			// �ش� NPC�� �����͸� �޾ƿ�
			player.isAlmostNPC = true;
			playercanvas.gameObject.SetActive(true);
			npc = other.gameObject.GetComponent<NPCInteration>();
			Managers.Sound.Play("Effect/Player/MeetNpc");
		}
	}
	private void OnTriggerExit(Collider other)
	{
		// NPC�� �������� �����ٸ�
		if (other.CompareTag("NPC"))
		{
			Debug.Log("Exit! NPC");
			// �ش� NPC�� ������ ����
			player.isAlmostNPC = false;
			playercanvas.gameObject.SetActive(false);
			npc = null;
		}
	}
}
