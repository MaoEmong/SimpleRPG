using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 게임데이터 관리 / 매니저 스크립트에서 관리
public class GameData
{
	// 플레이어 정보
	public class PlayerInfo
	{
		public string Name;
		public int Level;
		public int MaxHp;
		public int Hp;
		public int Str;
		public int Dex;
		public int Sti;
		public int Luk;
		public int RemainState;
		public int Exp;
		public int Money;
		public int UpgradeWeapon;
		public bool ShowFPS;

		public PlayerInfo()
		{
			Name = "Tester";
			Level = 0;
			MaxHp = 0;
			Hp = 0;
			Str = 0;
			Dex = 0;
			Sti = 0;
			Luk = 0;
			RemainState = 0;
			Money = 0;
			UpgradeWeapon = 0;
			ShowFPS = false;
		}
		public PlayerInfo(string name, int level, int maxhp, int hp, int str, int dex, int sti, int luk)
		{
			Name = name;
			Level = level;
			MaxHp = maxhp;
			Hp = hp;
			Str = str;
			Dex = dex;
			Sti = sti;
			Luk = luk;
			RemainState = 0;
			Exp = 0;
			Money = 0;
			UpgradeWeapon = 0;
			ShowFPS = false;
		}
		public PlayerInfo(string name, int level, int maxhp, int hp, int str, int dex, int sti, int luk, int remain, int exp, int weapon)
		{
			Name = name;
			Level = level;
			MaxHp = maxhp;
			Hp = hp;
			Str = str;
			Dex = dex;
			Sti = sti;
			Luk = luk;
			RemainState = remain;
			Exp = exp;
			Money = 0;
			UpgradeWeapon = weapon;
			ShowFPS = false;
		}
		public PlayerInfo(string name, int level, int maxhp, int hp, int str, int dex, int sti, int luk, int remain, int exp, int money, int weapon)
		{
			Name = name;
			Level = level;
			MaxHp = maxhp;
			Hp = hp;
			Str = str;
			Dex = dex;
			Sti = sti;
			Luk = luk;
			RemainState = remain;
			Exp = exp;
			Money = money;
			UpgradeWeapon = weapon;
			ShowFPS = false;
		}
		public void DeleteData()
		{
			Name = "Empty";
			Level = 0;
			MaxHp = 0;
			Hp = 0;
			Str = 0;
			Dex = 0;
			Sti = 0;
			Luk = 0;
			RemainState = 0;
			Exp = 0;
			Money = 0;
			UpgradeWeapon = 0;
			ShowFPS = false;
		}

		public void SetPlayerData(CharactorState state)
		{
			Name = state.CurState.Name;
			Level = state.Level;
			MaxHp = state.CurState.MaxHp;
			Hp = state.CurState.Hp;
			Str = state.OriginState.Str;
			Dex = state.OriginState.Dex;
			Sti = state.OriginState.Sti;
			Luk = state.OriginState.Luk;
			RemainState = state.RemainStat;
			Exp = state.Exp;
			Money= state.Money;
		}

	}

	public PlayerInfo playerinfo = new();

	public class EnemyBasicState
	{
		public string Name { get; set; }
		public int Level { get; set; }
		public int MaxHp { get; set; }
		public int Hp { get; set; }
		public int EXP { get; set; }
		public int STR { get; set; }
		public int Dex { get; set; }
		public int STI { get; set; }
		public int LUK { get; set; }
	}

	public class ItemClass
	{
		public int ItemCode = 0;
		public string ItemName = "";
		public string ItemExplan = "";
		public int Sell = 0;

		public ItemClass() { }
		public ItemClass(int itemCode, string itemName, string itemExplan, int sell)
		{
			ItemCode = itemCode;
			ItemName = itemName;
			ItemExplan = itemExplan;
			Sell = sell;
		}	
	}

	public class FeatData
	{
		// 업적 코드
		public int FeatCode;
		// 업적 설명
		public string FeatExplan;
		// 최대 수치
		public int MaxCount;
		// 최소 수치
		public int CurCount;

		public FeatData() { }

		public FeatData(int featCode, string featExplan, int maxCount, int curCount)
		{
			FeatCode = featCode;
			FeatExplan = featExplan;
			MaxCount = maxCount;
			CurCount = curCount;
		}

		public FeatData(int featCode, string featExplan, int maxCount)
		{
			FeatCode = featCode;
			FeatExplan = featExplan;
			MaxCount = maxCount;
			CurCount = 0;
		}

		public void AddCount()
		{
			CurCount++;
			if (CurCount >= MaxCount)
				CurCount = MaxCount;
		}
	}

	// 몬스터들의 데이터
	public List<EnemyBasicState> SlimeBasicState = new();
	public List<EnemyBasicState> MushroomBasicState = new();
	public List<EnemyBasicState> NeedleSnailBasicState = new();
	public List<EnemyBasicState> CactusBasicState = new();
	public List<EnemyBasicState> BeetleBasicState = new();
	public EnemyBasicState GreenDragonState = new();

	// 아이템데이터와 플레이어가 해당 아이템을 몇개 들고있는지 확인하기위한 데이터
	public List<ItemClass> ItemData = new();
	public List<MyTools.Pair<int, int>> PlayerItemData = new();

	// 업적 데이터
	public List<FeatData> PlayerFeat = new();

	// 이동(진행)방향 체크
	public bool isNext = true;

	// 최초 초기화
	public void Init()
	{
		// 플레이어 데이터 받아오기
		string Path;
#if UNITY_EDITOR
		Path = Application.dataPath;
#else
		Path = Application.persistentDataPath;
#endif
		// 플레이어 데이터 가져오기
		if(File.Exists($"{Path}/JsonData/PlayerInfo.json"))
		{
			Debug.Log("File is Find");
			playerinfo = Managers.Json.ImportJsonData<PlayerInfo>($"JsonData", "PlayerInfo");
			Debug.Log("Load File");
		}
		else
		{
			Debug.Log("File is NotFound");
			string playerdata = Managers.Json.ObjectToJson(playerinfo);
			Managers.Json.ExportJsonData("JsonData", "PlayerInfo", playerdata);
		}

		// 플레이어의 아이템 데이터 가져오기
		if (File.Exists($"{Path}/JsonData/PlayerItemData.json"))
		{
			Debug.Log("File is Find");
			PlayerItemData = Managers.Json.ImportJsonData<List<MyTools.Pair<int, int>>>($"JsonData", "PlayerItemData");
			Debug.Log("Load File");
		}
		else
		{
			Debug.Log("File is NotFound");
			for (int i = 0; i < 64; i++)
			{
				MyTools.Pair<int, int> Myitem = new();
				Myitem.First = i;
				Myitem.Second = 0;
				PlayerItemData.Add(Myitem);
			}
			string playerdata = Managers.Json.ObjectToJson(PlayerItemData);
			Managers.Json.ExportJsonData("JsonData", "PlayerItemData", playerdata);
		}
		
		// 플레이어의 업적 데이터 가져오기
		if (File.Exists($"{Path}/JsonData/PlayerFeat.json"))
		{
			Debug.Log("File is Find");
			PlayerFeat = Managers.Json.ImportJsonData<List<FeatData>>($"JsonData", "PlayerFeat");
			Debug.Log("Load File");
		}
		else
		{
			Debug.Log("File is NotFound");
			for (int i = 0; i < 6; i++)
			{
				FeatData feat = new(i,"",10);

				switch(i)
				{
					case 0:
						feat.FeatExplan = "슬라임 처치";
						break;
					case 1:
						feat.FeatExplan = "화난버섯 처치";

						break;
					case 2:
						feat.FeatExplan = "달팽이 처치";

						break;
					case 3:
						feat.FeatExplan = "선인장 처치";

						break;
					case 4:
						feat.FeatExplan = "딱정벌레 처치";

						break;
					case 5:
						feat.FeatExplan = "드래곤 처치";
						feat.MaxCount = 1;
						break;
				}
				PlayerFeat.Add(feat);
				

			}
			string playerfeat = Managers.Json.ObjectToJson(PlayerFeat);
			Managers.Json.ExportJsonData("JsonData", "PlayerFeat", playerfeat);
		}

		// 각각의 에너미 데이터 가져오기
		SlimeBasicState = Managers.Json.ImportdialogJsonData<List<EnemyBasicState>>("SlimeData");
		MushroomBasicState = Managers.Json.ImportdialogJsonData<List<EnemyBasicState>>("MushroomData");
		NeedleSnailBasicState = Managers.Json.ImportdialogJsonData<List<EnemyBasicState>>("NeedleSnailData");
		CactusBasicState = Managers.Json.ImportdialogJsonData<List<EnemyBasicState>>("CactusData");
		BeetleBasicState = Managers.Json.ImportdialogJsonData<List<EnemyBasicState>>("BeetleData");
		GreenDragonState = Managers.Json.ImportdialogJsonData<EnemyBasicState>("GreenDragonData");
		ItemData = Managers.Json.ImportdialogJsonData<List<ItemClass>>("ItemData");



		Debug.Log($"PlayerItemData.Count is {PlayerItemData.Count}");


	}
	// 게임 데이터 저장
	public void SaveData()
	{
		if(Managers.Player != null)
			playerinfo.SetPlayerData(Managers.Player.GetComponent<CharactorState>());

		string playerInfo = Managers.Json.ObjectToJson(playerinfo);
		Managers.Json.ExportJsonData("JsonData", "PlayerInfo", playerInfo);
		string playeritem = Managers.Json.ObjectToJson(PlayerItemData);
		Managers.Json.ExportJsonData("JsonData", "PlayerItemData", playeritem);
		string playerfeat = Managers.Json.ObjectToJson(PlayerFeat);
		Managers.Json.ExportJsonData("JsonData", "PlayerFeat", playerfeat);


	}
	// 게임 데이터 삭제
	public void Clear()
	{
		playerinfo = new();
		foreach (var n in PlayerItemData)
			n.Second = 0;
		foreach (var n in PlayerFeat)
			n.CurCount = 0;

		SaveData();
	}
}
