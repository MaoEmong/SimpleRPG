using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// NPC스크립트
public class NPCInteration : MonoBehaviour
{
    // 현재 NPC의 코드
    public int NPCCode = 0;
    // NPC 타입
    public Define.NPCType type = Define.NPCType.Talk;

    // 메인카메라가 바라볼 NPC의 머리
    [SerializeField]
    Transform NPC_Head;
    // NPC의 이름 UI
    [SerializeField]
    GameObject NamePanel;
    // 카메라 위치
    [SerializeField]
    Transform SetPlayerPos;
    // NPC의 대화문
    [SerializeField]
    NPCUI NPCsUI;


    private void Start()
    {
        NPCsUI.Init(this);
		NPCsUI.gameObject.SetActive(false);
    }
    // 플레이어에서 콜백하는 함수
    public void CallNPCUI(CharactorMovement player)
    {
        StartCoroutine(NPCAction(player));
        
    }
    // 대화가 끝났을때 콜백함수
    public void EndNpcTalk()
    {
        StartCoroutine(EndTalk());
    }

    IEnumerator NPCAction(CharactorMovement player)
    {
        // 1. 화면 어두워짐
        SceneMain main = Managers.Scene.CurScene.GetComponent<SceneMain>();
        main.BlackImage.gameObject.SetActive(true);
        StartCoroutine(MyTools.ImageFadeIn(main.BlackImage,1.0f));
        yield return new WaitForSeconds(1.0f);

        //2. 사운드 출력, 카메라 위지및 회전값 조절, NPC전용UI 출력
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

    // 대화 종료
    IEnumerator EndTalk()
    {
        // 1.화면 어두워짐, 사운드 출력
		SceneMain main = Managers.Scene.CurScene.GetComponent<SceneMain>();
		main.BlackImage.gameObject.SetActive(true);
		StartCoroutine(MyTools.ImageFadeIn(main.BlackImage, 1.0f));
        Managers.Sound.Play("Effect/NPC/TalkEnd");
		yield return new WaitForSeconds(1.0f);

        // 카메라 위치, 회전값 정상화, NPC전용UI 비활성화, 화면 밝아짐
		NamePanel.SetActive(true);
		Camera.main.gameObject.GetComponent<CameraMovement>().isLookPlayer = true;
        Managers.Player.GetComponent<CharactorMovement>().HidePlayerUI = false;
		NPCsUI.gameObject.SetActive(false);

		StartCoroutine(MyTools.ImageFadeOut(main.BlackImage, 1.0f));
		main.BlackImage.gameObject.SetActive(false);

	}

}
