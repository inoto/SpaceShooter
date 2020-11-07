using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
	public class UILifes : MonoBehaviour
	{
		[SerializeField] GameObject lifeTemplate = null;

		[Header("Image")]
		[SerializeField] Sprite activeSprite = null;
		[SerializeField] Color activeColor = Color.red;
		[SerializeField] Sprite inactiveSprite = null;
		[SerializeField] Color inactiveColor = Color.gray;

		PlayerController player;
		GameConfig gameConfig;
		List<Image> lifes;
		int currentIndex = 0;

		void Awake()
		{
			gameConfig = GameController.Instance.GameConfig;

			lifes = new List<Image>(gameConfig.ShipLifes);
			lifeTemplate.SetActive(false);
			for (int i = 0; i < gameConfig.ShipLifes; i++)
			{
				lifes.Add(Instantiate(lifeTemplate, transform).GetComponent<Image>());
				lifes[lifes.Count - 1].gameObject.SetActive(true);
			}

			player = FindObjectOfType<PlayerController>();
		}

		void Start()
		{
			player.LifeLostEvent += OnLifeLost;
			GameController.Instance.StartLevelEvent += Show;
			GameController.Instance.QuitLevelEvent += Hide;
			Hide();
		}

		void OnDestroy()
		{
			player.LifeLostEvent -= OnLifeLost;
			GameController.Instance.StartLevelEvent -= Show;
			GameController.Instance.QuitLevelEvent -= Hide;
		}

		public void OnLifeLost(int lifesLeft)
		{
			lifes[currentIndex].sprite = inactiveSprite;
			lifes[currentIndex].color = inactiveColor;
			currentIndex += 1;
		}

		void Show()
		{
			gameObject.SetActive(true);
			for (int i = 0; i < lifes.Count; i++)
			{
				lifes[i].sprite = activeSprite;
				lifes[i].color = activeColor;
				lifes[i].gameObject.SetActive(true);
			}
			currentIndex = 0;
		}

		void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}