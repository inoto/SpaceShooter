using System.Collections;
using System.Collections.Generic;
using SpaceShooter;
using UnityEngine;

namespace SpaceShooterReactive
{
	public class Spawner_Reactive : MonoBehaviour
	{
		[SerializeField] Vector3 spawnValues = Vector3.zero;
		[SerializeField] int asteroidCount = 10;
		[SerializeField] float startWait = 0.75f;
		[SerializeField] float waveWait = 3f;
		[SerializeField] GameObject asteroidExplosionPrefab = null;
		[SerializeField] Transform asteroidsContainer = null;

		GameConfig gameConfig;
		PlayerModel playerModel;
		LevelModel currentLevelData;

		public void Init()
		{
			gameConfig = GameController_Reactive.Instance.GameConfig;
			playerModel = GameController_Reactive.Instance.PlayerModel;
			currentLevelData = playerModel.LevelModel[playerModel.ProgressLevelIndex.Value];

			for (int i = 0; i < gameConfig.Asteroids.Length; i++)
			{
				SimplePool.Preload(gameConfig.Asteroids[i], asteroidsContainer, 5 * (playerModel.ProgressLevelIndex.Value + 1));
			}
			SimplePool.Preload(asteroidExplosionPrefab, 5);
		}

		public void Run()
		{
			var activeAsteroids = FindObjectsOfType<Asteroid>();
			for (int i = 0; i < activeAsteroids.Length; i++)
				SimplePool.Despawn(activeAsteroids[i].gameObject);

			StartCoroutine(SpawnWaves());
		}

		IEnumerator SpawnWaves()
		{
			yield return new WaitForSeconds(startWait);

			List<GameObject> asteroids = new List<GameObject>(currentLevelData.Asteroids.Count);
			for (int i = 0; i < currentLevelData.Asteroids.Count; i++)
			{
				for (int j = 0; j < gameConfig.Asteroids.Length; j++)
				{
					if (!currentLevelData.Asteroids[i].Value.Equals(gameConfig.Asteroids[j].name))
						continue;

					asteroids.Add(gameConfig.Asteroids[j]);
				}
			}

			GameObject asteroidPrefab;
			GameObject spawnedGO;

			while (true)
			{
				for (int i = 0; i < asteroidCount; i++)
				{
					asteroidPrefab = asteroids[Random.Range(0, asteroids.Count)];

					Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);

					spawnedGO = SimplePool.Spawn(asteroidPrefab, spawnPosition, Quaternion.identity);
					spawnedGO.GetComponent<Mover>().speed = currentLevelData.AsteroidSpeed.Value;

					yield return new WaitForSeconds(currentLevelData.SpawnWait.Value);
				}

				yield return new WaitForSeconds(waveWait);
			}
		}
	}
}