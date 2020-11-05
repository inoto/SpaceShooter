using System.Collections;
using System.Collections.Generic;
using SpaceShooter;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
	public GameObject explosionPrefab;

	void Awake()
	{

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Instantiate(explosionPrefab, other.transform.position, other.transform.rotation);
			SimplePool.Despawn(other.gameObject);
		}
	}
}
