using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 캐릭터 기본 스텟
// 기본스텟, 추가스텟, 현재스텟으로 구분되며
// 실제 적용되는 스텟은 현재스텟이고
// 현재스텟은 기본스텟 + 추가스텟으로 이루어져 있다
public class CharactorState : MonoBehaviour
{
	// 기본 스텟값
	public State OriginState;
	// 현재 스텟값
	public State CurState;
	// 증가한 스텟값
	public State AddState;
	//현재 스텟값 = 기본 스텟값 + 증가한 스텟값
	[SerializeField]
	int _level;
	public int Level { get { return _level; } set { _level = value; } }

	// 경험치
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

	// 초기화함수 / 데이터를 받아 설정함
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
	// 기본 스텟에 값 추가
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
	// 추가스텟에 값 추가
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
	// 능력치에 따른 스텟값 설정
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
	// 레벨업 코드
	public void LevelUp()
	{
		Managers.Sound.Play("Effect/Player/LevelUp");

		CurState.Hp = CurState.MaxHp;

		// 추가 스텟
		RemainStat += 5;
		// 레벨업
		Level += 1;
		// 필요 경험치량 증가
		MaxExp = Level * 10;

		// 레벨업 파티클
		LevelUpParticle.Play();
		Managers.CallWaitForSeconds(1f, () =>{ LevelUpParticle.Stop(); });

	}
	// 체력회복 파티클용
	public void HealthUp()
	{
		HealthUpParticle.Play();
		Managers.CallWaitForSeconds(1f, () => { HealthUpParticle.Stop(); });

	}

}
