using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeatPanel : MonoBehaviour
{
	public GameObject Container;

	public ScrollRect scoll;

	private void Start()
	{
		scoll.normalizedPosition = new Vector2(1, 1);

	}

	private void OnEnable()
	{
		Clear();


		CreateSlot();

		scoll.normalizedPosition = new Vector2(1, 1);

	}

	void CreateSlot()
	{
		for(int i = 0; i < Managers.GData.PlayerFeat.Count;i++)
		{
			if (Managers.GData.PlayerFeat[i].CurCount >= Managers.GData.PlayerFeat[i].MaxCount)
			{
				continue;
			}
			else
			{
				GameObject obj = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Prefabs/UI/FeatSlot"),Container.transform);
				obj.GetComponent<FeatSlot>().Init(Managers.GData.PlayerFeat[i].FeatCode);				
			}
		}
		for (int i = 0; i < Managers.GData.PlayerFeat.Count; i++)
		{
			if (Managers.GData.PlayerFeat[i].CurCount < Managers.GData.PlayerFeat[i].MaxCount)
			{
				continue;
			}
			else
			{
				GameObject obj = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Prefabs/UI/FeatSlot"), Container.transform);
				obj.GetComponent<FeatSlot>().Init(Managers.GData.PlayerFeat[i].FeatCode);
			}
		}

	}

	void Clear()
	{
		foreach(Transform child in Container.transform)
		{
			Destroy(child.gameObject);
		}
	}

}
