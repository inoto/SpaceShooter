using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SpaceShooter;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceShooterReactive
{
	public class GameController_Reactive : MonoBehaviour
	{
		const string SaveFileName = "player_reactive.save";

		public static GameController_Reactive Instance;

		[SerializeField] GameConfig gameConfig = null;
		[SerializeField] GameObject level = null;
		[SerializeField] bool deleteSaveOnStart = false;

		public PlayerModel PlayerModel = null;
		public GameConfig GameConfig => gameConfig;

		Ship_Reactive shipReactive;
		Spawner_Reactive spawner;

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

			shipReactive = FindObjectOfType<Ship_Reactive>();
			shipReactive.Init();

			spawner = FindObjectOfType<Spawner_Reactive>();
			spawner.Init();
		}

		void Start()
		{
			level.SetActive(true);
			spawner.Run();
		}

		void OnApplicationQuit()
		{
			Save();
		}

		void GenerateLevelsData()
		{
			for (int i = 0; i < gameConfig.Levels.Length; i++)
			{
				var levelFromConfig = gameConfig.Levels[i];

				// asteroids
				List<GameObject> possibleAsteroids = new List<GameObject>(gameConfig.Asteroids.Length);
				possibleAsteroids.AddRange(gameConfig.Asteroids);

				PlayerModel.LevelModel.Add(new LevelModel());
				for (int ast = 0; ast < levelFromConfig.NumberOfAsteroidVariants; ast++)
				{
					int randomIndex = Random.Range(0, possibleAsteroids.Count);
					PlayerModel.LevelModel[i].Asteroids.Add(new StringReactiveProperty(possibleAsteroids[randomIndex].name));
					possibleAsteroids.RemoveAt(randomIndex);
				}

				// wave wait
				PlayerModel.LevelModel[i].SpawnWait.Value =
					Random.Range(levelFromConfig.SpawnWait.Min, levelFromConfig.SpawnWait.Max + 1);
				// asteroid speed
				PlayerModel.LevelModel[i].AsteroidSpeed.Value =
					Random.Range(levelFromConfig.AsteroidSpeed.Min, levelFromConfig.AsteroidSpeed.Max);
			}
		}

		void Save()
		{
			Stream stream = File.Open($"{Application.persistentDataPath}/{SaveFileName}", FileMode.OpenOrCreate);
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, PlayerModel);
			stream.Close();
		}

		void Load()
		{
			if (File.Exists($"{Application.persistentDataPath}/{SaveFileName}"))
			{
				if (deleteSaveOnStart)
				{
					File.Delete($"{Application.persistentDataPath}/{SaveFileName}");
					PlayerModel = new PlayerModel(gameConfig.ShipLifes);
					GenerateLevelsData();
					return;
				}

				try
				{
					Stream stream = File.Open($"{Application.persistentDataPath}/{SaveFileName}", FileMode.Open);
					BinaryFormatter formatter = new BinaryFormatter();
					// stream.Position = 0;
					PlayerModel = (PlayerModel) formatter.Deserialize(stream);
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

			PlayerModel = new PlayerModel(gameConfig.ShipLifes);
			GenerateLevelsData();
		}
	}
}