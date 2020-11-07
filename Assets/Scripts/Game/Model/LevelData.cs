using UnityEngine;

namespace SpaceShooter
{
	[System.Serializable]
	public class LevelData
	{
		public string[] Asteroids = null;
		public float SpawnWait = 1f;
		public float AsteroidSpeed = -5f;
	}
}