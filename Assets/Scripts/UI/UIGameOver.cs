using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
	public class UIGameOver : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI textTMP = null;
		[SerializeField] Button restartButton = null;
		[SerializeField] Button quitButton = null;

		void Awake()
		{
			restartButton.onClick.AddListener(OnRestartButtonPressed);
			quitButton.onClick.AddListener(OnQuitButtonPressed);
		}

		void Start()
		{
			GameController.Instance.GameOverEvent += Show;
			Hide();
		}

		void OnDestroy()
		{
			GameController.Instance.GameOverEvent -= Show;
		}

		void Show(bool success)
		{
			if (success)
			{
				textTMP.text = "LEVEL COMPLETED";
				restartButton.gameObject.SetActive(false);
			}
			else
			{
				textTMP.text = "GAME OVER";
				restartButton.gameObject.SetActive(true);
			}
			gameObject.SetActive(true);
		}

		void Hide()
		{
			gameObject.SetActive(false);
		}

		void OnRestartButtonPressed()
		{
			Hide();
			GameController.Instance.RestartLevel();
		}

		void OnQuitButtonPressed()
		{
			Hide();
			GameController.Instance.QuitLevel();
		}
	}
}