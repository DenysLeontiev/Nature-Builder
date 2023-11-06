using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private float yOffset = 0.5f;
    [SerializeField] private float radius = 0.05f;

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
                var obj = Instantiate(objectToSpawn);
                Vector3 spawnPoint = hit.point;
                spawnPoint.y -= yOffset;
                obj.transform.position = spawnPoint;

                Collider[] colliders = Physics.OverlapSphere(obj.transform.position, radius);

                foreach (Collider collider in colliders)
                {
                    if(collider.name.Contains("Grass"))
                    {
                        Destroy(obj.gameObject);
                    }
                    // Check if the collider belongs to a different GameObject
                    if (collider.gameObject != gameObject)
                    {
                        // Do something with the GameObject that is currently touching this one
                        Debug.Log("Currently touching: " + collider.gameObject.name);
                    }
                }
            }
        }
    }
}
