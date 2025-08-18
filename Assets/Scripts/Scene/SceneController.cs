using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 씬 관리용 컨트롤러(씬 매니저로 사용중이나 SceneManager와 겹치기에 이름 바꿈)
public class SceneController
{
	// 현재 재생중인 씬의 정보
	public SceneBase CurScene;
	// 설정한 씬으로 이동
	public void LeadScene(Define.SceneType type)
	{
		Managers.Clear();

		// 씬 전환 시 플레이어 데이터 저장
		Managers.GData.SaveData();

		// 비동기 씬 전환
		SceneManager.LoadSceneAsync(GetSceneName(type));
	}

	// enum값을 string으로 변환
	string GetSceneName(Define.SceneType type)
	{
		string name = Enum.GetName(typeof(Define.SceneType), type);
		return name;
	}
	// 현재 씬 재시작
	public void RestartScene()
	{
		Managers.Clear();

		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
	}

	public void Clear()
	{
		CurScene.Clear();
		CurScene = null;
	}
}
