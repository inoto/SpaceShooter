using System.Collections.Generic;
using SpaceShooter;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooterReactive
{
	public class UILifesView : MonoBehaviour
	{
		[SerializeField] GameObject lifeTemplate = null;

		[Header("Image")]
		// [SerializeField] Sprite activeSprite = null;
		// [SerializeField] Color activeColor = Color.red;
		[SerializeField] Sprite inactiveSprite = null;
		[SerializeField] Color inactiveColor = Color.gray;

		List<Image> hearts;
		int currentIndex = 0;
		int lastLifes = 0;

		public void Init(int maxLifes)
		{
			lastLifes = maxLifes;
			hearts = new List<Image>(maxLifes);
			lifeTemplate.SetActive(false);
			for (int i = 0; i < maxLifes; i++)
			{
				hearts.Add(Instantiate(lifeTemplate, transform).GetComponent<Image>());
				hearts[hearts.Count - 1].gameObject.SetActive(true);
			}
		}

		public void UpdateHearts(int lifes)
		{
			if (lastLifes != lifes)
			{
				HeartLost(currentIndex++);
				lastLifes = lifes;
			}
		}

		void HeartLost(int index)
		{
			hearts[index].sprite = inactiveSprite;
			hearts[index].color = inactiveColor;
		}
	}
}