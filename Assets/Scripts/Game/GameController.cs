using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SpaceShooter;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace SpaceShooter
{
	public class GameController : MonoBehaviour
	{
		const string SaveFileName = "player.save";

		public static GameController Instance;

		public event Action<bool> GameOverEvent;
		public event Action QuitLevelEvent;
		public event Action StartLevelEvent;

		[SerializeField] GameConfig gameConfig = null;
		[SerializeField] GameObject level = null;
		[SerializeField] bool deleteSaveOnStart = false;

		public PlayerData PlayerData = null;
		public GameConfig GameConfig => gameConfig;

		PlayerController player;
		Spawner spawner;
		int currentlevelIndex = 0;

		void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				Destroy(gameObject);
			}

			Load();

			player = FindObjectOfType<PlayerController>();
			player.Init();

			spawner = FindObjectOfType<Spawner>();
			spawner.Init();
		}

		void Start()
		{
			level.SetActive(false);
			player.LifeLostEvent += OnLifeLost;
			player.EnoughKillsEvent += OnEnoughKills;
		}

		void OnDestroy()
		{
			player.LifeLostEvent -= OnLifeLost;
			player.EnoughKillsEvent -= OnEnoughKills;
		}

		void OnApplicationQuit()
		{
			Save();
		}

		public void StartLevel(int index)
		{
			currentlevelIndex = index;
			level.SetActive(true);
			spawner.Run();
			StartLevelEvent?.Invoke();
		}

		public void QuitLevel()
		{
			if (PlayerData.ProgressLevelIndex <= currentlevelIndex
				&& PlayerData.ProgressLevelIndex < PlayerData.LevelData.Length - 1)
				PlayerData.ProgressLevelIndex += 1;
			player.ResetAll();
			spawner.Stop();
			QuitLevelEvent?.Invoke();
		}

		public void RestartLevel()
		{
			level.SetActive(true);
			player.ResetAll();
			spawner.Stop();
			spawner.Run();
			StartLevelEvent?.Invoke();
		}

		void StopLevel()
		{
			spawner.Stop();
			level.SetActive(false);
		}

		void OnLifeLost(int lifes)
		{
			if (lifes > 0)
				return;

			StopLevel();
			GameOverEvent?.Invoke(false);
		}

		void OnEnoughKills()
		{
			StopLevel();
			GameOverEvent?.Invoke(true);
		}

		void GenerateLevelsData()
		{
			PlayerData.LevelData = new LevelData[gameConfig.Levels.Length];
			for (int i = 0; i < gameConfig.Levels.Length; i++)
			{
				var levelFromConfig = gameConfig.Levels[i];

				// asteroids
				List<GameObject> possibleAsteroids = new List<GameObject>(gameConfig.Asteroids.Length);
				possibleAsteroids.AddRange(gameConfig.Asteroids);

				PlayerData.LevelData[i] = new LevelData
				{
					Asteroids = new string[levelFromConfig.NumberOfAsteroidVariants]
				};
				for (int ast = 0; ast < levelFromConfig.NumberOfAsteroidVariants; ast++)
				{
					int randomIndex = Random.Range(0, possibleAsteroids.Count);
					PlayerData.LevelData[i].Asteroids[ast] = possibleAsteroids[randomIndex].name;
					possibleAsteroids.RemoveAt(randomIndex);
				}

				// wave wait
				PlayerData.LevelData[i].SpawnWait =
					Random.Range(levelFromConfig.SpawnWait.Min, levelFromConfig.SpawnWait.Max + 1);
				// asteroid speed
				PlayerData.LevelData[i].AsteroidSpeed =
					Random.Range(levelFromConfig.AsteroidSpeed.Min, levelFromConfig.AsteroidSpeed.Max);
			}
		}

		void Save()
		{
			Stream stream = File.Open($"{Application.persistentDataPath}/{SaveFileName}", FileMode.OpenOrCreate);
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, PlayerData);
			stream.Close();
		}

		void Load()
		{
			if (File.Exists($"{Application.persistentDataPath}/{SaveFileName}"))
			{
				if (deleteSaveOnStart)
				{
					File.Delete($"{Application.persistentDataPath}/{SaveFileName}");
					PlayerData = new PlayerData();
					GenerateLevelsData();
					return;
				}

				try
				{
					Stream stream = File.Open($"{Application.persistentDataPath}/{SaveFileName}", FileMode.Open);
					BinaryFormatter formatter = new BinaryFormatter();
					// stream.Position = 0;
					PlayerData = (PlayerData) formatter.Deserialize(stream);
					stream.Close();
					return;
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}
				finally
				{
					File.Delete($"{Application.persistentDataPath}/{SaveFileName}");
				}
			}

			PlayerData = new PlayerData();
			GenerateLevelsData();
		}
	}
}
