using System;
using UnityEngine;

namespace SpaceShooter
{
	public class Bolt : MonoBehaviour
	{
		public event Action<Bolt, bool> HitEvent;

		void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Enemy"))
			{
				HitEvent?.Invoke(this, true);
				SimplePool.Despawn(gameObject);
			}
		}

		void OnTriggerExit(Collider other)
		{
			if (!other.CompareTag("Boundary"))
				return;

			HitEvent?.Invoke(this, false);
			SimplePool.Despawn(gameObject);
		}
	}
}