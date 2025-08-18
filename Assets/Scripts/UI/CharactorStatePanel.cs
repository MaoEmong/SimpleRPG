using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 캐릭터 상태 UI
public class CharactorStatePanel : MonoBehaviour
{
	// 각 텍스트의 설정 방향
	enum TextType
	{
		Name,
		Level,
		Hp,
		Str,
		Dex,
		Sti,
		Luk,
		Atk,
		Def,
		AtkSpeed,
		Critical,
	}
	// 텍스트들
	public Text[] PanelTexts;

	// 스텟 추가 UI
	[SerializeField]
	GameObject UpgradePanel;
	[SerializeField]
	Text UpgradePointText;

	public Text PlusAtkText;

	private void OnEnable()
	{
		// 현재 플레이어 데이터에서 추가스텟이 남아있는지 확인
		CharactorState playerstate = Managers.Player.GetComponent<CharactorState>();
		// 남아있다면 스텟 추가 UI 활성화
		if (playerstate.RemainStat > 0)
		{
			UpgradePanel.SetActive(true);
			UpgradePointText.text = $"{playerstate.RemainStat}";
		}
		else
			UpgradePanel.SetActive(false);

		if (Managers.GData.playerinfo.UpgradeWeapon <= 0)
			PlusAtkText.gameObject.SetActive(false);
		else
			PlusAtkText.gameObject.SetActive(true);

	}

	// 플레이어 스텟 텍스트화
	public void SetState()
	{
		CharactorState playerstate = Managers.Player.GetComponent<CharactorState>();

		PanelTexts[(int)TextType.Name].text = $"이름 : {playerstate.OriginState.Name}";
		PanelTexts[(int)TextType.Level].text = $"레벨 : {playerstate.Level}";
		PanelTexts[(int)TextType.Hp].text = $"Hp : {playerstate.CurState.Hp} / {playerstate.CurState.MaxHp}";
		PanelTexts[(int)TextType.Str].text = $"Str : {playerstate.CurState.Str}";
		PanelTexts[(int)TextType.Dex].text = $"Dex : {playerstate.CurState.Dex}";
		PanelTexts[(int)TextType.Sti].text = $"Sti : {playerstate.CurState.Sti}";
		PanelTexts[(int)TextType.Luk].text = $"Luk : {playerstate.CurState.Luk}";
		PanelTexts[(int)TextType.Atk].text = $"공격력 : {playerstate.Atk}";
		PanelTexts[(int)TextType.Def].text = $"방어력 : {playerstate.Def}";
		PanelTexts[(int)TextType.AtkSpeed].text = $"공격속도 : {playerstate.AttackSpeed:F1}";
		PanelTexts[(int)TextType.Critical].text = $"치명확률 : {playerstate.Critical:F1}";
		PlusAtkText.text =$"+ {Managers.GData.playerinfo.UpgradeWeapon * 5}";

	}

	// 설정한 스텟에 포인트 추가
	public void SetState(string tag)
	{
		CharactorState playerstate = Managers.Player.GetComponent<CharactorState>();
		if (playerstate.RemainStat <= 0)
			return;

		Managers.Sound.Play("Effect/UI/UIClick");

		playerstate.AddOriginStateValue(tag, 1);
		playerstate.RemainStat--;
		UpgradePointText.text = $"{playerstate.RemainStat}";

		SetState();

		if(playerstate.RemainStat <= 0)
			UpgradePanel.SetActive(false);
	}

}
