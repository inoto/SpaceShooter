using UnityEngine;

namespace SpaceShooter
{
	[CreateAssetMenu(fileName = "GameConfig", menuName = "SpaceShooter/GameConfig", order = 0)]
	public class GameConfig : ScriptableObject
	{
		public int ShipLifes = 3;
		public GameObject[] Asteroids;

		[System.Serializable]
		public class LevelSettings
		{
			public int NumberOfAsteroidVariants = 1;
			public MinMaxFloat SpawnWait = new MinMaxFloat();
			public MinMaxFloat AsteroidSpeed = new MinMaxFloat();

			public int AsteroidsToWin = 10;
		}

		public LevelSettings[] Levels;
	}
}