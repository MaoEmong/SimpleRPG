using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvasUI : MonoBehaviour
{
	// 플레이어 인풋용
	[SerializeField]
	CharactorMovement playerMove;
	float Skill1curTime = 0.0f;
	bool isReady = true;


	public GameObject PlayerState;

	// 플레이어 기본 정보
	[SerializeField]
	Text Name;
	[SerializeField]
	Text Level;
	[SerializeField]
	Text Hp;
	[SerializeField]
	Image HpBar;
	[SerializeField]
	Image ExpBar;
	[SerializeField]
	Text PotionCountText;
	[SerializeField]
	GameObject RemainPoint;

	public Transform GetItemTransform;

	public List<GameObject> UIobj;

	public Image SkillDelay1Image;
	public Image AttackImage;

	public void Init(CharactorMovement player)
	{
		playerMove = player;

		Name.text = playerMove.MyState.OriginState.Name;
		Level.text = $"Lv.{playerMove.MyState.Level}";
		Hp.text = $"{playerMove.MyState.CurState.Hp} / {playerMove.MyState.CurState.MaxHp}";
		PotionCountText.text = $"X{Managers.GData.PlayerItemData[15].Second}";
		HpBar.fillAmount = playerMove.MyState.CurState.Hp / (float)playerMove.MyState.CurState.MaxHp;
		ExpBar.fillAmount = playerMove.MyState.Exp / (float)playerMove.MyState.MaxExp;
	}

	private void Update()
	{
		// 플레이어 사망 확인 시 UI 기본 오브젝트들 전부 가리기
		if(playerMove.isDie)
		{
			foreach (var n in UIobj)
				n.gameObject.SetActive(false);
			return;
		}
		Level.text = $"Lv.{playerMove.MyState.Level}";
		Hp.text = $"{playerMove.MyState.CurState.Hp} / {playerMove.MyState.CurState.MaxHp}";
		PotionCountText.text = $"X{Managers.GData.PlayerItemData[15].Second}";
		HpBar.fillAmount = playerMove.MyState.CurState.Hp / (float)playerMove.MyState.CurState.MaxHp;
		ExpBar.fillAmount = playerMove.MyState.Exp / (float)playerMove.MyState.MaxExp;

		if(playerMove.GetComponent<CharactorState>().RemainStat>0)
		{
			RemainPoint.SetActive(true);
		}
		else
		{
			RemainPoint.SetActive(false);
		}

		if (playerMove.fieldtype == Define.FieldType.Town)
			AttackImage.gameObject.SetActive(false);
		else
			AttackImage.gameObject.SetActive(true);

	}

	public void CallPlayerAttack()
	{
		playerMove.CallAttack();
	}

	public void CallPlayerSkill1()
	{
		if (!isReady)
			return;
		if (playerMove.fieldtype == Define.FieldType.Town)
			return;
		isReady = false;

		playerMove.CallSkill1();

		StartCoroutine(Skill1Delay());
	}

	IEnumerator Skill1Delay()
	{
		float curTime = playerMove.Skill1DelayTime;
		float delayTime = playerMove.Skill1DelayTime;

		while (curTime > 0)
		{
			yield return null;

			curTime -= Time.deltaTime;
			SkillDelay1Image.fillAmount = curTime / playerMove.Skill1DelayTime;

		}

		isReady = true;
	}


	// 체력 회복
	public void UsePotion()
	{
		// 포션이 없거나
		if (Managers.GData.PlayerItemData[15].Second <= 0)
			return;
		// 체력이 꽉차있다면 return
		else if (playerMove.MyState.CurState.Hp >= playerMove.MyState.CurState.MaxHp)
		{
			Managers.Sound.Play("Effect/UI/NotAccess");
			return;
		}
		else
		{
			Managers.Sound.Play("Effect/Player/UsePotion");
			Managers.GData.PlayerItemData[15].Second--;
			playerMove.MyState.CurState.Hp += 100;
			playerMove.MyState.HealthUp();
			if (playerMove.MyState.CurState.Hp >= playerMove.MyState.CurState.MaxHp)
			{
				playerMove.MyState.CurState.Hp = playerMove.MyState.CurState.MaxHp;
			}
		}
	}

	public void CallButtonClickSound()
	{
		Managers.Sound.Play("Effect/UI/UIClick2");
	}

	// 아이템 습득 시 콜백
	public void GetItemType(int itemCode)
	{
		GameObject obj = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Prefabs/UI/GetItemType"), transform);
		obj.transform.position = GetItemTransform.position;
		obj.GetComponent<GetItemType>().Init(itemCode, PlayerState.transform.position);
	}

}
