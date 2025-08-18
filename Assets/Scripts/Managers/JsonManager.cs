using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;

// Json Init
/*
		DataClass.UpgradeSystem UpgradeSystemJson = new DataClass.UpgradeSystem(1, 1, 1, 1);
		DataClass.CharactorUpgradeSystem[] CharactorSystemJson = new DataClass.CharactorUpgradeSystem[5]
		{
			new DataClass.CharactorUpgradeSystem(1,1,1),
			new DataClass.CharactorUpgradeSystem(1,1,1),
			new DataClass.CharactorUpgradeSystem(1,1,1),
			new DataClass.CharactorUpgradeSystem(1,1,1),
			new DataClass.CharactorUpgradeSystem(1,1,1)
		};

		string UpgradeJson = ObjectToJson(UpgradeSystemJson);
		string CharactorJson = ObjectToJson(CharactorSystemJson);
		Debug.Log(UpgradeJson);
		Debug.Log("==============");
		Debug.Log(CharactorJson);
		Debug.Log("==============");

		ExportJsonData("JsonData", "UpgradeJson", UpgradeJson);
		ExportJsonData("JsonData", "CharactorUpgradeJson", CharactorJson);

		===============================

		GameData.PlayerInfo PlayerInfo = new();
		string PlayerInfoJson = ObjectToJson(PlayerInfo);
		ExportJsonData("JsonData", "PlayerInfo", PlayerInfoJson);
		
		

*/
// Json데이터 관리용 매니저
public class JsonManager
{

	public void Init()
	{

	}
	// Json으로 변환
	public string ObjectToJson(object obj)
	{
		return JsonConvert.SerializeObject(obj);
	}
	// Json데이터를 원하는 데이터로 변환
	public T JsonToObject<T>(string jsonData)
	{
		return JsonConvert.DeserializeObject<T>(jsonData);
	}
	// Json데이터 저장
	public void ExportJsonData(string filePath, string fileName, string jsonData)
	{
		string Path;

#if UNITY_EDITOR
		Path = Application.dataPath;
#else
		Path = Application.persistentDataPath;
#endif

		if (!Directory.Exists(Path + $"/{filePath}"))
		{
			Directory.CreateDirectory(Path + $"/{filePath}");
		}
		FileStream fileStream = new FileStream(string.Format($"{Path}/{filePath}/{fileName}.json"), FileMode.Create);
		byte[] data = Encoding.UTF8.GetBytes(jsonData);
		fileStream.Write(data, 0, data.Length);
		fileStream.Close();
	}
	// Json데이터 불러오기
	public T ImportJsonData<T>(string loadPath, string fileName)
	{
		string Path;
#if UNITY_EDITOR
		Path = Application.dataPath;
#else
		Path = Application.persistentDataPath;
#endif
		Debug.Log($"{Path}/{loadPath}/{fileName}.json");
		FileStream fileStream = new FileStream(string.Format($"{Path}/{loadPath}/{fileName}.json"), FileMode.Open);
		byte[] data = new byte[fileStream.Length];
		fileStream.Read(data, 0, data.Length);
		fileStream.Close();
		string jsonData = Encoding.UTF8.GetString(data);
		return JsonConvert.DeserializeObject<T>(jsonData);
	}

	/// <summary>
	///  읽기 전용 Json데이터
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="fileName"></param>
	/// <returns></returns>
	public T ImportdialogJsonData<T>(string fileName)
	{		 
		TextAsset jsontext = Resources.Load<TextAsset>($"JsonData/{fileName}");
		T MyText = JsonToObject<T>(jsontext.ToString());
		return MyText;


	}


}
