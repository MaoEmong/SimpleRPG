using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ĳ���� �⺻ ����
// �⺻����, �߰�����, ���罺������ ���еǸ�
// ���� ����Ǵ� ������ ���罺���̰�
// ���罺���� �⺻���� + �߰��������� �̷���� �ִ�
public class CharactorState : MonoBehaviour
{
	// �⺻ ���ݰ�
	public State OriginState;
	// ���� ���ݰ�
	public State CurState;
	// ������ ���ݰ�
	public State AddState;
	//���� ���ݰ� = �⺻ ���ݰ� + ������ ���ݰ�
	[SerializeField]
	int _level;
	public int Level { get { return _level; } set { _level = value; } }

	// ����ġ
	[SerializeField]
	int _exp;
	public int Exp { get { return _exp; } set { _exp = value; } }
	[SerializeField]
	int _maxexp;
	public int MaxExp { get { return _maxexp;} set { _maxexp = value; } }

	int _atk = 0;
	public int Atk { get { return _atk; } set { _atk = value; } }
	int _def = 0;
	public int Def { get { return _def; } set { _def = value; } }
	float _atkspeed = 0;
	public float AttackSpeed { get { return _atkspeed; } set { _atkspeed = value; } }
	float _critical = 0;
	public float Critical { get { return _critical; } set { _critical = value; } }

	int _remainstat = 0;
	public int RemainStat { get { return _remainstat; } set { _remainstat = value; } }
	int _money = 0;
	public int Money { get { return _money; } set { _money = value; } }
	int weapon = 0;
	public int Weapon { get { return weapon; } set { weapon = value;} }

	[SerializeField]
	ParticleSystem LevelUpParticle; 
	[SerializeField]
	ParticleSystem HealthUpParticle;

	// �ʱ�ȭ�Լ� / �����͸� �޾� ������
	public void Init(State Data, int lev)
	{
		OriginState = new(Data);
		AddState = new();
		CurState = OriginState + AddState;
		CurState.MaxHp = OriginState.MaxHp;
		CurState.Hp = OriginState.Hp;
		CurState.Name = OriginState.Name;
		Level = lev;
		MaxExp = Level * 10;
		Exp = Managers.GData.playerinfo.Exp;
		RemainStat = Managers.GData.playerinfo.RemainState;
		Money = Managers.GData.playerinfo.Money;
		Weapon = Managers.GData.playerinfo.UpgradeWeapon;

		SetOtherState();

		LevelUpParticle.Stop();
		HealthUpParticle.Stop();
	}
	// �⺻ ���ݿ� �� �߰�
	public void AddOriginStateValue(string tag, int val)
	{
		switch (tag)
		{
			case "Str":
				OriginState.Str += val;
				break;
			case "Dex":
				OriginState.Dex += val;
				break;
			case "Sti":
				OriginState.Sti += val;
				break;
			case "Luk":
				OriginState.Luk += val;
				break;
			default:
				Debug.Log($"{tag} is not Found!");
				break;
		}

		CurState.Zero();
		CurState.AddTwoState(OriginState, AddState);
		SetOtherState();
	}
	// �߰����ݿ� �� �߰�
	public void SetAddStateValue(string tag, int val)
	{
		switch (tag)
		{
			case "Str":
				AddState.Str = val;
				break;
			case "Dex":
				AddState.Dex = val;
				break;
			case "Sti":
				AddState.Sti = val;
				break;
			case "Luk":
				AddState.Luk = val;
				break;
			default:
				Debug.Log($"{tag} is not Found!");
				break;
		}
		CurState.Zero();
		CurState.AddTwoState(CurState, AddState);
		SetOtherState();
	}
	// �ɷ�ġ�� ���� ���ݰ� ����
	public virtual void SetOtherState()
	{
		CurState.MaxHp = 100 + (CurState.Sti * 10);
		Atk = 10 + (int)Mathf.Ceil(CurState.Str * 1.5f);
		Def = 10 + (int)Mathf.Ceil(CurState.Sti * 1.2f);
		AttackSpeed = 1.0f + (CurState.Dex * 0.03f);
		var anim = GetComponentInChildren<Animator>();
		anim.SetFloat("AttackSpeed", AttackSpeed);
		Critical = 0 + CurState.Luk * 0.3f;
		Atk += (Weapon * 5);
	}
	// ������ �ڵ�
	public void LevelUp()
	{
		Managers.Sound.Play("Effect/Player/LevelUp");

		CurState.Hp = CurState.MaxHp;

		// �߰� ����
		RemainStat += 5;
		// ������
		Level += 1;
		// �ʿ� ����ġ�� ����
		MaxExp = Level * 10;

		// ������ ��ƼŬ
		LevelUpParticle.Play();
		Managers.CallWaitForSeconds(1f, () =>{ LevelUpParticle.Stop(); });

	}
	// ü��ȸ�� ��ƼŬ��
	public void HealthUp()
	{
		HealthUpParticle.Play();
		Managers.CallWaitForSeconds(1f, () => { HealthUpParticle.Stop(); });

	}

}
