using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ĳ���� ���� UI
public class CharactorStatePanel : MonoBehaviour
{
	// �� �ؽ�Ʈ�� ���� ����
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
	// �ؽ�Ʈ��
	public Text[] PanelTexts;

	// ���� �߰� UI
	[SerializeField]
	GameObject UpgradePanel;
	[SerializeField]
	Text UpgradePointText;

	public Text PlusAtkText;

	private void OnEnable()
	{
		// ���� �÷��̾� �����Ϳ��� �߰������� �����ִ��� Ȯ��
		CharactorState playerstate = Managers.Player.GetComponent<CharactorState>();
		// �����ִٸ� ���� �߰� UI Ȱ��ȭ
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

	// �÷��̾� ���� �ؽ�Ʈȭ
	public void SetState()
	{
		CharactorState playerstate = Managers.Player.GetComponent<CharactorState>();

		PanelTexts[(int)TextType.Name].text = $"�̸� : {playerstate.OriginState.Name}";
		PanelTexts[(int)TextType.Level].text = $"���� : {playerstate.Level}";
		PanelTexts[(int)TextType.Hp].text = $"Hp : {playerstate.CurState.Hp} / {playerstate.CurState.MaxHp}";
		PanelTexts[(int)TextType.Str].text = $"Str : {playerstate.CurState.Str}";
		PanelTexts[(int)TextType.Dex].text = $"Dex : {playerstate.CurState.Dex}";
		PanelTexts[(int)TextType.Sti].text = $"Sti : {playerstate.CurState.Sti}";
		PanelTexts[(int)TextType.Luk].text = $"Luk : {playerstate.CurState.Luk}";
		PanelTexts[(int)TextType.Atk].text = $"���ݷ� : {playerstate.Atk}";
		PanelTexts[(int)TextType.Def].text = $"���� : {playerstate.Def}";
		PanelTexts[(int)TextType.AtkSpeed].text = $"���ݼӵ� : {playerstate.AttackSpeed:F1}";
		PanelTexts[(int)TextType.Critical].text = $"ġ��Ȯ�� : {playerstate.Critical:F1}";
		PlusAtkText.text =$"+ {Managers.GData.playerinfo.UpgradeWeapon * 5}";

	}

	// ������ ���ݿ� ����Ʈ �߰�
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
