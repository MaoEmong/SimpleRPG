using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 기타 등등 enum값
public class Define
{
	// 사운드 종류
	public enum Sound
	{
		Bgm,
		Effect,
		MaxCount
	}
	// 씬 종류
	public enum SceneType
	{
		Unknown,
		TitleScene,
		CreateScene,
		MainScene,
		ForestFieldScene,
		DesertFieldScene,
		PlainFieldScene,


	}
	// 캐릭터의 현재상태
	public enum CharacterState
	{
		Idle,
		Move,
		Attack,
		Hit,
		Die,
		Attack1,
		Attack2,
		Attack3,
	}
	// 캐릭터가 존재하는 필드의 상태
	public enum FieldType
	{
		Town,
		Field,

	}

	public enum EnemyType
	{
		Unknown,
		Slime,
		Mushroom,
		NeedleSnail,
		Cactus,
		Beetle,
		GreenDragon,

	}

	public enum NPCType
	{
		Talk,
		Shop,
		Upgrade,
	}

	public enum ShopItemList
	{
		HealthPotion,

	}

	public enum UpgradeType
	{
		Weapon,

	}
}

// 기본 오브젝트 스테이터스
[System.Serializable]
public class State
{
	// 이름과 체력은 아래의 스테이터스와 다르게 따로 관리해야됨
	[SerializeField]
	string _name;
	public string Name { get { return _name; } set { _name = value; } }
	[SerializeField]
	int _maxhp;
	public int MaxHp { get { return _maxhp; } set { _maxhp = value; } }
	[SerializeField]
	int _hp;
	public int Hp { get { return _hp; } set { _hp = value; } }

	// 순서대로 힘, 민첩, 강도, 운
	[SerializeField]
	int _str;   // 힘 - 공격력 관련 스텟
	public int Str { get { return _str; } set { _str = value; } }
	[SerializeField]
	int _dex;   // 민첩 - 공격속도, 이동속도 스텟
	public int Dex { get { return _dex; } set { _dex = value; } }
	[SerializeField]
	int _sti;   // 강도 - 체력, 방어력 관련 스텟
	public int Sti { get { return _sti; } set { _sti = value; } }
	[SerializeField]
	int _luk;   // 운 = 크리티컬 관련 스텟
	public int Luk { get { return _luk; } set { _luk = value; } }

	public State() { Name = ""; MaxHp = 0; Hp = MaxHp; Str = 0; Dex = 0; Sti = 0; Luk = 0; }
	public State(string _Name,int _MaxHp, int _hp, int _Str, int _Dex, int _Sti, int _Luk) 
	{
		Name = _Name; MaxHp = _MaxHp; Hp = _hp; Str = _Str; Dex = _Dex; Sti = _Sti; Luk = _Luk;
	}
	public State(string _Name,  int _Str, int _Dex, int _Sti, int _Luk)
	{
		Name = _Name; Str = _Str; Dex = _Dex; Sti = _Sti; Luk = _Luk; MaxHp = Sti * 10; Hp = MaxHp;
	}
	public State(State state)
	{
		Name = state.Name; MaxHp = state.MaxHp; Hp = state.Hp; Str = state.Str; Dex = state.Dex; Sti = state.Sti; Luk = state.Luk;
	}
	public static State operator +(State state1, State state2)
	{
		State result = new State();

		result.Str  = state1.Str + state2.Str;
		result.Dex  = state1.Dex + state2.Dex;
		result.Sti  = state1.Sti + state2.Sti;
		result.Luk  = state1.Luk + state2.Luk;

		return result;
	}
	// 각 스테이터스 0으로 바꿈
	public void Zero()
	{
		Str = 0; Dex = 0; Sti = 0; Luk = 0;
	}
	// 현재 스테이터스에 하나의 스테이터를 더함
	public void AddState(State value)
	{
		Str += value.Str;
		Dex += value.Dex;
		Sti += value.Sti;
		Luk += value.Luk;
	}
	// 현재 스테이터스에 두개의 스태이터스를 더함
	public void AddTwoState(State value1, State value2)
	{
		Str += value1.Str + value2.Str;
		Dex += value1.Dex + value2.Dex;
		Sti += value1.Sti + value2.Sti;
		Luk += value1.Luk + value2.Luk;
	}
}

// 확장메서드
public static class ExtensionMethod
{
	// Enum값을 string으로 변환
	public static T ToEnum<T>(this string value)
	{
		if(!System.Enum.IsDefined(typeof(T), value))
			return default(T);

		return (T)System.Enum.Parse(typeof(T), value, true);
	}
}
