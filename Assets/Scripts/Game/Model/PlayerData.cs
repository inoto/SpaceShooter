namespace SpaceShooter
{
	[System.Serializable]
	public class PlayerData
	{
		public int ProgressLevelIndex = 0;
		public bool ProgressCompleted = false;
		public LevelData[] LevelData;
	}
}