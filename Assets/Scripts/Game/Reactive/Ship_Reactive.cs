using System;
using SpaceShooter;
using UniRx;
using UnityEngine;

namespace SpaceShooterReactive
{
	public class Ship_Reactive : MonoBehaviour
	{
		[SerializeField] float fireRate = 0.5F;
		[SerializeField] float speed = 10f;
		[SerializeField] float tilt = 5f;
		[SerializeField] Boundary boundary = null;
		[SerializeField] GameObject boltPrefab = null;
		[SerializeField] Transform shotSpawn = null;
		[SerializeField] GameObject explosionPrefab = null;
		[SerializeField] Transform boltsContainer = null;

		PlayerModel playerModel;
		float myTime = 0.0F;
		float nextFire = 0.5F;
		Rigidbody rb;
		AudioSource audioSource;

		public void Init()
		{
			playerModel = GameController_Reactive.Instance.PlayerModel;

			rb = GetComponent<Rigidbody>();
			audioSource = GetComponent<AudioSource>();

			SimplePool.Preload(boltPrefab, boltsContainer, 10);
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
			SimplePool.Spawn(boltPrefab, shotSpawn.position, shotSpawn.rotation).GetComponent<Bolt>();
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
				playerModel.Lifes.Value -= 1;
				playerModel.Lifes
					.ObserveEveryValueChanged(x => x.Value)
					.Subscribe(xs =>
					{
						if (xs > 0)
							return;

						Instantiate(explosionPrefab, transform.position, transform.rotation);
						Destroy(gameObject);
					})
					.AddTo(this);
			}
		}
	}
}