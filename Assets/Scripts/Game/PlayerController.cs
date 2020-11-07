using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
	public class PlayerController : MonoBehaviour
	{
		public event Action<int> LifeLostEvent;
		public event Action EnoughKillsEvent;

		[SerializeField] float fireRate = 0.5F;
		[SerializeField] float speed = 10f;
		[SerializeField] float tilt = 5f;
		[SerializeField] Boundary boundary = null;
		[SerializeField] GameObject boltPrefab = null;
		[SerializeField] Transform shotSpawn = null;
		[SerializeField] GameObject explosionPrefab = null;
		[SerializeField] Transform boltsContainer = null;

		PlayerData playerData;
		GameConfig gameConfig;
		float myTime = 0.0F;
		float nextFire = 0.5F;
		Rigidbody rb;
		AudioSource audioSource;
		int lifes;
		Vector3 initialPosition;
		int kills = 0;

		public void Init()
		{
			playerData = GameController.Instance.PlayerData;
			gameConfig = GameController.Instance.GameConfig;

			rb = GetComponent<Rigidbody>();
			audioSource = GetComponent<AudioSource>();

			SimplePool.Preload(boltPrefab, boltsContainer, 10);

			lifes = gameConfig.ShipLifes;
			initialPosition = transform.position;
		}

		public void ResetAll()
		{
			lifes = gameConfig.ShipLifes;
			rb.velocity = Vector3.zero;
			transform.position = initialPosition;
			myTime = 0.0F;
			kills = 0;
		}

		void Update()
		{
			myTime = myTime + Time.deltaTime;

			if (Input.GetButton("Fire1") && myTime > nextFire)
			{
				nextFire = myTime + fireRate;
				SpawnBolt();

				nextFire = nextFire - myTime;
				myTime = 0.0F;
			}
		}

		void SpawnBolt()
		{
			var bolt = SimplePool.Spawn(boltPrefab, shotSpawn.position, shotSpawn.rotation).GetComponent<Bolt>();
			bolt.HitEvent += OnBoltHit;
			audioSource.Play();
		}

		void FixedUpdate()
		{
			float moveHorizontal = Input.GetAxis("Horizontal");
			float moveVertical = Input.GetAxis("Vertical");

			Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
			rb.velocity = movement * speed;

			rb.position = new Vector3(
				Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
				0.0f,
				Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
			);

			rb.rotation = Quaternion.Euler(
				0.0f,
				0.0f,
				rb.velocity.x * -tilt
			);
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Enemy"))
			{
				lifes -= 1;
				LifeLostEvent?.Invoke(lifes);
				if (lifes <= 0)
				{
					Instantiate(explosionPrefab, transform.position, transform.rotation);
				}
			}
		}

		void OnBoltHit(Bolt bolt, bool isEnemy)
		{
			bolt.HitEvent -= OnBoltHit;

			if (!isEnemy)
				return;

			kills += 1;
			if (kills >= gameConfig.Levels[playerData.ProgressLevelIndex].AsteroidsToWin)
			{
				EnoughKillsEvent?.Invoke();
			}
		}
	}
}
