using SpaceShooter;
using UniRx;
using UnityEngine;

namespace SpaceShooterReactive
{
	public class LifesPresenter : MonoBehaviour
	{
		[SerializeField] UILifesView view = null;

		PlayerModel model;

		void Start()
		{
			model = GameController_Reactive.Instance.PlayerModel;

			view.Init(GameController_Reactive.Instance.GameConfig.ShipLifes);

			model.Lifes
				.ObserveEveryValueChanged(x => x.Value)
				.Subscribe(xs =>
				{
					view.UpdateHearts(xs);
				})
				.AddTo(this);
		}
	}
}