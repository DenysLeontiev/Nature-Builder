using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NatureSpawner : MonoBehaviour
{
    public static NatureSpawner Instance { get; private set; }

    private GameObject currentGameObjectIndicator;
    private bool isGameObjectIndicatorInstantiated;

    public class OnPlantChangedEventArgs : EventArgs
    {
        public PlantBase plantBase;
        public float timeBetweenSpawnMax;
        public float currentTimeBetweenSpawn;
    }

    public event EventHandler<OnPlantChangedEventArgs> OnPlantChanged;

    [SerializeField] private float yOffset = 0.5f;
    [SerializeField] private float radius = 0.05f;

    [SerializeField] private float minRotationY = 0f;
    [SerializeField] private float maxRotationY = 360f;

    private PlantBase currentPlantToSpawn;

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
        SpawnObject();
        ShowCurrentPlantVisual();
    }

    private void ShowCurrentPlantVisual()
    {
        if (currentPlantToSpawn == null)
        {
            return;
        }

        if (Physics.Raycast(GetRay(), out RaycastHit hit))
        {
            if (isGameObjectIndicatorInstantiated == false)
            {
                currentGameObjectIndicator = Instantiate(currentPlantToSpawn.GetPlantSO().TransparentObjectIndicator, hit.point, Quaternion.identity);
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
            if(currentGameObjectIndicator != null)
                currentGameObjectIndicator.SetActive(false);
            isGameObjectIndicatorInstantiated = false;
        }
    }

    public (PlantBase plantBase, float plantCurrentTimeBetweenSpawn) GetCurrentObjectAndTimeBetweenSpawn()
    {
        return (currentPlantToSpawn, currentTimeBetweenSpawn);
    }

    public void SetObjectToSpawn(PlantBase plant)
    {
        if(currentPlantToSpawn == plant)
        {
            return;
        }

        currentPlantToSpawn = plant;

        currentTimeBetweenSpawn = plant.GetPlantSO().delayBetweenSpawnTime;
        timeBetweenSpawnMax = plant.GetPlantSO().delayBetweenSpawnTime;

        isGameObjectIndicatorInstantiated = false;

        if(currentGameObjectIndicator != null)
            Destroy(currentGameObjectIndicator.gameObject);

        OnPlantChanged?.Invoke(this, new OnPlantChangedEventArgs { plantBase = plant,currentTimeBetweenSpawn = this.currentTimeBetweenSpawn, timeBetweenSpawnMax = this.timeBetweenSpawnMax });
    }

    private void SpawnObject()
    {
        if(currentPlantToSpawn == null)
        {
            return;
        }

        currentTimeBetweenSpawn -= Time.deltaTime;
        if (Input.GetMouseButtonUp(0) && currentTimeBetweenSpawn < 0f)
        {
            if (Physics.Raycast(GetRay(), out RaycastHit hit))
            {
                var spawnedObj = Instantiate(currentPlantToSpawn);
                Vector3 spawnPoint = hit.point;
                spawnPoint.y -= yOffset;
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
            }

            currentTimeBetweenSpawn = timeBetweenSpawnMax;
        }
    }

    private Ray GetRay()
    {
        return mainCamera.ScreenPointToRay(Input.mousePosition);
    }
}
