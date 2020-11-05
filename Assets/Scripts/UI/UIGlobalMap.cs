using System;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
	public class UIGlobalMap : MonoBehaviour
	{
		[SerializeField] GameConfig gameConfig = null;
		[SerializeField] Button[] buttons = null;

		[Header("Colors")]
		[SerializeField] Color doneLevelColor = Color.white;

		PlayerData playerData;

		void Awake()
		{
			if (gameConfig.Levels.Length != buttons.Length)
				throw new Exception("Number of levels in config NOT equal number of buttons on a Global map");

			for (int i = 0; i < buttons.Length; i++)
			{
				var i1 = i;
				buttons[i].onClick.AddListener(() => SelectLevel(i1));
			}

			playerData = GameController.Instance.PlayerData;
		}

		void Start()
		{
			GameController.Instance.QuitLevelEvent += Show;
			CheckProgress();
		}

		void OnDestroy()
		{
			GameController.Instance.QuitLevelEvent -= Show;
		}

		void Show()
		{
			gameObject.SetActive(true);
			CheckProgress();
		}

		void CheckProgress()
		{
			for (int i = 0; i < buttons.Length; i++)
			{
				if (i > playerData.ProgressLevelIndex)
					continue;

				buttons[i].interactable = true;
				if (i < playerData.ProgressLevelIndex)
				{
					buttons[i].GetComponent<Image>().color = doneLevelColor;
				}
			}
		}

		void SelectLevel(int index)
		{
			gameObject.SetActive(false);
			GameController.Instance.StartLevel(index);
		}
	}
}