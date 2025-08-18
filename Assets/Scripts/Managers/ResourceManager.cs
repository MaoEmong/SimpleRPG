using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 리소스 관리 매니저
public class ResourceManager
{
	// 설정 경로에 존재하는 데이더 가져오기
   public T Load<T>(string path) where T : Object
	{
		return Resources.Load<T>(path);
	}
	// 설정한 경로에 존재하는 오브젝트 생성
	public GameObject Instantiate(string path, Transform parent = null)
	{
		GameObject prefab = Load<GameObject>($"Prefabs/{path}");
		if(prefab == null)
		{
			Debug.Log($"Failed Load Prefab : Prefabs/{path}");
			return null;
		}

		return Object.Instantiate(prefab, parent);
	}
	// 오브젝트 파괴
	public void Destroy(GameObject go)
	{
		if (go == null)
			return;

		Object.Destroy(go);
	}
}
