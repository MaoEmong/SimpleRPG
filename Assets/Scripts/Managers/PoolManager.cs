using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 풀매니저
public class PoolManager
{
	// 오브젝트 풀
	public class Pool
	{
		// 원본 오브젝트
		public GameObject OriginPoolObject;
		// 관리용 부모Transform
		public Transform Root;
		// 오브젝트 관리용 stack
		Stack<GameObject> _poolStack = new Stack<GameObject>();

		// 초기화(원본 오브젝트, 미리 만들어 둘 갯수)
		public void Init(GameObject origin, int count = 3)
		{
			OriginPoolObject = origin;
			Root = new GameObject().transform;
			Root.name = $"{origin.name}_Root";

			for (int i = 0; i < count; i++)
				Push(Create());
		}
		// 원본 오브젝트의 복사본 생성
		GameObject Create()
		{
			GameObject go = Object.Instantiate(OriginPoolObject);
			go.name = OriginPoolObject.name;
			return go;
		}
		// 데이터 넣어두기
		public void Push(GameObject go)
		{
			if (go == null)
				return;

			go.transform.parent = Root;
			go.SetActive(false);
			
			_poolStack.Push(go);
		}
		// 데이터 가져가기
		public GameObject Pop(Transform _root)
		{
			GameObject go;

			// 미리 만들어둔 데이터가 전부 소모됬을 경우 추가생성
			if(_poolStack.Count > 0)
				go = _poolStack.Pop();
			else
				go = Create();

			go.SetActive(true);

			if (_root == null)
				go.transform.parent = Managers.Scene.CurScene.transform;

			go.transform.parent = _root;

			return go;
		}
	}

	// Dictionary<오브젝트이름,데이터풀>
	Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();
	// 데이터 풀의 부모
	Transform _root;
	// 활성화된 데이터의 부모
	Transform _ableRoot;
	

	public void Init()
	{
		if(_root == null)
		{
			_root = new GameObject { name = "Pool_Root" }.transform;
			Object.DontDestroyOnLoad(_root);
		}
		if(_ableRoot == null)
		{
			_ableRoot = new GameObject { name = "Able_Pool_Root" }.transform;
			Object.DontDestroyOnLoad(_ableRoot);
		}
	}

	// 데이터 풀 생성(원본 오브젝트가 존재하지 않을 시에만)
	public void CreatePool(GameObject origin, int count = 3)
	{
		if (_pools.ContainsKey(origin.name) == false)
		{
			Pool pool = new Pool();
			pool.Init(origin, count);
			pool.Root.parent = _root;

			_pools.Add(origin.name, pool);
		}
	}
	// 데이터 집어넣기
	public void Push(GameObject go)
	{
		string name = go.name;
		// 집어넣는 데이터의 풀이 존재하지 않다면 해당 데이터는 삭제
		if(_pools.ContainsKey(name) == false)
		{
			GameObject.Destroy(go);
			return;
		}

		_pools[name].Push(go);
	}
	// 데이터 가져가기
	public GameObject Pop(GameObject go, Transform root = null)
	{
		// 해당하는 오브젝트가 존재하는 풀이 없다면 생성
		if (_pools.ContainsKey(go.name) == false)
			CreatePool(go);

		Transform Root = root;

		if (Root == null)
			Root = _ableRoot;

		return _pools[go.name].Pop(Root);
	}

	// 활성화, 비활성화된 오브젝트 풀 제거
	public void Clear()
	{
		foreach(Transform t in _root)
			GameObject.Destroy(t.gameObject);
		foreach(Transform t in _ableRoot)
			GameObject.Destroy(t.gameObject);

		_pools.Clear();
	}
}
