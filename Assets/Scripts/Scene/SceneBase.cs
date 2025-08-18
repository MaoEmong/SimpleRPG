using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// ���� �⺻ ���̽� ��ũ��Ʈ
public abstract class SceneBase : MonoBehaviour
{
    // ���� ���� ����
    public Define.SceneType type = Define.SceneType.Unknown;

    void Awake()
    {
        Init();
    }

    protected virtual void Start()
    {
        // ���ο� �� ���� �� ���� ���̺� ���
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
        // �ð����� ����
        Managers.TimePlay();
    }

    public abstract void Clear();

    IEnumerator AutoSave()
    {
        // 10�п� �ѹ� �÷��̾� ������ ���̺�
        yield return new WaitForSeconds(600.0f);

        Managers.GData.SaveData();

    }

}
