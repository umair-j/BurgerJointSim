using System.Collections.Generic;
using UnityEngine;
	[CreateAssetMenu(fileName = "Game Data", menuName = "Game Data", order = 51)]
	public class GameData : ScriptableObject
	{
		private static GameData _instance = null;
		public static GameData Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = (GameData)Resources.Load("Game Data");
					if (_instance == null)
					{
						throw new UnityException("Asset can't found");
					}
				}
				return _instance;
			}
		}

		[Header("Player Attributes")]

		public Vector3 playerPosition;
		public Vector3 playerRotation;
		public int playerKitchenObjectsCount;
		public int cookingCounterKitchenObjectsCount;

}
