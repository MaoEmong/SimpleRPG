using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 큐브의 플레이어 체크 콜라이더
public class ItemCubeCheckPlayer : MonoBehaviour
{
    ItemCube cube;

    void Start()
    {
        cube = GetComponentInParent<ItemCube>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Managers.Sound.Play("Effect/Object/GetItem");
            Managers.GData.PlayerItemData[cube.ItemCode].Second++;
            Managers.Pool.Push(cube.gameObject);
            FindObjectOfType<MainCanvasUI>().GetItemType(cube.ItemCode);
        }
    }

}
