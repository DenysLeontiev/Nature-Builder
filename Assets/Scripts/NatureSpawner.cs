using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NatureSpawner : MonoBehaviour
{
    [SerializeField] private Grass currentObjectToSpawn;
    [SerializeField] private float yOffset = 0.5f;
    [SerializeField] private float radius = 0.05f;

    [SerializeField] private float minRotationY = 0f;
    [SerializeField] private float maxRotationY = 360f;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                var spawnedObj = Instantiate(currentObjectToSpawn);
                Vector3 spawnPoint = hit.point;
                spawnPoint.y -= yOffset;
                spawnedObj.transform.position = spawnPoint;

                float randomYRot = Random.Range(minRotationY, maxRotationY);
                spawnedObj.transform.rotation = Quaternion.Euler(spawnedObj.transform.rotation.x, randomYRot, spawnedObj.transform.rotation.z);

                Collider[] colliders = Physics.OverlapSphere(spawnedObj.transform.position, radius);

                foreach (Collider collider in colliders)
                {
                    if (collider.transform.GetComponent<Grass>() != null)
                    {
                        Destroy(spawnedObj.gameObject);
                    }
                }
            }
        }
    }
}
