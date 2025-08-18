using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 씬의 기본 베이스 스크립트
public abstract class SceneBase : MonoBehaviour
{
    // 현재 씬의 정보
    public Define.SceneType type = Define.SceneType.Unknown;

    void Awake()
    {
        Init();
    }

    protected virtual void Start()
    {
        // 새로운 씬 시작 시 오토 세이브 기능
        StartCoroutine(AutoSave());
    }

    protected virtual void Init()
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
		{
            GameObject go = new GameObject() { name = "EventSystem" };
            go.AddComponent<EventSystem>();
		}
        // 시간정지 해제
        Managers.TimePlay();
    }

    public abstract void Clear();

    IEnumerator AutoSave()
    {
        // 10분에 한번 플레이어 데이터 세이브
        yield return new WaitForSeconds(600.0f);

        Managers.GData.SaveData();

    }

}
