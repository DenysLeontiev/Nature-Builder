using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NatureSpawner;

public class AnimalSpawner : MonoBehaviour
{
	public static AnimalSpawner Instance { get; private set; }

	public class OnAnimalChangedEventArgs : EventArgs
	{
		public AnimalBase animalBase;
		public float timeBetweenSpawnMax;
		public float currentTimeBetweenSpawn;
	}

	public event EventHandler<OnAnimalChangedEventArgs> OnAnimalChanged;

	[Header("AvailabilityMaterials")]
	[SerializeField] private Material notAvailableMaterial;
	[SerializeField] private Material availableMaterial;

	[Header("SpawnAnimalOffset")]
	[SerializeField] private float yOffset = 0.5f;
	[SerializeField] private float radius = 0.05f;

	private AnimalBase currentAnimalToSpawn;

	private GameObject currentGameObjectIndicator;
	private bool isGameObjectIndicatorInstantiated;


	private Camera mainCamera;

	private float timeBetweenSpawnMax;
	private float currentTimeBetweenSpawn;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		mainCamera = Camera.main;
	}
	private void Update()
	{
		if (currentAnimalToSpawn == null)
		{
			return;
		}

		SpawnObject();
		ShowCurrentPlantVisual();
		HandlePrefabIndicatorVisuals();
	}

	private void HandlePrefabIndicatorVisuals()
	{
		if (currentGameObjectIndicator != null)
		{
			Collider[] colliders = Physics.OverlapSphere(currentGameObjectIndicator.transform.position, radius);

			MeshRenderer currentGameObjectIndicatorMeshRenderer = currentGameObjectIndicator.GetComponent<MeshRenderer>();

			foreach (Collider collider in colliders)
			{
				if (collider.transform.GetComponent<PlantBase>() != null)
				{
					currentGameObjectIndicatorMeshRenderer.material = notAvailableMaterial;
				}
				else
				{
					currentGameObjectIndicatorMeshRenderer.material = availableMaterial;
				}
			}
		}
	}

	private void ShowCurrentPlantVisual()
	{
		if (currentAnimalToSpawn != null)
		{
			if (Physics.Raycast(GetRay(), out RaycastHit hit))
			{
				if (isGameObjectIndicatorInstantiated == false)
				{
					currentGameObjectIndicator = Instantiate(currentAnimalToSpawn.GetAnimalSO().TransparentObjectIndicator, hit.point, Quaternion.identity);
					isGameObjectIndicatorInstantiated = true;
				}
				else
				{
					currentGameObjectIndicator.SetActive(true);
					currentGameObjectIndicator.transform.position = hit.point;
				}
			}
			else
			{
				if (currentGameObjectIndicator != null)
				{
					Destroy(currentGameObjectIndicator.gameObject);
				}
				isGameObjectIndicatorInstantiated = false;
			}
		}
	}

	public (AnimalBase animalBase, float animalCurrentTimeBetweenSpawn) GetCurrentObjectAndTimeBetweenSpawn()
	{
		return (currentAnimalToSpawn, currentTimeBetweenSpawn);
	}

	public void SetObjectToSpawn(AnimalBase animal)
	{
		if (currentAnimalToSpawn == animal)
		{
			return;
		}

		currentAnimalToSpawn = animal;

		currentTimeBetweenSpawn = animal.GetAnimalSO().DelayBetweenSpawnTime;
		timeBetweenSpawnMax = animal.GetAnimalSO().DelayBetweenSpawnTime;

		
		OnAnimalChanged?.Invoke(this, new OnAnimalChangedEventArgs { animalBase = animal, currentTimeBetweenSpawn = this.currentTimeBetweenSpawn, timeBetweenSpawnMax = this.timeBetweenSpawnMax });
	}

	private void SpawnObject()
	{
		currentTimeBetweenSpawn -= Time.deltaTime;
		if (Input.GetMouseButtonUp(0) && currentTimeBetweenSpawn < 0f)
		{
			if (Physics.Raycast(GetRay(), out RaycastHit hit))
			{
				var spawnedObj = Instantiate(currentAnimalToSpawn);
				Vector3 spawnPoint = hit.point;
				spawnPoint.y += yOffset;
				spawnedObj.transform.position = spawnPoint;

				Collider[] colliders = Physics.OverlapSphere(spawnedObj.transform.position, 0.1f);

				foreach (Collider collider in colliders)
				{
					if (collider.transform.GetComponent<PlantBase>() != null)
					{
						Destroy(spawnedObj.gameObject);
					}
				}

				ResetCurrentAnimalToSpawn();

			}
			currentTimeBetweenSpawn = timeBetweenSpawnMax;
		}
	}

	private void ResetCurrentAnimalToSpawn()
	{
		currentAnimalToSpawn = null;
		currentGameObjectIndicator.SetActive(false);
	}

	private Ray GetRay()
	{
		return mainCamera.ScreenPointToRay(Input.mousePosition);
	}

}
