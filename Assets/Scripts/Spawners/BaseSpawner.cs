using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NatureSpawner;

public abstract class BaseSpawner : MonoBehaviour
{
	[Header("SpawnPlantOffset")]
	[SerializeField] protected float yOffset = 0.5f;
	[SerializeField] protected float radius = 0.05f;

	[Header("RandomPlantSpawnLocation")]
	[SerializeField] protected float minRotationY = 0f;
	[SerializeField] protected float maxRotationY = 360f;

	[Header("AvailabilityMaterials")]
	[SerializeField] protected Material notAvailableMaterial;
	[SerializeField] protected Material availableMaterial;

	protected GameObject currentGameObjectIndicator;
	protected bool isGameObjectIndicatorInstantiated;

	protected Camera mainCamera;

	protected float timeBetweenSpawnMax;
	protected float currentTimeBetweenSpawn;

	protected void Start()
	{
		mainCamera = Camera.main;
	}
	public abstract (IPlaceable plantBase, float plantCurrentTimeBetweenSpawn) GetCurrentObjectAndTimeBetweenSpawn();

	protected void ShowCurrentObjectVisual(IPlaceable currentObjectToSpawn)
	{
		if (currentObjectToSpawn != null)
		{
			if (Physics.Raycast(GetRay(), out RaycastHit hit))
			{
				if (isGameObjectIndicatorInstantiated == false)
				{
					currentGameObjectIndicator = Instantiate(currentObjectToSpawn.GetPlaceableSO().TransparentObjectIndicator, hit.point, Quaternion.identity);
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

	protected void SpawnObject(MonoBehaviour objectToSpawn)
	{
		currentTimeBetweenSpawn -= Time.deltaTime;
		if (Input.GetMouseButtonUp(0) && currentTimeBetweenSpawn < 0f)
		{
			if (Physics.Raycast(GetRay(), out RaycastHit hit))
			{
				var spawnedObj = Instantiate(objectToSpawn);
				Vector3 spawnPoint = hit.point;
				spawnPoint.y += yOffset;
				spawnedObj.transform.position = spawnPoint;

				float randomYRot = UnityEngine.Random.Range(minRotationY, maxRotationY);
				spawnedObj.transform.rotation = Quaternion.Euler(spawnedObj.transform.rotation.x, randomYRot, spawnedObj.transform.rotation.z);

				Collider[] colliders = Physics.OverlapSphere(spawnedObj.transform.position, radius);

				foreach (Collider collider in colliders)
				{
					if (collider.transform.GetComponent<PlantBase>() != null)
					{
						Destroy(spawnedObj.gameObject);
					}
				}

				ResetCurrentObjectToSpawn();

			}
			currentTimeBetweenSpawn = timeBetweenSpawnMax;
		}
	}

	protected void HandlePrefabIndicatorVisuals()
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

	protected abstract void ResetCurrentObjectToSpawn();

	protected Ray GetRay()
	{
		return mainCamera.ScreenPointToRay(Input.mousePosition);
	}
}
