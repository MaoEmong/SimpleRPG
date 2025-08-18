using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ��Ÿ ��� enum��
public class Define
{
	// ���� ����
	public enum Sound
	{
		Bgm,
		Effect,
		MaxCount
	}
	// �� ����
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
	// ĳ������ �������
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
	// ĳ���Ͱ� �����ϴ� �ʵ��� ����
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

// �⺻ ������Ʈ �������ͽ�
[System.Serializable]
public class State
{
	// �̸��� ü���� �Ʒ��� �������ͽ��� �ٸ��� ���� �����ؾߵ�
	[SerializeField]
	string _name;
	public string Name { get { return _name; } set { _name = value; } }
	[SerializeField]
	int _maxhp;
	public int MaxHp { get { return _maxhp; } set { _maxhp = value; } }
	[SerializeField]
	int _hp;
	public int Hp { get { return _hp; } set { _hp = value; } }

	// ������� ��, ��ø, ����, ��
	[SerializeField]
	int _str;   // �� - ���ݷ� ���� ����
	public int Str { get { return _str; } set { _str = value; } }
	[SerializeField]
	int _dex;   // ��ø - ���ݼӵ�, �̵��ӵ� ����
	public int Dex { get { return _dex; } set { _dex = value; } }
	[SerializeField]
	int _sti;   // ���� - ü��, ���� ���� ����
	public int Sti { get { return _sti; } set { _sti = value; } }
	[SerializeField]
	int _luk;   // �� = ũ��Ƽ�� ���� ����
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
	// �� �������ͽ� 0���� �ٲ�
	public void Zero()
	{
		Str = 0; Dex = 0; Sti = 0; Luk = 0;
	}
	// ���� �������ͽ��� �ϳ��� �������͸� ����
	public void AddState(State value)
	{
		Str += value.Str;
		Dex += value.Dex;
		Sti += value.Sti;
		Luk += value.Luk;
	}
	// ���� �������ͽ��� �ΰ��� �������ͽ��� ����
	public void AddTwoState(State value1, State value2)
	{
		Str += value1.Str + value2.Str;
		Dex += value1.Dex + value2.Dex;
		Sti += value1.Sti + value2.Sti;
		Luk += value1.Luk + value2.Luk;
	}
}

// Ȯ��޼���
public static class ExtensionMethod
{
	// Enum���� string���� ��ȯ
	public static T ToEnum<T>(this string value)
	{
		if(!System.Enum.IsDefined(typeof(T), value))
			return default(T);

		return (T)System.Enum.Parse(typeof(T), value, true);
	}
}
