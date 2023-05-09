using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MainData
{
	[System.Serializable]
	public class Record
	{
		[Header("Player Data")]
		public Vector3 playerPosition;
		public Vector3 playerRotation;
		public int playerKitchenObjectsCount;
		[Header("Counters Data")]
		public int cookingCounterKitchenObjectsCount;
	}
	public Record settings;
}

public static class GameDataManager
{
	public static void Set()
	{
		Debug.Log("Set");
		MainData data = new MainData();
		data.settings = new MainData.Record();
		data.settings.playerPosition = GameData.Instance.playerPosition;
		data.settings.playerRotation = GameData.Instance.playerRotation;
		data.settings.playerKitchenObjectsCount = GameData.Instance.playerKitchenObjectsCount;
		data.settings.cookingCounterKitchenObjectsCount = GameData.Instance.cookingCounterKitchenObjectsCount;
		string rawSetData = JsonUtility.ToJson(data);
		PlayerPrefs.SetString("Setting", rawSetData);
		PlayerPrefs.Save();
	}
	public static void Get()
	{
		Debug.Log("Get");
		if (PlayerPrefs.HasKey("Setting"))
		{
			string rawGetData = PlayerPrefs.GetString("Setting");
			MainData data = ParseData(rawGetData);
			GameData.Instance.playerPosition = data.settings.playerPosition;
			GameData.Instance.playerRotation = data.settings.playerRotation;
			GameData.Instance.playerKitchenObjectsCount = data.settings.playerKitchenObjectsCount;
			GameData.Instance.cookingCounterKitchenObjectsCount = data.settings.cookingCounterKitchenObjectsCount;
		}
		else
		{
			ApplyDefaultGameSettings();
		}
	}

    private static void ApplyDefaultGameSettings()
    {
		Debug.Log("Default settings");
		GameData.Instance.playerPosition = Vector3.zero;
		GameData.Instance.playerRotation = Vector3.zero;
		GameData.Instance.playerKitchenObjectsCount = 0;
		GameData.Instance.cookingCounterKitchenObjectsCount = 0;
    }

    private static MainData ParseData(string rawGetData)
	{
		MainData data;
		try
		{
			data = JsonUtility.FromJson<MainData>(rawGetData);
			Debug.Log("In Set : \n" + data);
		}
		catch
		{
			data = null;
		}
		return data;
	}
}
