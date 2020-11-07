using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace SpaceShooter
{
	[System.Serializable]
	public class PlayerModel
	{
		public IntReactiveProperty ProgressLevelIndex;
		public IntReactiveProperty Lifes;
		public List<LevelModel> LevelModelList;
		public ReactiveCollection<LevelModel> LevelModel;

		public PlayerModel(int lifes)
		{
			ProgressLevelIndex = new IntReactiveProperty(0);
			Lifes = new IntReactiveProperty(lifes);
			LevelModelList = new List<LevelModel>();
			LevelModel = new ReactiveCollection<LevelModel>(LevelModelList);
		}
	}

	[System.Serializable]
	public class LevelModel
	{
		public List<StringReactiveProperty> AsteroidsList;
		public ReactiveCollection<StringReactiveProperty> Asteroids;
		public FloatReactiveProperty SpawnWait;
		public FloatReactiveProperty AsteroidSpeed;

		public LevelModel()
		{
			AsteroidsList = new List<StringReactiveProperty>();
			Asteroids = new ReactiveCollection<StringReactiveProperty>(AsteroidsList);
			SpawnWait = new FloatReactiveProperty(1f);
			AsteroidSpeed = new FloatReactiveProperty(-5f);
		}
	}
}