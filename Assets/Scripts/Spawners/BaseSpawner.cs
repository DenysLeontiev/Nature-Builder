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
	public abstract (IPlaceable objBase, float plantCurrentTimeBetweenSpawn) GetCurrentObjectAndTimeBetweenSpawn();

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
				Vector3 spawnPoint = hit.point;
				spawnPoint.y += yOffset;

				if (IsValidSpawnPoint(spawnPoint))
				{
					var spawnedObj = Instantiate(objectToSpawn, spawnPoint, Quaternion.identity);

					float randomYRot = UnityEngine.Random.Range(minRotationY, maxRotationY);
					spawnedObj.transform.rotation = Quaternion.Euler(spawnedObj.transform.rotation.x, randomYRot, spawnedObj.transform.rotation.z);

					ResetCurrentObjectToSpawn();
				}
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
				if (collider.transform.GetComponent<IPlaceable>() != null)
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

	private bool IsValidSpawnPoint(Vector3 spawnPoint)
	{
		Collider[] colliders = Physics.OverlapSphere(spawnPoint, radius);

		foreach (Collider collider in colliders)
		{
			if (collider.GetComponent<IPlaceable>() != null)
			{
				return false;
			}
		}

		return true;
	}
}
