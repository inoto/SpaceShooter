using System.Collections;
using UnityEngine;

namespace SpaceShooter
{
	public class Asteroid : MonoBehaviour
	{
		[SerializeField] GameObject explosionPrefab = null;

		GameObject explosion;

		void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Boundary"))
				return;

			SimplePool.Spawn(explosionPrefab, transform.position, Quaternion.identity);
			SimplePool.Despawn(gameObject);
		}

		void OnTriggerExit(Collider other)
		{
			if (!other.CompareTag("Boundary"))
				return;

			SimplePool.Despawn(gameObject);
		}
	}
}