using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// NPC��ũ��Ʈ
public class NPCInteration : MonoBehaviour
{
    // ���� NPC�� �ڵ�
    public int NPCCode = 0;
    // NPC Ÿ��
    public Define.NPCType type = Define.NPCType.Talk;

    // ����ī�޶� �ٶ� NPC�� �Ӹ�
    [SerializeField]
    Transform NPC_Head;
    // NPC�� �̸� UI
    [SerializeField]
    GameObject NamePanel;
    // ī�޶� ��ġ
    [SerializeField]
    Transform SetPlayerPos;
    // NPC�� ��ȭ��
    [SerializeField]
    NPCUI NPCsUI;


    private void Start()
    {
        NPCsUI.Init(this);
		NPCsUI.gameObject.SetActive(false);
    }
    // �÷��̾�� �ݹ��ϴ� �Լ�
    public void CallNPCUI(CharactorMovement player)
    {
        StartCoroutine(NPCAction(player));
        
    }
    // ��ȭ�� �������� �ݹ��Լ�
    public void EndNpcTalk()
    {
        StartCoroutine(EndTalk());
    }

    IEnumerator NPCAction(CharactorMovement player)
    {
        // 1. ȭ�� ��ο���
        SceneMain main = Managers.Scene.CurScene.GetComponent<SceneMain>();
        main.BlackImage.gameObject.SetActive(true);
        StartCoroutine(MyTools.ImageFadeIn(main.BlackImage,1.0f));
        yield return new WaitForSeconds(1.0f);

        //2. ���� ���, ī�޶� ������ ȸ���� ����, NPC����UI ���
        int val = Random.Range(0, 2);
        if(val > 0) { Managers.Sound.Play("Effect/NPC/TalkStart1"); }
        else
			Managers.Sound.Play("Effect/NPC/TalkStart2");

		NamePanel.SetActive(false);
        Camera.main.gameObject.GetComponent<CameraMovement>().isLookPlayer = false;
        Camera.main.transform.position = SetPlayerPos.position;
        player.HidePlayerUI = true;
        Camera.main.transform.LookAt(NPC_Head);
		NPCsUI.Init(this);
		NPCsUI.gameObject.SetActive(true);

        StartCoroutine(MyTools.ImageFadeOut(main.BlackImage, 1.0f));
        main.BlackImage.gameObject.SetActive(false);
    }

    // ��ȭ ����
    IEnumerator EndTalk()
    {
        // 1.ȭ�� ��ο���, ���� ���
		SceneMain main = Managers.Scene.CurScene.GetComponent<SceneMain>();
		main.BlackImage.gameObject.SetActive(true);
		StartCoroutine(MyTools.ImageFadeIn(main.BlackImage, 1.0f));
        Managers.Sound.Play("Effect/NPC/TalkEnd");
		yield return new WaitForSeconds(1.0f);

        // ī�޶� ��ġ, ȸ���� ����ȭ, NPC����UI ��Ȱ��ȭ, ȭ�� �����
		NamePanel.SetActive(true);
		Camera.main.gameObject.GetComponent<CameraMovement>().isLookPlayer = true;
        Managers.Player.GetComponent<CharactorMovement>().HidePlayerUI = false;
		NPCsUI.gameObject.SetActive(false);

		StartCoroutine(MyTools.ImageFadeOut(main.BlackImage, 1.0f));
		main.BlackImage.gameObject.SetActive(false);

	}

}
