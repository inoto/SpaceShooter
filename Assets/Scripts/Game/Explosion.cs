using System.Collections;
using UnityEngine;

namespace SpaceShooter
{
	public class Explosion : MonoBehaviour
	{
		[SerializeField] float despawnAfterTime = 2f;

		IEnumerator Start()
		{
			yield return new WaitForSeconds(despawnAfterTime);
			SimplePool.Despawn(gameObject);
		}
	}
}